
$("#btnSave").on("click", function () {
    var frmAddBrand = $("#frmAddBrand");
    frmAddBrand.validate();
    var isValid = frmAddBrand.valid();
    if (isValid) {
        $.validator.unobtrusive.parse(frmAddBrand);
        var formData = $(frmAddBrand).serializeJSON();
        formData = JSON.stringify(formData);

        $.ajax({
            method: "POST",
            async: true,
            url: "/ItemBrand/CreateBrand",
            data: formData,
            contentType: "application/json; charset=utf-8",
        }).done(function (response) {
            if (response.result) {
                ShowMessageSuccess(response.message);

                //Reset form
                $('#frmAddBrand')[0].reset(); // [0] gets the DOM element from the jQuery object

            }
            else {
                ShowMessageError(response.message);
            }
            $("#global-loader").css('display', 'none');
        });
    }
});