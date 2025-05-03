
var datepicker;

$('.select2').select2();
InitialDatePicker();
InitialNumberInput();

let dataTable = $('#tbItems').DataTable({
    //"destroy": true,
    //"bFilter": true,
    //"sDom": 'fBtlpi',
    //'pagingType': 'numbers',
    //"ordering": true,
    "destroy": true,
    "bFilter": true,
    //"sDom": 'Btlpi',
    "sDom": 'tp',
    //"sDom": 'fBtlpi',
    "pagingType": 'numbers',
    "ordering": true,
    "autoWidth": false,
    "ajax": {
        "url": "/Sale/GetTempItemData",
        "type": "GET",
        "datatype": "json",
        "dataSrc": function (response) {
            let totalAmount = 0;

            // Iterate through each item to calculate the total price
            response.data.forEach(function (item) {
                totalAmount += item.itemprice * item.qty; // Calculate total
            });

            //Set sum amount
            $("#txtSummaryTHB").val(currencyFormat(totalAmount));

            return response.data; // Return the data for DataTable to use
        },
    },
    "columns": [
        { "data": "seq" },
        { "data": "itemid" },
        { "data": "itemname" },
        { "data": "qty" },
        { "data": "itemprice" },
        {
            "data": "seq",
            "render": function (data) {
                return "<a class='me-3' style='margin-left:5px' onclick=Delete(" + data + ")><img src='../assets/img/icons/delete.svg' alt='img'></a>";
            }
        }
    ],
    "language": {
        search: ' ',
        sLengthMenu: '_MENU_',
        searchPlaceholder: "ค้นหาข้อมูล...",
        info: "_START_ - _END_ of _TOTAL_ items",
        emptyTable: "ไม่พบข้อมูล.",
    },
    "order": [[0, "desc"]],
    "columnDefs": [
        {
            "targets": [1],
            "visible": false
        }
    ],
    "initComplete": function (settings, json) {
        // You can perform additional actions here if needed
        // Iterate through each item to calculate the total price
        //var totalAmount = 0;
        //json.data.forEach(function (item) {
        //    totalAmount += item.itemprice * item.qty; // Calculate total
        //});

        ////Set sum amount
        //console.log(totalAmount);
        //$("#txtSummaryTHB").val(currencyFormat(totalAmount));
    }
});

$(document).on('change', '.select2', function (e) {
    // Get the selected value
    var selectedValue = $(this).val();
    // Get the data-row attribute to identify the row
    var row = $(this).data('row');
    console.log($(this).data('name'));
    // Log the selected value for the current row (you can replace this with your desired logic)
    console.log("Row " + row + ": " + selectedValue);
    //ShowMessageInfo('Selected value :' + selectedValue);
});

$("#btnSave").on("click", function (e) {
    e.preventDefault();

    alert('Create Branch');
    var frmSelling = $("#frmSelling");
    frmSelling.validate();
    var isValid = frmSelling.valid();
    if (isValid) {
        $.validator.unobtrusive.parse(frmSelling);
        var data = $(frmSelling).serializeJSON();
        data.qty = 1;
        data = JSON.stringify(data);

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

                $.ajax({
                    type: 'POST',
                    url: '/Sale/SaveSellingItemByBarcode',
                    data: data,
                    contentType: 'application/json',
                    success: function (data) {
                        if (data.result) {
                            //popup.dialog('close');
                            ShowMessageSuccess(data.msg);
                            dataTable.ajax.reload();

                            ShowMessageSuccess(data.msg);
                            AlertSuccess(data.msg);
                            $("#txtSummaryTHB").val(0);
                            ResetForm();
                        }
                        else {
                            AlertError(data.msg);
                        }
                    }
                });
            }
            else if (result.dismiss === Swal.DismissReason.cancel) {
                //Code
                ShowMessageInfo('ยกเลิก');
            }
        });
    }
    
    
});
function ValidationEnglishKeyPress() {
    $("input[ID='txtItemCode']").on("keypress", function (event) {

        // Disallow anything not matching the regex pattern (A to Z uppercase, a to z lowercase and white space)
        // For more on JavaScript Regular Expressions, look here: https://developer.mozilla.org/en-US/docs/JavaScript/Guide/Regular_Expressions
        var englishAlphabetAndWhiteSpace = /[A-Za-z0-9]/g;

        // Retrieving the key from the char code passed in event.which
        // For more info on even.which, look here: http://stackoverflow.com/q/3050984/114029
        var key = String.fromCharCode(event.which);

        //alert(event.keyCode);

        // For the keyCodes, look here: http://stackoverflow.com/a/3781360/114029
        // keyCode == 8  is backspace
        // keyCode == 37 is left arrow
        // keyCode == 39 is right arrow
        // englishAlphabetAndWhiteSpace.test(key) does the matching, that is, test the key just typed against the regex pattern
        if (event.keyCode == 8 || event.keyCode == 37 || event.keyCode == 39 || englishAlphabetAndWhiteSpace.test(key)) {
            return true;
        }

        // If we got this far, just return false because a disallowed key was typed.
        return false;
    });
    $("input[ID='txtItemCode']").on("paste", function (e) {
        e.preventDefault();
    });
}

