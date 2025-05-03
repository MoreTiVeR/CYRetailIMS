

$("#btnSave").on("click", function () {
    alert('Create Branch');
    var frmAddBrand = $("#frmAddBranch");
    frmAddBrand.validate();
    var isValid = frmAddBrand.valid();
    if (isValid) {
        $.validator.unobtrusive.parse(frmAddBrand);
        var formData = $(frmAddBrand).serializeJSON();
        formData = JSON.stringify(formData);

        $.ajax({
            method: "POST",
            async: true,
            url: "/Branch/CreateBranch",
            data: formData,
            contentType: "application/json; charset=utf-8",
        }).done(function (response) {
            if (response.result) {
                ShowMessageSuccess(response.message);

                //Reset form
                $('#frmAddBranch')[0].reset(); // [0] gets the DOM element from the jQuery object

            }
            else {
                ShowMessageError(response.message);
            }
            $("#global-loader").css('display', 'none');
        });
    }
});