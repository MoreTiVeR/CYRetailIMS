
function Login(form) {
    $.validator.unobtrusive.parse(form);
    var formData = $(form).serializeJSON();
    formData = JSON.stringify(formData);
    $("#global-loader").css('display', '');
    $.ajax({
        method: "POST",
        async: true,
        url: "/Account/Authen",
        data: formData,
        contentType: "application/json; charset=utf-8",
    }).done(function (response) {
        if (response.result) {
            ShowMessageSuccess(response.message);

            //Redirect to home
            window.location = response.url;
        }
        else {
            ShowMessageError(response.message);
        }
        $("#global-loader").css('display', 'none');
    });
    return false;
}