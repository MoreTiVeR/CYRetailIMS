
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
class QrScannerWrapper {
    constructor(elementId, config) {
        this.qrCodeScanner = new Html5Qrcode(elementId);
        this.config = config || {
            fps: 15,
            qrbox: { width: 300, height: 300 },
            aspectRatio: 1.7777778,
            disableFlip: true
        };

        this.cameraId = null;
        this.isScanning = false;
        this.isPaused = false;
        this.resultCallback = null;
        this.errorCallback = null;
    }

    async init(cameraLabelContains = "back") {
        const cameras = await Html5Qrcode.getCameras();
        if (!cameras || cameras.length === 0) {
            throw new Error("ขออภัย, ไม่พบกล้อง.");
        }
        //const backCam = cameras.find(c => c.label.toLowerCase().includes(cameraLabelContains)) || cameras[0];
        const backCam = cameras.find(c =>
            c.label.toLowerCase().includes('back') ||
            c.label.toLowerCase().includes('rear') ||
            c.label.toLowerCase().includes('environment')
        ) || cameras[cameras.length - 1]; // Fallback to last camera
        this.cameraId = backCam.id;
        document.getElementById("result").innerText = "Initial successful.";
    }

    async start(onScanSuccess, onScanFailure) {
        document.getElementById("result").innerText = "Initialization...";
        if (!this.cameraId) {
            await this.init(); // get camera if not already
        }

        this.resultCallback = onScanSuccess;
        this.errorCallback = onScanFailure;

        await this.qrCodeScanner.start(
            { deviceId: { exact: this.cameraId } },
            this.config,
            decodedText => {
                if (!this.isPaused && typeof this.resultCallback === "function") {
                    this.resultCallback(decodedText);
                }
            },
            errorMessage => {
                if (typeof this.errorCallback === "function") {
                    this.errorCallback(errorMessage);
                }
            }
        );

        this.isScanning = true;
        this.isPaused = false;
        document.getElementById("result").innerText = "";
    }

    async stop() {
        if (this.isScanning) {
            await this.qrCodeScanner.stop();
            await this.qrCodeScanner.clear();
            this.isScanning = false;
            this.isPaused = false;
        }
    }

    pause() {
        if (this.isScanning) {
            this.isPaused = true;
        }
    }

    resume() {
        if (this.isScanning) {
            this.isPaused = false;
        }
    }

    isCameraRunning() {
        return this.isScanning;
    }
}

let html5QrCode;
let isCameraStarted = false;
let lastCameraId = null;
const scannerModal = document.getElementById('mdlMobileScannerV2');
const scanner = new QrScannerWrapper("reader");
let lastResult = null; // Moved to global scope for accessibility
let countResults = 0;  // Moved to global scope for accessibility

scannerModal.addEventListener('shown.bs.modal', async () => {
    setTimeout(async () => {
        try {
            document.getElementById("result").innerText = "";
            await scanner.start(
                (qrCodeMessage) => handleScanResultV2(qrCodeMessage),
                (error) => handleScanError(error)
            );

            // Only hide loader after camera actually starts
            document.getElementById("scanner-loading").style.display = "none";
        } catch (err) {
            console.log(`ไม่สามารถเข้าถึงกล้อง: ${err.message}`);
            document.getElementById("result").innerText = `ไม่สามารถเข้าถึงกล้อง: ${err.message}`;
        }
    }, 300); // Delay 300ms to allow modal rendering to complete
});

scannerModal.addEventListener('hidden.bs.modal', async () => {
    await scanner.stop();
    document.getElementById("result").innerText = ""; // Clear result
});

function handleScanResultV2(qrCodeMessage) {
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
            //showConfirmationDialog(qrCodeMessage, res.message);
            handleDuplicateItem(qrCodeMessage, res.message);
        }
        else
        {
            //รายการใหม่ ยังไม่ซ้ำ ยังไม่ได้สแกน
            //if (qrCodeMessage !== lastResult) {
            //    countResults++;
            //    lastResult = qrCodeMessage;

            //    console.log(`Scan result ${qrCodeMessage}`);
            //    await AddItemDataList(qrCodeMessage);
            //}
            // Use .then() to handle the promise returned by AddItemDataList

            scanner.pause(); // Pause immediately after a scan
            AddItemDataList(qrCodeMessage).then(() => {
                // Handle success if needed
                
            }).catch((error) => {
                // Handle error from AddItemDataList if needed
                //scanner.resume(); // Pause immediately after a scan
                ShowMessageError(error);
            }).finally(() => {
                console.log("Scan completed."); // Cleanup or final actions

                setTimeout(function () {
                    scanner.resume(); // Pause immediately after a scan
                }, 1000)
            });
        }

    }).fail(function (jqXHR, textStatus, errorThrown) {
        // Handle AJAX request failure
        console.error('Failed', textStatus, errorThrown);
        ShowMessageError(jqXHR.responseText || 'Unknown error => Validate barcode data.');
    });
}

