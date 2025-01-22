
var datatable;
var selectedItems = new Set(); // Use a Set to store selected item IDs for better management

InitialNumberInput();
$('.select2').select2();
$("#txtSumUserRefillQTY").attr('readonly', true);
$("#txtSumSystemTotalRefillQTY").attr('readonly', true);

var total = 0;
var sumUserRefilQty = 0;
var sumSystemRefilQty = 0;
var previousValue = 0;

var selectAllItems = "#select-all-items";
var checkboxItem = ":checkbox";

datatable = $("#tbItemInventoryTransfer").DataTable({
    "destroy": true,
    "bFilter": true,
    "sDom": 'fBtlpi',
    "pagingType": 'numbers',
    "ordering": true,
    "pageLength": 50,
    "autoWidth": false,
    "ajax": {
        "url": "/Inventory/GetItemInventoryTransfer",
        "type": "GET",
        "datatype": "json"
    },
    "columns": [
        {
            "data": { itemid: "itemid", "itemcode": "itemcode" },
            "render": function (data) {
                /*return "<label class='checkboxs'><input type='checkbox' class='select-item' name='select_itemid_" + data.itemid + "' value='" + data.itemid + "'><span class='checkmarks'></span></label>";*/
                //return '<input type="checkbox" name="id[]" value="' + $('<div/>').text(data).html() + '">';
                return "<label class='checkboxs'><input type='checkbox' class='select-item' name='select_itemid_" + data.itemid + "' value='" + data.itemid + "'><span class='checkmarks'></span></label>";
            }
        },
        { "data": "branchid" },
        { "data": "itemid" },
        {
            "data": { itemid: "itemid", refillqty: "refillqty", "itemcode": "itemcode" },
            "render": function (data) {
                return "<input class='itemid-refillqty' type='number' id='itemid_" + data.itemid + "' name='itemid_" + data.itemid + "' value='" + data.refillqty + "' onkeyup='if(this.value<0){this.value= this.value * -1}' min='1' />";
            }
        },
        { "data": "itemcode" },
        { "data": "itemname" },
        { "data": "brandname" },
        { "data": "qtyinstock" },
        {
            "data": { qtyinbranch: "qtyinbranch" },
            "render": function (data) {
                return "<span style='color:blue'>" + data.qtyinbranch + "</span>";
            }
        },
        {
            "data": { notifyminqty: "notifyminqty" },
            "render": function (data) {
                return "<span style='color:red;font-weight: bold'>" + data.notifyminqty + "</span>";
            }
        },
        { "data": "orderqty" },
        { "data": "refillqty" }
    ],
    select: {
        style: 'multi',
        selector: 'td:first-child',
        headerCheckbox: 'select-page'
    },
    "order": [[2, "asc"]],
    "columnDefs": [
        {
            "targets": [1, 2, 11],
            "visible": false
        }
    ],
    "language": {
        search: ' ',
        sLengthMenu: '_MENU_',
        searchPlaceholder: "ค้นหาข้อมูล...",
        info: "_START_ - _END_ of _TOTAL_ items",
        "emptyTable": "ไม่พบข้อมูล."
    },
    initComplete: (settings, json) => {
        $('.dataTables_filter').appendTo("#tbItemInventoryTransfer");
        $('.dataTables_filter').appendTo('.search-input');
    },
    buttons: [
        {
            extend: 'excelHtml5',
            title: 'รายงานโอนสินค้า',
            text: 'ดาวโหลดไฟล์ Excel',
            class: 'btn-primary',
            exportOptions: {
                columns: [0, 3, 4, 5, 6, 7, 8, 9, 11]
            }
        },
        {
            extend: 'pdfHtml5',
            title: 'PDF',
            text: 'Export to PDF'
        }
    ]
});

