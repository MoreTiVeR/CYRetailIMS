
var datatable;
InitialNumberInput();
$('.select2').select2();
$("#txtSumUserRefillQTY").attr('readonly', true);
$("#txtSumSystemTotalRefillQTY").attr('readonly', true);

var total = 0;
var sumUserRefilQty = 0;
var sumSystemRefilQty = 0;

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
                console.log('render columns : checkbox');
                return "<label class='checkboxs'><input type='checkbox' id='select-all-items' class='select-item' name='select_itemid_" + data.itemid +"'><span class='checkmarks'></span></label>";
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
        { "data": "qtyinbranch" },
        { "data": "notifyminqty" },
        { "data": "orderqty" },
        { "data": "refillqty" }
        //{
        //    "data": { itemid: "itemid", refillqty: "refillqty" },
        //    "render": function (data) {
        //        console.log('refillqty: ' + data.refillqty);
        //        return "<label class='checkboxs'><input type='checkbox' id='select-all' name='select_itemid_" + data.itemid + "'><span class='checkmarks'></span></label>";
        //        //return "<lable id='refillqty_itemid_" + data.itemid + "' name='refillqty_itemid_" + data.itemid + "'>" + data.refillqty + "</lable>";
        //    }
        //},
    ],
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
    //rowCallback: function (row, data) {
    //    //alert(row);
    //    //$(row).css("cursor", "pointer");
    //    var ccc = $(row).find('.itemid-refillqty').val();
    //    console.log('ccc -> ' + ccc);

    //    $(row).on("change", ".itemid-refillqty", function () {
    //        var input = $(this);
    //        //console.log(row);
    //        //console.log(data);

    //        //getting the previous value
    //        var previousValue = $(this).data("val");
    //        console.log($(this).data());

    //        //get current value
    //        var curValue = input.val();
    //        console.log('curValue -> ' + curValue);

    //        //new value
    //        var newValue = $(row).find('.itemid-refillqty').val();
    //        console.log('newValue -> ' + newValue);

    //        //final value
    //        var finalVal = newValue - curValue;
    //        console.log('finalVal -> ' + finalVal);

    //        //set new value
    //        //var intNewValue = (isNaN(parseInt(newValue))) ? 0 : parseInt(newValue)
    //        //$(this).val(intNewValue);


    //        var isCheck = $(row).find('#select-all-items').is(':checked');
    //        if (isCheck) {
                
    //            var name = "select_itemid_" + data.itemid;
    //            //console.log('name: ' + name);
    //            alert(name);
    //            var systemRefilQty = (isNaN(parseInt(data.refillqty))) ? 0 : parseInt(data.refillqty);

    //            var newValue = $(row).find('.itemid-refillqty').val();
    //            var intNewValue = (isNaN(parseInt(newValue))) ? 0 : parseInt(newValue);

    //            var minus = intNewValue - systemRefilQty;

    //            var name2 = "#itemid_" + data.itemid;
    //            //$(name2).val(intNewValue);

    //            //CalculateTotalUserRefillQTYByInputName(name, isCheck);
    //            CalculateTotalUserRefillQTYByInputNameAndMinusValue(name, isCheck);
    //        }
    //        //console.log(ddd);

    //        //console.log(data);
    //        //console.log(data.refillqty);
    //        //var input = $(this);
    //        //console.log($(this).val());
    //        //var lastval = input.data("lastval");
    //        //console.log('lastval -> ' + lastval);
    //        //input.val(input.val());
    //    });
    //    //$('.itemid-refillqty').bind("input", function (event) {
    //    //    console.log('success');
    //    //});
    //},
    /*dom: 'Bfrtip',*/
    buttons: [
        {
            extend: 'excelHtml5',
            title: 'รายงานโอนสินค้า',
            text: 'ดาวโหลดไฟล์ Excel',
            class: 'btn-primary',
            //Columns to export
            exportOptions: {
                columns: [0, 3, 4, 5, 6, 7, 8, 9, 11]
            }
        },
        {
            extend: 'pdfHtml5',
            title: 'PDF',
            text: 'Export to PDF'
            //Columns to export
            //exportOptions: {
            //     columns: [0, 1, 2, 3, 4, 5, 6]
            //  }
        }
    ]
});

$('#tbItemInventoryTransfer').on('input', '.itemid-refillqty', function () {
    updateTotal();
});

// Function to update total
function updateTotal() {
    total = 0; // reset total
    $('#tbItemInventoryTransfer tbody tr').each(function () {
        var $row = $(this);
        //var checkbox = $row.find('.select-item');
        var isCheck = $row.find('#select-all-items').is(':checked');
        //var value = parseFloat($row.find('.itemid-refillqty').val()) || 0; // get new value
        var value = (isNaN(parseInt($row.find('.itemid-refillqty').val()))) ? 0 : parseInt($row.find('.itemid-refillqty').val());

        // Only add if checkbox is checked
        if (isCheck) {
            total += Math.abs(value);
        }
    });

    console.log('recalculate total: ' + Math.abs(total));
    // Update total display
    $('#txtSumUserRefillQTY').val(Math.abs(total));
}