function CalculatePriceByPrice(price, name) {
    var res = name.split('[');
    var resIdx = res[1].split(']');

    var qty = $("input[name='outer-item-group[" + resIdx[0] + "][txtItemQty]']").val() | 0;
    var total = parseFloat(price) * qty;

    $("input[name='outer-item-group[" + resIdx[0] + "][txtAmount]']").val(total.toFixed(2));

    //Sum total amount
    var totalRow = parseInt($("#totalrow").val());
    var totalAmt = 0;
    for (var i = 0; i < totalRow; i++) {
        var txtAmt = $("input[name='outer-item-group[" + i + "][txtAmount]']").val() | 0;
        totalAmt += parseFloat(txtAmt);
    }
    $("#txtSummaryTHB").val(currencyFormat(totalAmt));

}

function CalculatePriceByKey(itemkey, name) {
    var res = name.split('[');
    var resIdx = res[1].split(']');
    //alert('index -> ' + resIdx[0]);
    //var curCode = $("input[name='" + name + "']").val();
    //Set new Rate
    var seen = {}; // Object to store encountered values
    var isDuplicate = false;
    $('.item-sale-repeater').find('select').each(function (e) {
        if (this.type == 'select-one') {
            if (this.value != '') {
                seen[this.value];
                if (seen[this.value]) {
                    // Duplicate found
                    isDuplicate = true;
                    return;
                }
                else {
                    seen[this.value] = true;
                }
            }
        }
    });
    if (isDuplicate) {
        //ShowMessageError('ขออภัย, ไม่สามารถระบุสินค้าชนิดเดียวกันได้!');
        $("select[name='outer-item-group[" + resIdx[0] + "][ddlSearchItem]']").val('').trigger('change.select2');
        //$("select[name='outer-item-group[" + resIdx[0] + "][ddlSearchItem]']").val('');
        $("input[name='outer-item-group[" + resIdx[0] + "][txtItemPrice]']").val('');
        $("input[name='outer-item-group[" + resIdx[0] + "][txtItemQty]']").val('');
        $("input[name='outer-item-group[" + resIdx[0] + "][txtAmount]']").val('');
        return;
    }

    var itemid = parseInt(itemkey) | 0;
    var branchid = parseInt($("#ddlBranch").val()) | 0;

    var searchdata = {
        itemid: parseInt(itemkey) | 0,
        branchid: parseInt($("#ddlBranch").val()) | 0,
    };
    var ajaxRequest = $.ajax({
        url: 'GetItemPriceByCriteria',
        async: true,
        type: 'POST',
        dataType: 'JSON',
        data: searchdata,
        success: function (response) {

            if (!response.result) {
                $("input[name='outer-item-group[" + resIdx[0] + "][txtItemPrice]']").val('');
                ShowMessageError(response.msg);
                return;
            }

            //Set item price
            $("input[name='outer-item-group[" + resIdx[0] + "][txtItemPrice]']").val(response.data.price);

            //Set item current qty
            $("input[name='outer-item-group[" + resIdx[0] + "][txtCurrentQty]']").val(response.data.qty);
            

            //Get & Re-check qty if is null
            var qty = $("input[name='outer-item-group[" + resIdx[0] + "][txtItemQty]']").val() | 0;
            if (isNaN(qty)) {
                qty = $("input[name='outer-item-group[" + resIdx[0] + "][txtItemQty]']").val();
            }

            //var curRate = $("input[name='outer-item-group[" + resIdx[0] + "][txtItemPrice]']").val();
            var total = parseFloat(response.data.price) * qty;
            $("input[name='outer-item-group[" + resIdx[0] + "][txtAmount]']").val(total.toFixed(2));

            //Sum total amount
            var totalAmt = 0;

            var totalRow = parseInt($("#totalrow").val());

            for (var i = 0; i < totalRow; i++) {
                var txtAmt = $("input[name='outer-item-group[" + i + "][txtAmount]']").val();
                totalAmt += parseFloat(txtAmt);
            }

            $("#txtSummaryTHB").val(currencyFormat(totalAmt));
        },
        failure: function (response) {
            ShowMessageError(response.msg);
        },
        error: function (response) {
            ShowMessageError(response.msg);
        }
    });
}