// Handle scanning errors
function handleScanError(error) {
    console.error("Scan error:", error);
    // Optional: handle scan failures
}

// Handle duplicate items
async function handleDuplicateItem(qrCodeMessage, errMsg) {
    scanner.pause(); // Pause immediately after a scan

    Swal.fire({
        title: `<strong>${errMsg}</strong>`,
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
            AddItemDataList(qrCodeMessage).then(() =>
            {
                // Resume scanning after adding
                scanner.resume();

            }).catch((error) => {
                // Handle any error from AddItemDataList
                console.error('Error adding item:', error);

                // Optional: Show an error message
                ShowMessageError(error);

                // Resume scanning even if there was an error
                scanner.resume();
            });
        }
        else
        {
            // Just resume if canceled
            scanner.resume();
        }
    });
}

async function showConfirmationDialog(qrCodeMessage, errMsg) {

    const result = await Swal.fire({
        title: `<strong>${errMsg}</strong>`,
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
        focusConfirm: true
    });

    if (result.isConfirmed) {
        console.log(`Scan result ${qrCodeMessage}`);
        await AddItemDataList(qrCodeMessage);
    } else if (result.dismiss === Swal.DismissReason.cancel) {
        //ShowMessageInfo('ยกเลิก');
    }
}

function startScanner(cameraId = null) {

    document.getElementById("scanner-loading").style.display = "block";

    //let config = {
    //    fps: 10,
    //    qrbox: { width: 250, height: 250 },
    //    aspectRatio: 1.7777778,
    //    disableFlip: false
    //};
    let config = {
        fps: 15,
        qrbox: { width: 300, height: 300 },
        aspectRatio: 1.7778,
        disableFlip: true
    };

    if (!html5QrCode) {
        html5QrCode = new Html5Qrcode("reader");
    }
    
    const startWithCamera = (camera) => {
        lastCameraId = camera.id;

        const highResConstraints = {
            deviceId: { exact: camera.id },
            width: { min: 640, ideal: 1280 },
            height: { min: 480, ideal: 1280 }
        };

        html5QrCode.start(
            highResConstraints,
            config,
            qrCodeMessage => {
               
                if (qrCodeMessage !== lastResult) {
                    ++countResults;
                    lastResult = qrCodeMessage;

                    // Handle on success condition with the decoded message.
                    console.log(`Scan result ${qrCodeMessage}`);

                    document.getElementById("result").innerText = `Scanned: ${qrCodeMessage}`;
                    AddItemDataList(qrCodeMessage);
                }
                else {
                    stopScanner();
                    Swal.fire({
                        title: `<strong>ต้องการเพิ่มจำนวนสินค้า ${lastResult} รายการเดิมหรือไม่?</strong>`,
                        icon: 'warning',
                        html: '<u><span style="color:red">กรุณาตรวจสอบข้อมูลก่อนยืนยัน!</span></u>',
                        showCancelButton: true,
                        //showDenyButton: true,
                        confirmButtonColor: '#04B431',
                        confirmButtonText: 'ยืนยัน',
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

                            //Add item
                            AddItemDataList(qrCodeMessage);

                            startWithCamera({ id: cameraId });
                        }
                        else if (result.dismiss === Swal.DismissReason.cancel) {
                            //Do nothing
                            //html5QrcodeScanner.resume();
                        }
                    });
                }
                
            },
            errorMessage => {
                // Ignore scan errors
            }
        ).then(() => {
            document.getElementById("scanner-loading").style.display = "none";
            isCameraStarted = true;
        }).catch(err => {
            console.error("ขออภัย, ไม่สามารถเข้าถึงกล้อง:", err);
            document.getElementById("scanner-loading").style.display = "none";
            if (err.name === "NotAllowedError") {
                ShowMessageError("กรุณาอนุญาตให้เว็บไซต์เข้าถึงกล้องของคุณ.");
            }
            else {
                ShowMessageError(`ขออภัย, ไม่สามารถเข้าถึงกล้อง: ${err}`);
            }
        });
    };

    if (cameraId) {
        // If we already know which camera to use
        startWithCamera({ id: cameraId });
    }
    else
    {
       // If no cameras found or label access is restricted
        Html5Qrcode.getCameras().then(cameras =>
        {
            if (cameras && cameras.length)
            {
                //let backCamera = cameras.find(c => c.label.toLowerCase().includes('back')) || cameras[0];
                //startWithCamera(backCamera);

                // Prefer camera with label containing 'back' or facingMode workaround
                let backCamera = cameras.find(c =>
                    c.label.toLowerCase().includes('back') ||
                    c.label.toLowerCase().includes('rear') ||
                    c.label.toLowerCase().includes('environment')
                ) || cameras[cameras.length - 1]; // Fallback to last camera

                // Try to use back camera by deviceId
                startWithCamera({ id: backCamera.id });
            }
            else {
                // If no cameras found or label access is restricted
                html5QrCode.start(
                    { facingMode: "environment" },
                    config,
                    qrCodeMessage => {

                        if (qrCodeMessage !== lastResult) {
                            ++countResults;
                            lastResult = qrCodeMessage;

                            // Handle on success condition with the decoded message.
                            console.log(`Scan result ${qrCodeMessage}`);

                            document.getElementById("result").innerText = `Scanned: ${qrCodeMessage}`;
                            AddItemDataList(qrCodeMessage);
                        }
                        else {

                            stopScanner();
                            Swal.fire({
                                title: `<strong>ต้องการเพิ่มจำนวนสินค้า ${lastResult} รายการเดิมหรือไม่?</strong>`,
                                icon: 'warning',
                                html: '<u><span style="color:red">กรุณาตรวจสอบข้อมูลก่อนยืนยัน!</span></u>',
                                showCancelButton: true,
                                //showDenyButton: true,
                                confirmButtonColor: '#04B431',
                                confirmButtonText: 'ยืนยัน',
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

                                    //Add item
                                    AddItemDataList(qrCodeMessage);

                                    startWithCamera({ id: cameraId });
                                }
                                else if (result.dismiss === Swal.DismissReason.cancel) {
                                    //Do nothing
                                    //html5QrcodeScanner.resume();
                                }
                            });
                        }
                    },
                    error => {
                        // Ignore scan errors
                        console.error("Scan error:", err);
                        ShowMessageError(`Scan error: ${err}`);
                    }
                ).then(() => {
                    isCameraStarted = true;
                }).catch(err => {

                    console.error("ขออภัย, ไม่สามารถเข้าถึงกล้อง:", err);
                    document.getElementById("scanner-loading").style.display = "none";
                    if (err.name === "NotAllowedError") {
                        ShowMessageError("กรุณาอนุญาตให้เว็บไซต์เข้าถึงกล้องของคุณ.");
                    }
                    else {
                        ShowMessageError(`ขออภัย, ไม่สามารถเข้าถึงกล้อง: ${err}`);
                    }
                });
            }
        }).catch(err =>
        {
            console.error("พบข้อผิดพลาดในการเข้าถึงกล้อง", err);
            document.getElementById("scanner-loading").style.display = "none";
            ShowMessageError(`พบข้อผิดพลาดในการเข้าถึงกล้อง: ${err}`);
        });
    }
}

