
$('.select2').select2();
InitialCharacterRemaining();
function CreateReceiptTemp(form) {

    ShowLoading();

    var frmCreateReceiptTemplate = $("#frmCreateReceiptTemplate");
    frmCreateReceiptTemplate.validate();
    var isValid = frmCreateReceiptTemplate.valid();
    if (isValid) {
        console.log('Call => CreateReceiptTemplate');
        $.validator.unobtrusive.parse(form);
        var data = $(form).serializeJSON();
        console.log(data);
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Receipt/CreateReceiptTemplate',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.result) {

                    AlertSuccess('เพิ่มข้อมูลสำเร็จ');
                    $("#frmCreateReceiptTemplate")[0].reset();
                    HideLoading();

                    //To do next?
                    //window.location = data.url;
                }
                else {
                    //ShowMessageError(data.message);
                    AlertError(data.message);
                    HideLoading();
                }
            }
        });
        return false;
    }
    else {
        HideLoading();
    }
}

function InitialCharacterRemaining() {
    $('textarea').charactersRemaining();
    $('textarea').charactersRemaining({
        singleCharacterText: '## จำนวนตัวอักษรที่พิมพ์ได้',
        multipleCharacterText: '## จำนวนตัวอักษรที่พิมพ์ได้'
    });
}