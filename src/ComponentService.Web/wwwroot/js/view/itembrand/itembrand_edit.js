
InitialCharacterRemaining();

function EditBrand(form) {

    ShowLoading();
    var frmEdit = $("#frmEditBrand");
    frmEdit.validate();
    var isValid = frmEdit.valid();
    if (isValid) {
        console.log('Call => EditBrand');
        $.validator.unobtrusive.parse(form);
        var formData = $(form).serializeJSON();
        var jsonData = JSON.stringify(formData);
        console.log(jsonData);

        $.ajax({
            type: 'POST',
            url: '/ItemBrand/EditBrand',
            data: jsonData,
            contentType: 'application/json',
            success: function (response) {
                if (response.result) {
                    //popup.dialog('close');

                    console.log(response);
                    //AlertSuccess('ปรับปรุงข้อมูลสำเร็จ');
                    ShowMessageSuccess('ปรับปรุงข้อมูลสำเร็จ');

                    //$("#frmEditItem")[0].reset();
                    HideLoading();
                    //ShowMessageSuccess(response.message);

                    //To do next?
                    //window.location = data.url;
                }
                else {
                    //AlertError(response.message);
                    ShowMessageError(response.message);
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

$("#btnCancel").on('click', function (e) {
    window.location.href = '/ItemBrand/Index';
});