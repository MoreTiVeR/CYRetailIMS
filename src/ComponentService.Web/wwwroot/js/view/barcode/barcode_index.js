
var datepicker;
InitialBarcodeScanner()


function InitialBarcodeScanner() {
    document.getElementById('scanButton').addEventListener('click', async () => {
        const codeReader = new ZXing.BrowserMultiFormatReader();
        const videoInputDevices = await codeReader.listVideoInputDevices();
        const selectedDeviceId = videoInputDevices[0].deviceId;

        const preview = document.getElementById('preview');
        preview.style.display = 'block';
        //await codeReader.decodeFromVideoDevice(selectedDeviceId, 'preview', (result, err) => {
        //    if (result) {
        //        alert(`Barcode scanned: ${result.text}`);
        //        // Send barcode data to backend
        //        fetch('/Barcode/ScanBarcode', {
        //            method: 'POST',
        //            headers: {
        //                'Content-Type': 'application/json',
        //            },
        //            body: JSON.stringify({ barcode: result.text }),
        //        });
        //        preview.style.display = 'none';
        //        codeReader.reset();
        //    }
        //    if (err && !(err instanceof ZXing.NotFoundException)) {
        //        console.error(err);
        //    }
        //});
    });
}