
$('#btnUpload').on('click', function () {
    ShowMessageSuccess('กำลังอัพนำเข้าไฟล์สินค้า...');
    // Checking whether FormData is available in browser  
    if (window.FormData !== undefined) {

        var fileUpload = $("#fileUpload").get(0);
        var files = fileUpload.files;

        // Create FormData object  
        var fileData = new FormData();

        // Looping over all files and add it to FormData object  
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }

        // Adding one more key to FormData object  
        fileData.append('username', 'Manas');

        $.ajax({
            url: '/Item/UploadFiles',
            type: "POST",
            contentType: false, // Not to set any content header  
            processData: false, // Not to process data  
            data: fileData,
            success: function (response) {
                if (response.result) {
                    ShowMessageSuccess(response.message);
                }
                else {
                    ShowMessageError(response.message);
                }
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    } else {
        ShowMessageError('ขออภัย มีบางอย่างไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!');
    }
});  