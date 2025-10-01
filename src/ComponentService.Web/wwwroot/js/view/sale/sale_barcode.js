
var datepicker;

$('.ddl-ddlBranch').select2();
$('.ddl-transactiontype').select2();
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

    var frmSelling = $("#frmSelling");
    frmSelling.validate();
    var isValid = frmSelling.valid();
    if (isValid) {
        $.validator.unobtrusive.parse(frmSelling);
        var data = $(frmSelling).serializeJSON();

        // Access the role ID from the userProfile object in JavaScript
        var userRoleId = document.getElementById("uRoleID").value;

        // Now you can use userRoleId in your JavaScript code
        if (userRoleId == 2) {
            //Role Sale
            CreateSellingTransactionDataBySale(data);
        }
        else {
            //Admin or Other
            CreateSellingTrasnactionDataByAdmin(data);
        }
        
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
    InitialDatePicker();
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

function CreateSellingTrasnactionDataByAdmin(data) {
    Swal.fire({
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
        focusConfirm: true,
        didOpen: function () {
            //Initial
        },
        reverseButtons: true // this will put Cancel on left, Confirm on right
    }).then(function (result) {

        if (result.isConfirmed) {

            //Prepare Request Data and Direct create transaction if is admin
            data.qty = 1;
            data = JSON.stringify(data);
            CreateData(data);
        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            //Code
            ShowMessageInfo('ยกเลิก');
        }
    });
}

function CreateSellingTransactionDataBySale(data) {
    Swal.fire({
        title: '<u><strong>เลือกประเภทการจ่ายเงิน</strong></u>',
        showCancelButton: true,
        confirmButtonColor: '#04B431',
        confirmButtonText: '<i class="fa fa-money-bill" aria-hidden="true"></i> เงินสด',
        cancelButtonColor: '#D33',
        cancelButtonText: '<i class="fa fa-university" aria-hidden="true"></i> เงินโอน',
        customClass: {
            confirmButton: 'btn-lg btn-icon btn-primary',
            denyButton: 'btn btn-warning ml-1',
            cancelButton: 'btn-lg btn-icon btn-success'
        },
        buttonsStyling: false,
        focusConfirm: true,
        didOpen: function () {
            //Initial other
        }
    }).then(function (result) {

        if (result.isConfirmed) {
            //เงินสด
            data.qty = 1;
            data.transactiontype = 3;
            data.iscash = true;
            data.version = "2";
            data = JSON.stringify(data);
            //CreateData(data);
            //ShowPrintReceipt(data);

            (async () => {
                try {
                    const result = await ShowPrintReceipt(data);
                    console.log("✅ Printing finished:", result);
                } catch (err) {
                    console.error("❌ Printing failed:", err.message);
                }
            })();
            // Remove the event listener after printing
            //document.getElementById('btnPrintReceipt').removeEventListener('click', ShowPrintReceipt);
        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            //เงินโอน
            data.qty = 1;
            data.transactiontype = 3;
            data.iscash = false;
            data.version = "2";
            data = JSON.stringify(data);
            //CreateData(data);
            //ShowPrintReceipt(data);
            (async () => {
                try {
                    const result = await ShowPrintReceipt(data);
                    console.log("✅ Printing finished:", result);
                } catch (err) {
                    console.error("❌ Printing failed:", err.message);
                }
            })();
            // Remove the event listener after printing
            //document.getElementById('btnPrintReceipt').removeEventListener('click', ShowPrintReceipt);
        }
    });
}

function CreateData(objData) {
    
    $.ajax({
        type: 'POST',
        url: '/Sale/SaveSellingItemByBarcode',
        data: objData,
        contentType: 'application/json',
        success: function (data) {
            if (data.result) {

                ShowMessageSuccess(data.msg);
                dataTable.ajax.reload();

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


async function CreateDataAsync(objData) {
    try
    {
        const response = await $.ajax({
            type: 'POST',
            url: '/Sale/SaveSellingItemByBarcode',
            data: objData, // stringify ถ้าเป็น object
            contentType: 'application/json'
        });

        if (response.result) {
            ShowMessageSuccess(response.msg);
            dataTable.ajax.reload();

            AlertSuccess(response.msg);
            $("#txtSummaryTHB").val(0);
            ResetForm();
        } else {
            ShowMessageError(response.msg);
        }
        return response; // important: return response ไปให้ await ใช้งาน
    } catch (error) {
        ShowMessageError(error.statusText || 'เกิดข้อผิดพลาด');
        throw error; // ส่ง error กลับไปเพื่อให้ .catch ใช้ได้
    }
}

//function ShowPrintReceipt(objData) {
//    ShowLoading();

//    $.ajax({
//        url: "/Sale/GenerateReceiveSlipText",
//        type: "POST",
//        data: objData,
//        contentType: "application/json; charset=utf-8",
//        success: function (res) {
//            if (res.result) {
//                $("body").append(res.msg);
//                $("#print-receipt").modal("show");
//                HideLoading();

//                document.getElementById('btnPrintReceipt').addEventListener('click', async function handler()
//                {
//                    try
//                    {
//                        ShowLoading();
//                        const result = await CreateDataAsync(objData); // ถ้า CreateData return promise
//                        if (result && result.result === true) {
//                            console.log('PrintReceipt Success');

//                            $('#print-receipt').modal('hide');

//                            // ใช้ Response จากการบันทึกข้อมูล Sale/GenerateReceiveSlip
//                            console.log('Printer Command and Printer Name:' + res.printername);
//                            console.log(res.text);
//                            //console.log(res.printername);

//                            await SendPOSCommand(res.text, res.printername);

//                        } else {
//                            console.log('PrintReceipt Failed');
//                            AlertError(result?.msg || 'ไม่สามารถบันทึกข้อมูลได้');
//                        }
//                        HideLoading();
//                    } catch (error) {
//                        console.log('PrintReceipt Error');
//                        AlertError(error.message || 'เกิดข้อผิดพลาด');
//                        HideLoading();
//                    } finally {
//                        HideLoading();
//                        // ✅ remove event listener after execution
//                        document.getElementById('btnPrintReceipt').removeEventListener('click', handler);
//                    }
//                });
//            } else {
//                AlertError(res.msg);
//                HideLoading();
//            }
//        }
//    });

//}

function ShowPrintReceipt(objData) {
    return new Promise((resolve, reject) => {
        ShowLoading();

        $.ajax({
            url: "/Sale/GenerateReceiveSlipText",
            type: "POST",
            data: objData,
            contentType: "application/json; charset=utf-8",
            success: function (res) {
                if (!res.result) {
                    HideLoading();
                    AlertError(res.msg);
                    reject(new Error(res.msg));
                    return;
                }

                $("body").append(res.msg);
                $("#print-receipt").modal("show");
                HideLoading();

                // ✅ Capture values
                const printerName = res.printername;
                const printerText = res.text;

                // Reset old handlers before binding new
                $("#btnPrintReceipt").off('click').on('click', async function () {
                    try {
                        ShowLoading();

                        const result = await CreateDataAsync(objData);

                        if (result && result.result === true) {
                            console.log('PrintReceipt Success');
                            $('#print-receipt').modal('hide');

                            console.log('Printer Command and Printer Name:', printerName);
                            console.log(printerText);

                            await SendPOSCommand(printerText, printerName);

                            resolve({ success: true, printerName, printerText });
                        } else {
                            console.log('PrintReceipt Failed');
                            AlertError(result?.msg || 'ไม่สามารถบันทึกข้อมูลได้');
                            reject(new Error(result?.msg || 'PrintReceipt Failed'));
                        }
                    } catch (error) {
                        console.log('PrintReceipt Error');
                        AlertError(error.message || 'เกิดข้อผิดพลาด');
                        reject(error);
                    } finally {
                        HideLoading();
                    }
                });

                
            },
            error: function (xhr, status, err) {
                HideLoading();
                reject(err || new Error("AJAX error: " + status));
            }
        });
    });
}

// ✅ Clean up when modal closes without printing
$(document).on('hidden.bs.modal', '#print-receipt', function () {
    $(this).remove();   // ✅ remove modal completely
});

function startJSPM() {
    if (!window.JSPM || !JSPM.JSPrintManager) { setTimeout(startJSPM, 200); return; }
    JSPM.JSPrintManager.auto_reconnect = true;
    JSPM.JSPrintManager.start();
}
startJSPM();

async function SendPOSCommand(cmds, printername) {
    try {
        // Start JSPM
        await JSPM.JSPrintManager.start();

        if (!window.JSPM || !JSPM.ClientPrintJob) {
            console.error('PrintManager not available.');
            return;
        }

        // Create print job
        const cpj = new JSPM.ClientPrintJob();
        const myPrinter = new JSPM.InstalledPrinter(printername);
        cpj.clientPrinter = myPrinter;

        // Prepare ESC/POS commands
        const escpos = Neodynamic.JSESCPOSBuilder;
        const doc = new escpos.Document();

        const escposCommands = doc
            .font(escpos.FontFamily.A)   // Font A
            .size(0, 0)                  // Normal size
            .setCharacterCodeTable(255)  // Codepage 874 for Thai
            .text(cmds, 874)             // Use cmds as the text
            //.feed(2)
            //.cut()
            .generateUInt8Array();

        console.log('ESC/POS Commands:', escposCommands);

        // Assign commands
        cpj.binaryPrinterCommands = escposCommands;

        // Send to printer
        await cpj.sendToClient();

        console.log('Print job sent successfully.');
    } catch (e) {
        console.error('[ERROR] SendPOSCommand ->', e);
        //DISABLE
        //if (typeof AlertError === "function") AlertError(e);
    }
}
