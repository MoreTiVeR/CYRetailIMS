

function EditItem(form) {

    $("#global-loader").css('display', '');

    var frmEditItem = $("#frmEditItem");
    frmEditItem.validate();
    var isValid = frmEditItem.valid();
    if (isValid) {
        console.log('Call => EditItem');
        $.validator.unobtrusive.parse(form);
        var data = $(form).serializeJSON();
        console.log(data);
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Item/EditItem',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.result) {
                    //popup.dialog('close');

                    console.log(data);
                    AlertSuccess('ปรับปรุงข้อมูลสำเร็จ');
                    $("#frmEditItem")[0].reset();
                    $("#global-loader").css('display', 'none');
                    //ShowMessageSuccess(data.message);

                    //To do next?
                    //window.location = data.url;
                }
                else {
                    //ShowMessageError(data.message);
                    AlertError(data.message);
                    $("#global-loader").css('display', 'none');
                }
            }
        });
        return false;
    }
    else {
        $("#global-loader").css('display', 'none');
    }
}