// Handle click on "Select all" control
$(selectAllItems).on('click', function () {

    // Check/uncheck all checkboxes in the table
    var rows = datatable.rows({ 'search': 'applied' }).nodes();
    $('input[type="checkbox"]', rows).prop('checked', this.checked);

    //var rowCount = datatable.rows().count();
    //if (rowCount == 0) {
    //    $('#txtSumUserRefillQTY').val('');
    //}
    //else {
    //    var totalSum = 0;
    //    if (this.checked) {
    //        totalSum = $('#tbItemInventoryTransfer').DataTable().column(11).data().sum();
    //        $('#txtSumUserRefillQTY').val(totalSum);
    //    }
    //    else {
    //        $('#txtSumUserRefillQTY').val(''); // Update the sum in the textbox
    //    }
    //    sumSystemRefilQty = totalSum;
    //}

    SumSelectedItemQty();
});

// Handle click on checkbox to set state of "Select all" control
$('#tbItemInventoryTransfer tbody').on('change', 'input[type="checkbox"]', function () {
    //var checkedItemID = parseInt(this.name.split("_")[2]);
    //var refillQty = $("input[name='itemid_" + checkedItemID + "']").val();
    //var qty = (isNaN(parseInt(refillQty))) ? 0 : parseInt(refillQty);

    //// If checkbox is not checked
    //if (!this.checked) {
    //    var el = $('#select-all-items').get(0);
    //    // If "Select all" control is checked and has 'indeterminate' property
    //    if (el && el.checked && ('indeterminate' in el)) {
    //        // Set visual state of "Select all" control
    //        // as 'indeterminate'
    //        el.indeterminate = true;
    //    }

    //    sumSystemRefilQty -= qty;
    //}
    //else {
    //    sumSystemRefilQty += qty;
    //}
    //$('#txtSumUserRefillQTY').val(sumSystemRefilQty);

    //var sumCol4 = sumColumn4();
    //$('#txtSumUserRefillQTY').val(sumCol4);
    SumSelectedItemQty();
});

$('#tbItemInventoryTransfer').on('input', '.itemid-refillqty', function (e) {
    SumSelectedItemQty();
});

$('#btnConfirmTransfer').on('click', function (e) {

    ShowLoading();
    e.preventDefault();
    var data = datatable.$('input, select').serialize();
    var object_update = {
        InventoryTransferDataList: datatable.rows()
            .data()
            .toArray()
            .map((el) => {
                //console.log(el.itemid);
                var txtRefillQty = datatable.$('input[name=itemid_' + el.itemid + '], select');
                var isCheck = datatable.$('input[name=select_itemid_' + el.itemid + '], select');
                el.ischeck = isCheck.is(":checked");
                el.refillqty = parseInt(txtRefillQty.val());
                return el;
            })
    }
    console.log(object_update);

    var reqData = { "detail": object_update.InventoryTransferDataList };
    var jsonData = JSON.stringify(reqData);
    console.log(jsonData);

    var request = $.ajax({
        type: 'POST',
        url: '/Inventory/ItemInvenrotyTransferValidation',
        data: jsonData,
        contentType: 'application/json',
        success: function (response) {

            if (response.result) {

                Swal.fire({
                    //title: 'ยืนยันการบันทึกข้อมูล?',
                    //text: 'กรุณาตรวจสอบข้อมูลก่อนทำการบันทึก!',
                    //type: 'warning',
                    title: '<strong>ยืนยันการบันทึกข้อมูล?</strong>',
                    icon: 'warning',
                    html: '<u><span style="color:red">กรุณาตรวจสอบข้อมูลก่อนทำการบันทึก!</span></u>',
                    showCancelButton: true,
                    //showDenyButton: true,
                    confirmButtonColor: '#04B431',
                    confirmButtonText: 'บันทึก',
                    cancelButtonColor: '#D33',
                    cancelButtonText: "ยกเลิก",
                    //denyButtonText: 'ยืนยัน-ไม่ออกใบเสร็จ',
                    //denyButtonColor: '#D33',
                    customClass: {
                        confirmButton: 'btn btn-success',
                        denyButton: 'btn btn-warning ml-1',
                        cancelButton: 'btn btn-danger ml-1'
                    },
                    buttonsStyling: false,
                    focusConfirm: true
                }).then(function (result) {
                    if (result.value) {
                        //Post: SaveInvenrotyTransfer
                        //ShowMessageSuccess(result.message);

                        var request = $.ajax({
                            type: 'POST',
                            url: '/Inventory/CreateItemInvenrotyTransfer',
                            data: jsonData,
                            contentType: 'application/json',
                            success: function (response) {

                                if (response.result) {
                                    ShowMessageSuccess(response.message);

                                    setTimeout(function () {
                                        window.location.href = "/Inventory/Index";
                                    }, 1000);
                                    //Update the DataTable with the filtered data from the server
                                    /*console.log(response.data);*/
                                    /*$("#tbItemTransferHistory").DataTable().clear().rows.add(response.data).draw();*/
                                }
                                else {
                                    AlertErrorNoTitle(response.message);
                                }

                                //console.log(response.data);
                                //$("#tblInventoryReport").DataTable().clear().rows.add(response.data).draw();
                                //HideLoading();
                            },
                            failure: function (response) {
                                AlertError(response.message);
                            },
                            error: function (response) {
                                AlertError(response.message);
                            }
                        });
                    }
                    else if (result.dismiss === Swal.DismissReason.cancel) {
                        //Code
                        ShowMessageInfo('ยกเลิก');
                    }
                });
            }
            else {
                AlertErrorNoTitle(response.message);
            }

            console.log(response.data);
            //$("#tblInventoryReport").DataTable().clear().rows.add(response.data).draw();
            HideLoading();
        },
        failure: function (response) {
            AlertError(response.message);
        },
        error: function (response) {
            AlertError(response.message);
        }
    });

});

