
InitialCharacterRemaining();
function EditReceiptTemp(form) {

    ShowLoading();

    var frmEditReceiptTemplate = $("#frmEditReceiptTemplate");
    frmEditReceiptTemplate.validate();
    var isValid = frmEditReceiptTemplate.valid();
    if (isValid) {
        console.log('Call => EditReceiptTemp');
        $.validator.unobtrusive.parse(form);
        var data = $(form).serializeJSON();
        console.log(data);
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Receipt/EditReceiptTemplate',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.result) {

                    AlertSuccess('ปรับปรุงข้อมูลสำเร็จ');
                    $("#frmEditReceiptTemplate")[0].reset();
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