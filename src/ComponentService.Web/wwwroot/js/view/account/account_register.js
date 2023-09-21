
$('.select2').select2();

$(document).on('change', '.select2', function (e) {
    // Get the selected value
    var selectedValue = $(this).val();
    // Get the data-row attribute to identify the row
    var row = $(this).data('row');
    console.log($(this).data('name'));
    // Log the selected value for the current row (you can replace this with your desired logic)
    console.log("Row " + row + ": " + selectedValue);
    //ShowMessageInfo('Selected value :' + selectedValue);
});


function Register(form) {
    //$.validator.unobtrusive.parse(form);
    //var formData = $(form).serializeJSON();
    //formData = JSON.stringify(formData);

    $("#global-loader").css('display', '');

    var frmRegister = $("#frmRegister");
    frmRegister.validate();
    var isValid = frmRegister.valid();
    if (isValid) {
        //Data
        $.validator.unobtrusive.parse(form);
        var formData = $(form).serializeJSON();
        formData = JSON.stringify(formData);

        $.ajax({
            method: "POST",
            async: true,
            url: "/Account/Register",
            data: formData,
            contentType: "application/json; charset=utf-8",
        }).done(function (response) {
            if (response.result) {

                ShowMessageSuccess(response.message);
                $("#frmRegister")[0].reset();
                $("#global-loader").css('display', 'none');
            }
            else {
                ShowMessageError(response.message);
                $("#global-loader").css('display', 'none');
            }
        });
        return false;
    }
    else {
        $("#global-loader").css('display', 'none');
    }

}