$('#btnSaveDraft').on('click', function (e) {
    
    e.preventDefault();
    var data = datatable.$('input, select').serialize();
    var object_update = {
        InventoryTransferDataList: datatable.rows()
            .data()
            .toArray()
            .map((el) => {
                //console.log(el.itemid);
                var txtRefillQty = datatable.$('input[name=itemid_' + el.itemid + '], select');
                var isCheck = datatable.$('input[name=select_itemid_' + el.itemid + '], select');
                el.ischeck = isCheck.is(":checked");
                el.refillqty = parseInt(txtRefillQty.val());
                return el;
            })
    }
    console.log(object_update);

    var reqData = { "detail": object_update.InventoryTransferDataList };
    var jsonData = JSON.stringify(reqData);
    console.log(jsonData);

    Swal.fire({
        //title: 'ยืนยันการบันทึกข้อมูล?',
        //text: 'กรุณาตรวจสอบข้อมูลก่อนทำการบันทึก!',
        //type: 'warning',
        title: '<strong>ยืนยันการบันทึกข้อมูลฉบับร่าง?</strong>',
        icon: 'warning',
        html: '<u><span style="color:red">กรุณาตรวจสอบข้อมูลก่อนทำการบันทึกฉบับร่าง!</span></u>',
        showCancelButton: true,
        //showDenyButton: true,
        confirmButtonColor: '#04B431',
        confirmButtonText: 'บันทึก',
        cancelButtonColor: '#D33',
        cancelButtonText: "ยกเลิก",
        //denyButtonText: 'ยืนยัน-ไม่ออกใบเสร็จ',
        //denyButtonColor: '#D33',
        customClass: {
            confirmButton: 'btn btn-success',
            denyButton: 'btn btn-warning ml-1',
            cancelButton: 'btn btn-danger ml-1'
        },
        buttonsStyling: false,
        focusConfirm: true
    }).then(function (result) {
        if (result.value) {
            ShowLoading();
            var request = $.ajax({
                type: 'POST',
                url: '/Inventory/CreateDraftItemInvenrotyTransfer',
                data: jsonData,
                contentType: 'application/json',
                success: function (response) {

                    if (response.result) {
                        ShowMessageSuccess(response.message);
                        HideLoading();

                        //Update the DataTable with the filtered data from the server
                        /*console.log(response.data);*/
                        /*$("#tbItemTransferHistory").DataTable().clear().rows.add(response.data).draw();*/

                        window.location.href = '/Inventory/Index';
                    }
                    else {
                        AlertErrorNoTitle(response.message);
                    }

                    //console.log(response.data);
                    //$("#tblInventoryReport").DataTable().clear().rows.add(response.data).draw();
                    HideLoading();
                },
                failure: function (response) {
                    AlertError(response.message);
                },
                error: function (response) {
                    AlertError(response.message);
                }
            });
        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            //Code
            ShowMessageInfo('ยกเลิก');
            HideLoading();
        }

    });

});

