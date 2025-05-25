
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
        "url": "/Sale/GetTempItemDataMobile",
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

$("#btnSave").on("click", function (e) {
    e.preventDefault();

    var frmSelling = $("#frmSelling");
    frmSelling.validate();
    var isValid = frmSelling.valid();
    if (isValid) {
        $.validator.unobtrusive.parse(frmSelling);
        var data = $(frmSelling).serializeJSON();
        //data.qty = 1;
        //data.iscash = true;
        //data = JSON.stringify(data);

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
            url: '/Sale/AddTempItemSellingMobileBarcode',
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
                url: '/Sale/DeleteTempItemSellingMobileBarcode',
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
        }
    }).then(function (result) {

        if (result.isConfirmed) {

            //Prepare Request Data
            data.qty = 1;
            data.transactiontype = 4;
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
            data.transactiontype = 4;
            data.iscash = true;
            data = JSON.stringify(data);
            CreateData(data);
        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            //เงินโอน
            data.qty = 1;
            data.transactiontype = 4;
            data.iscash = false;
            data = JSON.stringify(data);
            CreateData(data);
        }
    });
}

function CreateData(objData) {
    $.ajax({
        type: 'POST',
        url: '/Sale/SaveSellingItemByMobileBarcode',
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

async function AddItemDataList(barcode)
{    
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: '/Sale/AddTempItemSellingMobileBarcode',
            data: JSON.stringify({ barcode }),
            contentType: 'application/json',
            success: function (response) {
                if (response.result) {
                    ShowMessageSuccess(response.message);
                    dataTable.ajax.reload();
                    $("#txtSummaryTHB").val(currencyFormat(response.amount));
                    resolve();
                } else {
                    ShowMessageError(response.message);
                    reject(response.message);
                }
            },
            error: function (xhr, status, error) {
                reject(error);
                ShowMessageError(error);
            }
        });
    });
}

function CheckExistItemDataByMobileBarcode(barcode) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: '/Sale/AddTempItemSellingMobileBarcode',
            data: JSON.stringify({ barcode }),
            contentType: 'application/json',
            success: function (response) {
                if (response.result) {
                    ShowMessageSuccess(response.message);
                    dataTable.ajax.reload();
                    $("#txtSummaryTHB").val(currencyFormat(response.amount));
                    resolve();
                } else {
                    AlertErrorNoTitle(response.message);
                    reject(response.message);
                }
            },
            error: function (xhr, status, error) {
                reject(error);
                AlertErrorNoTitle(error);
            }
        });
    });
}

//Camera Func
// Initialize ZXing Barcode Reader
const codeReader = new ZXing.BrowserMultiFormatReader();
let selectedDeviceId;
let scannerRunning = false;

$('#mdlMobileScannerV2').on('shown.bs.modal', async function () {
    ShowMessageInfo("Initial Camera v3.0.5")
    $('#scanner-loading').show();
    $('#result').text('');

    try {

        const videoInputDevices = await codeReader.listVideoInputDevices();

        // Prefer back camera if available
        if (videoInputDevices.length > 0) {
            const backCamera = videoInputDevices.find(device => /back|rear|environment/i.test(device.label)) || videoInputDevices[0];
            selectedDeviceId = backCamera.deviceId;
        }
        else {
            document.getElementById('scanner-loading').style.display = 'none';
            throw new Error("ไม่พบกล้องบนอุปกรณ์");
        }

        // Start scanning
        startScanner();
        $('#scanner-loading').hide();
    } catch (err) {
        $('#scanner-loading').hide();
        $('#result').text('เกิดข้อผิดพลาด: ' + err.message);
    }
});

$('#mdlMobileScannerV2').on('hidden.bs.modal', function () {
    if (codeReader) {
        codeReader.reset();
        scannerRunning = false;
    }
});

// Start scanning
async function startScanner() {
    if (!selectedDeviceId) {
        const videoInputDevices = await codeReader.listVideoInputDevices();

        // Prefer back camera if available
        if (videoInputDevices.length > 0) {
            const backCamera = videoInputDevices.find(device => /back|rear|environment/i.test(device.label)) || videoInputDevices[0];
            selectedDeviceId = backCamera.deviceId;
        }
        else {
            document.getElementById('scanner-loading').style.display = 'none';
            throw new Error("ไม่พบกล้องบนอุปกรณ์");
        }
    }

    scannerRunning = true;
    await codeReader.decodeFromVideoDevice(selectedDeviceId, 'video', async (result, err) => {
        if (result && scannerRunning) {

            // handle result
            //scannerRunning = false;
            //codeReader.reset();

            const barcode = result.text;
            $('#result').text("พบบาร์โค้ด: " + barcode);

            alert("พบบาร์โค้ด: " + barcode);
            try {
                //await AddItemDataList(barcode);
                HandleScanResult(barcode);
            } catch (error) {
                console.error('Scan process failed:', error);
            }
        }
    });
}

// Stop scanning
function stopScanner() {
    scannerRunning = false;
    codeReader.reset();
}

// Resume scanning
function resumeScanner() {
    startScanner();
}

function HandleScanResult(qrCodeMessage) {
    console.log("Scanned:", qrCodeMessage);

    const jsonData = JSON.stringify({ barcode: qrCodeMessage });

    $.ajax({
        url: "/Sale/IsExistItemDataByMobileBarcode",
        type: "POST",
        contentType: "application/json", // Set the content type to JSON
        data: jsonData
    }).done(function (res) {
        // Handle the server response

        if (res.result) {
            //มีรายการเพิ่มอยู่แล้ว
            //Pasue camera
            console.log(res.message);
            HandleDuplicateItem(qrCodeMessage, res.message);
        }
        else {
            //รายการใหม่ ยังไม่ซ้ำ ยังไม่ได้สแกน
            AddItemDataList(qrCodeMessage).then(() => {
                // Handle success if needed

            }).catch((error) => {
                // Handle error from AddItemDataList if needed
                ShowMessageError(error);
            }).finally(() => {
                console.log("Scan completed."); // Cleanup or final actions
            });
        }

    }).fail(function (jqXHR, textStatus, errorThrown) {
        // Handle AJAX request failure
        console.error('Failed', textStatus, errorThrown);
        ShowMessageError(jqXHR.responseText || 'Unknown error => Validate barcode data.');
    });
}

async function HandleDuplicateItem(qrCodeMessage, errMsg) {
    stopScanner();
    Swal.fire({
        title: '<strong>' + errMsg +'</strong>',
        icon: 'warning',
        html: '<u><span style="color:red">กรุณาตรวจสอบข้อมูลก่อนยืนยัน!</span></u>',
        showCancelButton: true,
        confirmButtonColor: '#04B431',
        confirmButtonText: 'ยืนยัน',
        cancelButtonColor: '#D33',
        cancelButtonText: "ยกเลิก",
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger ml-1'
        },
        buttonsStyling: false,
        focusConfirm: true,
    }).then((result) => {


        if (result.isConfirmed) {
            // Add item
            AddItemDataList(qrCodeMessage).then(() => {
                // Resume scanning after adding
                startScanner();

            }).catch((error) => {
                // Handle any error from AddItemDataList
                console.error('Error adding item:', error);

                // Optional: Show an error message
                ShowMessageError(error);

                // Resume scanning even if there was an error
                startScanner();
            });
        }
        else {
            // Just resume if canceled
            startScanner();
        }
    });
}