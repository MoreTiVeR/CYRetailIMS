
$('.select2').select2();

$('#aDownloadTemplateFile').on('click', function (e) {
    ShowMessageInfo('กำลังดาวน์โหลดไฟล์เทมเพลต...');
    e.preventDefault();  //stop the browser from following
    window.location.href = '../excel_template/Import_Item_temp.xlsx';
});

$('#btnUpload').on('click', function () {

    ShowMessageSuccess('กำลังนำเข้าไฟล์สินค้า...');
    ShowLoading();
    // Checking whether FormData is available in browser  
    if (window.FormData !== undefined) {

        var selectedBranchId = $("#branchid").val();
        var branchid = parseInt(selectedBranchId);

        var fileUpload = $("#fileUpload").get(0);
        var files = fileUpload.files;

        // Create FormData object  
        var fileData = new FormData();

        // Looping over all files and add it to FormData object  
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }

        // Adding one more key to FormData object  
        fileData.append('branchid', branchid);

        $.ajax({
            url: '/Item/UploadFiles',
            type: "POST",
            contentType: false, // Not to set any content header  
            processData: false, // Not to process data  
            data: fileData,
            success: function (response) {
                if (response.result) {                    
                    AlertSuccess(response.message);
                }
                else {
                    ShowMessageError(response.message);
                }
                HideLoading();
            },
            error: function (err) {
                ShowMessageError(err.statusText);
                HideLoading();
            }
        });
    } else {
        ShowMessageError('ขออภัย มีบางอย่างไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!');
        HideLoading();
    }
});  