$("#btnSearch").on('click', function (event) {
    ShowLoading();

    event.preventDefault(); // Prevent the default form submission

    var text = $("#ddlBranch :selected").text();
    var sbranchid = $("#ddlBranch :selected").val();
    var sbrandid = $("#ddlBrand :selected").val();

    var branchid = parseInt(sbranchid);
    var brandid = parseInt(sbrandid);

    var reqdata = { "branchid": branchid, "brandid": brandid };
    var jsonData = JSON.stringify(reqdata);
    //console.log(jsonData);
    var request = $.ajax({
        type: 'POST',
        url: '/Inventory/SearchInvenrotyTransferForTransfer',
        data: jsonData,
        contentType: 'application/json',
        success: function (response) {

            if (response.result) {
                ShowMessageSuccess(response.message);

                ResetControls();
                //Update the DataTable with the filtered data from the server
                /*console.log(response.data);*/
                /*$("#tbItemTransferHistory").DataTable().clear().rows.add(response.data).draw();*/
            }
            else {
                AlertErrorNoTitle(response.message);
                ResetControls();
            }

            $("#tbItemInventoryTransfer").DataTable().clear().rows.add(response.data).draw();
            $("#txtSumSystemTotalRefillQTY").val(response.totalrefillqty);
            HideLoading();
        },
        failure: function (response) {
            AlertError(response.message);
        },
        error: function (response) {
            AlertError(response.message);
        }
    });
});

$("#btnCancel").on('click', function (e) {
    window.location.href = '/Inventory/Index';
});

$(document).on('change', '.select2', function (e) {

    // Get the selected value
    var selectedValue = $(this).val();
    // Get the data-row attribute to identify the row
    var row = $(this).data('row');
    console.log($(this).data('name'));
    // Log the selected value for the current row (you can replace this with your desired logic)
    console.log("Row " + row + ": " + selectedValue);
});

function SearchTransferData(branchid, brandid) {

    var reqdata = { "branchid": branchid, "brandid": brandid };
    var jsonData = JSON.stringify(reqdata);
    console.log(jsonData);
    var request = $.ajax({
        type: 'POST',
        url: '/Inventory/SearchInvenrotyTransfer',
        data: jsonData,
        contentType: 'application/json',
        success: function (response) {

            if (response.result) {
                ShowMessageSuccess(response.message);

                //Update the DataTable with the filtered data from the server
                /*console.log(response.data);*/
                /*$("#tbItemTransferHistory").DataTable().clear().rows.add(response.data).draw();*/
            }
            else {
                AlertErrorNoTitle(response.message);
            }

            console.log(response.data);
            $("#tbItemInventoryTransfer").DataTable().clear().rows.add(response.data).draw();
            
        },
        failure: function (response) {
            AlertError(response.message);
        },
        error: function (response) {
            AlertError(response.message);
        }
    });
}

