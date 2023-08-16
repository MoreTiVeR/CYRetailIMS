
$(document).ready(function (){
    
});

function Login(form) {

    $("#global-loader").css('display', '');

    var frmLogin = $("#frmLogin");
    frmLogin.validate();
    var isValid = frmLogin.valid();
    if (isValid) {
        console.log('Call => SubmitAuthen');
        $.validator.unobtrusive.parse(form);
        var data = $(form).serializeJSON();
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Account/Authen',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.result) {
                    //popup.dialog('close');
                    ShowMessageSuccess(data.message);

                    //To do next?
                    window.location = data.url;
                }
                else {
                    ShowMessageError(data.message);
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