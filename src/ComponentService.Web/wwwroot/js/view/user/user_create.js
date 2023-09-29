var dataTable;

$('.select2').select2();

function RegisterAccount(form) {

    $("#global-loader").css('display', '');
    var frmRegisterAccount = $("#frmRegisterAccount");
    frmRegisterAccount.validate();
    var isValid = frmRegisterAccount.valid();
    if (isValid) {
        console.log('Call => AddItem');
        $.validator.unobtrusive.parse(form);
        var data = $(form).serializeJSON();
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/UserManagement/CreateAccount',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.result) {
                    //popup.dialog('close');

                    AlertSuccess("เพิ่มบัญชีผู้ใช้งานสำเร็จ");
                    $("#frmRegisterAccount")[0].reset();
                    $("#global-loader").css('display', 'none');

                    //To do next?
                    //window.location = data.url;
                }
                else {
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