function CalculateTotalUserRefillQTYByInputName(inputname, ischecked) {
    var checkedItemID = parseInt(inputname.split("_")[2]);
    var txtRefillQty = datatable.$('input[name=itemid_' + checkedItemID + '], select');
    console.log('input[name=itemid_' + checkedItemID + ']' + txtRefillQty.val());

    var curSumUserRefilQty = (isNaN(parseInt(sumUserRefilQty))) ? 0 : parseInt(sumUserRefilQty);
    console.log('curSumUserRefilQty: ' + curSumUserRefilQty);

    var refillQtyValue = (isNaN(parseInt(txtRefillQty.val()))) ? 0 : parseInt(txtRefillQty.val());
    console.log('refillQtyValue: ' + refillQtyValue);

    if (ischecked) {
        sumUserRefilQty = curSumUserRefilQty + refillQtyValue;
        console.log('ischecked:true -> sumUserRefilQty: ' + sumUserRefilQty);
        $('#txtSumUserRefillQTY').val(sumUserRefilQty);
    }
    else {
        sumUserRefilQty = curSumUserRefilQty - refillQtyValue;
        console.log('ischecked:false -> sumUserRefilQty: ' + sumUserRefilQty);
        $('#txtSumUserRefillQTY').val(sumUserRefilQty);
    }
}

function CalculateTotalSystemRefillQTYByRow(row, ischecked) {
    //var row = $(this).closest('tr');
    var cValue = row.find("td:eq(8)").html()
    var curSystemRefilQty = (isNaN(parseInt(cValue))) ? 0 : parseInt(cValue)
    var totalSystemRefilQty = (isNaN(parseInt(sumSystemRefilQty))) ? 0 : parseInt(sumSystemRefilQty);

    if (ischecked) {
        sumSystemRefilQty = totalSystemRefilQty + curSystemRefilQty;
        $('#txtSumSystemTotalRefillQTY').val(sumSystemRefilQty);
    }
    else {
        sumSystemRefilQty = totalSystemRefilQty - curSystemRefilQty;
        $('#txtSumSystemTotalRefillQTY').val(sumSystemRefilQty);
    }
}

// Function to get the sum of values in column 4 (assuming numeric data in textbox)
function SumColumn4() {
    var totalSum = 0;

    // Iterate through each row
    datatable.rows().every(function () {
        // Find the textbox in the current row (column index 4)
        var quantityInput = this.node().querySelector('input.itemid-refillqty'); // Adjust the selector if necessary
        if (quantityInput) {
            var value = parseFloat(quantityInput.value) || 0; // Get the textbox value, default to 0 if NaN
            //console.log('value: ' + value);
            totalSum += value; // Add the value to the sum
        }
    });
    return totalSum; // Return the total sum
}

// Function to get the sum of values in column 11 (assuming numeric data)
function SumColumn11() {
    var column11Sum = $('#tbItemInventoryTransfer').DataTable().column(11).data().sum();
    return column11Sum; // Return the total sum
}

// Function to get selected item quantities
function SumSelectedItemQty() {
    var selectedQuantities = [];
    var sumSystemRefilQty = 0;
    var count = 0;

    datatable.rows().every(function (rowIdx, tableLoop, rowLoop) {
        var quantityInput = this.node().querySelector('input.itemid-refillqty'); // Adjust the selector if necessary
        if (quantityInput) {

            var value = parseInt(quantityInput.value) || 0; // Get the textbox value, default to 0 if NaN
            //console.log('value: ' + value);
            //totalSum += value; // Add the value to the sum

            var checkbox = this.node().querySelector('.select-item'); //
            // If the associated select-item checkbox is checked
            if (checkbox.checked) {
                //console.log('quantity' + quantity);
                selectedQuantities.push(value);
            }
        }
    });

    var sum = 0;
    $.each(selectedQuantities, function () { sum += parseInt(this) || 0; });
    console.log('[getSelectedItemQuantities] Total: ' + sum);
    if (sum == 0) {
        $('#txtSumUserRefillQTY').val('');
    }
    else {
        $('#txtSumUserRefillQTY').val(sum);
    }
}

function ResetControls() {
    $("#select-all-items").prop('checked', false);
    $("#txtSumUserRefillQTY").val('');
}