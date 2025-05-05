
var datepicker;
//InitialHtmlQRCode();

InitialBarcodeScanner(function () {
    let config = {
        fps: 10,
        qrbox: 250,
        rememberLastUsedCamera: false,
        supportedScanTypes: [Html5QrcodeScanType.SCAN_TYPE_CAMERA]
    };

    var resultContainer = document.getElementById('qr-reader-results');
    var lastResult, countResults = 0;

    function onScanSuccess(decodedText, decodedResult) {
        if (decodedText !== lastResult) {
            ++countResults;
            lastResult = decodedText;
            // Handle on success condition with the decoded message.
            console.log(`Scan result ${decodedText}`, decodedResult);
            ShowMessageSuccess(`Scan result ${decodedText}`, decodedResult);

            // If you want to stop scanning after first successful scan:
            //html5QrcodeScanner.clear()
            //    .then(_ => console.log("Stopped scanning."), ShowMessageError(`Stopped scanning.`))
            //    .catch(err => console.error("Failed to clear scanner.", err), ShowMessageError(`Failed to clear scanner`));
        }
        else {
            ShowMessageError(`decodedText same lastResult ${decodedText}`, decodedResult);
        }
    }

    function onScanError(errorMessage) {
        // handle on error condition, with error message
        // typically gets called for decode failures or camera issues
        console.warn(`QR code scan error = ${errorMessage}`);
        ShowMessageError(`QR code scan error = ${errorMessage}`);

        // you can optionally show errors to the user:
        document.getElementById('qr-reader-results').innerHTML =
            `<div style="color:red">
         <strong>Scan error:</strong> ${errorMessage}
       </div>`;
    }

    var html5QrcodeScanner = new Html5QrcodeScanner("qr-reader", config, false);
    html5QrcodeScanner.render(onScanSuccess);
});

function InitialBarcodeScanner(fn) {
    // see if DOM is already available
    if (document.readyState === "complete"
        || document.readyState === "interactive") {
        // call on next available tick
        setTimeout(fn, 1);
    } else {
        document.addEventListener("DOMContentLoaded", fn);
    }
}


//function InitialHtmlQRCode() {
//    const html5QrCode = new Html5QrcodeScanner("qr-reader");
//    const qrCodeSuccessCallback = (decodedText, decodedResult) => {
//        /* handle success */
//        console.log(`Scan result: ${decodedText}`, decodedResult);
//        document.getElementById('kode').value = decodedText;
//        // ...
//        html5QrcodeScanner.clear();
//    };
//    const config = { fps: 10, qrbox: 250 };
//    // Select front camera or fail with `OverconstrainedError`.
//    html5QrCode.start({ facingMode: { exact: "environment"} }, config, qrCodeSuccessCallback);
//    //html5QrCode.start({ facingMode: { exact: "user" } }, config, qrCodeSuccessCallback);
//}