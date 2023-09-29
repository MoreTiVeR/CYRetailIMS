var dataTable;

$('.select2').select2();

function EditAccount(form) {

    $("#global-loader").css('display', '');
    var frmEditAccount = $("#frmEditAccount");
    frmEditAccount.validate();
    var isValid = frmEditAccount.valid();
    if (isValid) {
        console.log('Call => AddItem');
        $.validator.unobtrusive.parse(form);
        var data = $(form).serializeJSON();
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/UserManagement/SaveEditAccount',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.result) {
                    //popup.dialog('close');

                    AlertSuccess("อัพเดทข้อมูลบัญชีผู้ใช้งานสำเร็จ");
                    //$("#frmRegisterAccount")[0].reset();
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