function updateTotal(row) {
    total = 0; // reset total
    $('#tbItemInventoryTransfer tbody tr').each(function () {
        var $row = $(this);
        //var checkbox = $row.find('.select-item');
        var isCheck = $row.find('#select-all-items').is(':checked');
        //var value = parseFloat($row.find('.itemid-refillqty').val()) || 0; // get new value
        var value = (isNaN(parseInt($row.find('.itemid-refillqty').val()))) ? 0 : parseInt($row.find('.itemid-refillqty').val());

        // Only add if checkbox is checked
        if (isCheck) {
            total += Math.abs(value);
        }
    });

    console.log('recalculate total: ' + Math.abs(total));
    // Update total display
    $('#txtSumUserRefillQTY').val(Math.abs(total));
}

$(document).on('change', '.select2', function (e) {

    // Get the selected value
    var selectedValue = $(this).val();
    // Get the data-row attribute to identify the row
    var row = $(this).data('row');
    console.log($(this).data('name'));
    // Log the selected value for the current row (you can replace this with your desired logic)
    console.log("Row " + row + ": " + selectedValue);
});

$(selectAllItems).on('click', function () {
    console.log('reset counter');
    sumUserRefilQty = 0;
    sumSystemRefilQty = 0;
    if (this.checked) {
        $(checkboxItem).each(function (e) {
            this.checked = true;
            CalculateTotalUserRefillQTYByInputName(this.name, this.checked);
            CalculateTotalSystemRefillQTYByRow($(this).closest('tr'), this.checked);
        });
    } else {
        $(checkboxItem).each(function () {
            this.checked = false;
        });
    }
});

$(document).on('change', ':checkbox', function (e) {

    //Get data from row at column จำนวนที่ต้องเติม
    var row = $(this).closest('tr');
    CalculateTotalSystemRefillQTYByRow(row, $(this).is(':checked'));

    //จำนวนที่เติม edit by user
    //CalculateTotalUserRefillQTYByInputName(this.name, $(this).is(':checked'));
    updateTotal();

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

    //$.post("InvenrotyTransferDataValidation", { jsonData }).then(
    //    function (results) {

    //        if (results.result) {
    //            console.log(results.msg);
    //            Swal.fire({
    //                //title: 'ยืนยันการบันทึกข้อมูล?',
    //                //text: 'กรุณาตรวจสอบข้อมูลก่อนทำการบันทึก!',
    //                //type: 'warning',
    //                title: '<strong>ยืนยันการบันทึกข้อมูล?</strong>',
    //                icon: 'warning',
    //                html: '<u><span style="color:red">กรุณาตรวจสอบข้อมูลก่อนทำการบันทึก!</span></u>',
    //                showCancelButton: true,
    //                //showDenyButton: true,
    //                confirmButtonColor: '#04B431',
    //                confirmButtonText: 'บันทึก',
    //                cancelButtonColor: '#D33',
    //                cancelButtonText: "ยกเลิก",
    //                //denyButtonText: 'ยืนยัน-ไม่ออกใบเสร็จ',
    //                //denyButtonColor: '#D33',
    //                customClass: {
    //                    confirmButton: 'btn btn-success',
    //                    denyButton: 'btn btn-warning ml-1',
    //                    cancelButton: 'btn btn-danger ml-1'
    //                },
    //                buttonsStyling: false,
    //                focusConfirm: true
    //            }).then(function (result) {
    //                if (result.value) {
    //                    //Post: SaveInvenrotyTransfer
    //                    ShowMessageSuccess('Post: SaveInvenrotyTransfer');

    //                    var request = $.ajax({
    //                        type: 'POST',
    //                        url: '/Inventory/SaveInvenrotyTransfer',
    //                        data: jsonData,
    //                        contentType: 'application/json',
    //                        success: function (response) {

    //                            if (response.result) {
    //                                ShowMessageSuccess(response.message);

    //                                //Update the DataTable with the filtered data from the server
    //                                /*console.log(response.data);*/
    //                                /*$("#tbItemTransferHistory").DataTable().clear().rows.add(response.data).draw();*/
    //                            }
    //                            else {
    //                                AlertErrorNoTitle(response.message);
    //                            }

    //                            console.log(response.data);
    //                            $("#tblInventoryReport").DataTable().clear().rows.add(response.data).draw();
    //                            HideLoading();
    //                        },
    //                        failure: function (response) {
    //                            AlertError(response.message);
    //                        },
    //                        error: function (response) {
    //                            AlertError(response.message);
    //                        }
    //                    });
    //                }
    //                else if (result.dismiss === Swal.DismissReason.cancel) {
    //                    //Code
    //                    ShowMessageInfo('ยกเลิก');
    //                }
    //            });
    //        }
    //        else {
    //            ShowMessageError(results.msg);
    //            return;
    //        }

    //    }, function (results) {
    //        //Failed
    //        console.log('Failed');
    //        ShowMessageError(results.message);

    //    }, function () {
    //        ShowMessageError('Unknow error => Create Sale data.');
    //        console.log('this will run if the deferred generates a progress update.');
    //    }
    //);

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

                //Update the DataTable with the filtered data from the server
                /*console.log(response.data);*/
                /*$("#tbItemTransferHistory").DataTable().clear().rows.add(response.data).draw();*/
            }
            else {
                AlertErrorNoTitle(response.message);
            }

            $("#tbItemInventoryTransfer").DataTable().clear().rows.add(response.data).draw();
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