function CalculatePriceByQty(qty, name) {
    var res = name.split('[');
    var resIdx = res[1].split(']');

    var itemPrice = $("input[name='outer-item-group[" + resIdx[0] + "][txtItemPrice]']").val();
    var total = parseFloat(itemPrice) * qty;

    $("input[name='outer-item-group[" + resIdx[0] + "][txtAmount]']").val(total.toFixed(2));

    //Sum total amount
    var totalRow = parseInt($("#totalrow").val());
    var totalAmt = 0;
    for (var i = 0; i < totalRow; i++) {
        var txtAmt = $("input[name='outer-item-group[" + i + "][txtAmount]']").val() | 0;
        totalAmt += parseFloat(txtAmt);
    }
    $("#txtSummaryTHB").val(currencyFormat(totalAmt));
}

function OnSuccess(data) {
    if (data.result) {
        ShowMessageSuccess(data.msg);
        AlertSuccess(data.msg);
        $("#txtSummaryTHB").val(0);
        ResetForm();
    }
    else {
        ShowMessageError(data.msg);
    }
}

function ResetForm() {
    $('#frmSelling')[0].reset(); // [0] gets the DOM element from the jQuery object
    dataTable.clear().draw(); // Clear the table and redraw
}

$("#txtBarCode").keyup(function (event) {
    
    if (event.keyCode == 13) {
        if (!ValidateSellingBranchSelection()) {
            ShowMessageError("กรุณาเลือกสาขาก่อนทำรายการ ก่อนทำรายการ.");
            return;
        }
        var sBarCode = $("#txtBarCode").val();
        var data = { "barcode": sBarCode };
        $.ajax({
            type: 'POST',
            url: '/Sale/AddTempItemSellingBarcode',
            data: JSON.stringify(data),
            contentType: 'application/json',
            success: function (response) {
                if (response.result) {
                    ShowMessageSuccess(response.message);
                    dataTable.ajax.reload();

                    //Set sum amount
                    $("#txtSummaryTHB").val(currencyFormat(response.amount));
                }
                else {
                    AlertErrorNoTitle(response.message);
                }
                $("#txtBarCode").val('');
            }
        });
    }

});

function AddSellingBarcodeItem(form) {

    if (!ValidateTransferBranchSelection()) {
        ShowMessageError("กรุณาเลือกสาขา ก่อนทำรายการ.");
        return;
    }

    var sBarCode = $("#txtBarCode").val();
    var data = { "barcode": sBarCode };

    var frmAddOrderItem = $("#frmAddSellingBarcodeItem");
    frmAddOrderItem.validate();
    var isValid = frmAddOrderItem.valid();
    if (!isValid) {
        ShowMessageError('กรุณาระบุข้อมูลให้ถูกต้องก่อนทำรายการ');
        return;
    }
    $.ajax({
        type: 'POST',
        url: '/Sale/AddTempItemSellingBarcode',
        data: data,
        contentType: 'application/json',
        success: function (data) {
            if (data.result) {
                //popup.dialog('close');
                ShowMessageSuccess(data.message);
                dataTable.ajax.reload();
                
                //$('#frmAddOrderItem')[0].reset();

                //$('#mdlAddItem').modal('toggle');
                //$('#mdlAddItem').modal('hide');
                //$("#btnCloseMdl").click();

                //$("#sbarcode").val('');
            }
            else {
                AlertError(data.message);
            }
        }
    });
    return false;
}

function Delete(id) {
    console.log('Call => Delete => ' + id);
    Swal.fire({
        title: "ยืนยันการลบข้อมูล?",
        //text: "เมื่อลบข้อมูลแล้ว จะไม่สามารถทำการยกเลิกได้!",
        html: "<span class='text-danger'>เมื่อลบข้อมูลแล้ว จะไม่สามารถทำการยกเลิกได้!</span>",
        icon: 'warning',
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "ยืนยัน",
        confirmButtonClass: "btn btn-primary",
        cancelButtonText: "ยกเลิก",
        cancelButtonClass: "btn btn-danger ml-1",
        buttonsStyling: false,
    }).then(function (t) {
        if (t.value) {

            //Delete
            $.ajax({
                type: 'POST',
                url: '/Sale/DeleteTempItemSellingBarcode',
                dataType: 'JSON',
                data: { "seq": id },
                success: function (response) {
                    if (response.result) {

                        ShowMessageSuccess('ลบข้อมูลสำเร็จ');
                        HideLoading();

                        dataTable.ajax.reload();


                        //Set sum amount
                        $("#txtSummaryTHB").val(currencyFormat(response.amount));

                        //Set focus to txtBarCode
                        $('#txtBarCode').trigger('focus');
                    }
                    else {
                        //ShowMessageError(data.message);
                        ShowMessageError(response.message);
                        HideLoading();
                    }
                }
            });
        }
    });

}

function ValidateSellingBranchSelection() {

    //Validate tranfer type, from-branch, to-branch
    var sellingBranch = $('.ddl-ddlBranch').val();

    // Validate the selected value
    if (sellingBranch === undefined || sellingBranch === null || sellingBranch <= 0) {
        return false;
    }
    return true; // Return true if all validations pass
}