function stopScanner() {
    if (html5QrCode && isCameraStarted) {
        html5QrCode.stop().then(() => {
            html5QrCode.clear();
            isCameraStarted = false;
        }).catch(err => {
            console.error("เกิดข้อผิดพลาดการปิดใช้งานกล้อง:", err);
            ShowMessageError(`เกิดข้อผิดพลาดการปิดใช้งานกล้อง: ${err}`);
        });
    }
}

function pauseScanner() {
    if (html5QrCode && isCameraStarted) {
        html5QrCode.pas().then(() => {
            html5QrCode.clear();
            isCameraStarted = false;
        }).catch(err => {
            console.error("เกิดข้อผิดพลาดการหยุดใช้งานกล้อง:", err);
            ShowMessageError(`เกิดข้อผิดพลาดการหยุดใช้งานกล้อง: ${err}`);
        });
    }
}

async function requestCameraPermissionSilently() {
    try {
        const stream = await navigator.mediaDevices.getUserMedia({ video: { facingMode: "environment" } });
        stream.getTracks().forEach(track => track.stop());
        console.log("อนุญาตให้เว็บไซต์เข้าถึงกล้องของคุณสำเร็จ.");
        ShowMessageSuccess("อนุญาตให้เว็บไซต์เข้าถึงกล้องของคุณสำเร็จ.");

    } catch (err) {
        console.warn("ปฎิเสธการเข้าถึงกล้องหรือเว็บไซตืเข้าถึงกล้องไม่สำเร็จ!.");
        ShowMessageWarning("ปฎิเสธการเข้าถึงกล้องหรือเว็บไซตืเข้าถึงกล้องไม่สำเร็จ!.");
    }
}

document.getElementById("btnMobileScanV2").addEventListener("click", async () => {
    await requestCameraPermissionSilently();
});

