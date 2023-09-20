

function EditBrach(form) {

    $("#global-loader").css('display', '');

    var frmEditBranch = $("#frmEditBranch");
    frmEditBranch.validate();
    var isValid = frmEditBranch.valid();

    if (isValid) {
        console.log('Call => EditBranch');
        $.validator.unobtrusive.parse(form);
        var formData = $(form).serializeJSON();
        console.log(formData);
        formData = JSON.stringify(formData);

        $.ajax({
            method: "POST",
            async: true,
            url: "/Branch/EditBracnh",
            data: formData,
            contentType: "application/json; charset=utf-8",
        }).done(function (response) {
            if (response.result) {
                
                //AlertSuccess('ปรับปรุงข้อมูลสำเร็จ');
                ShowMessageSuccess('ปรับปรุงข้อมูลสำเร็จ');

                //Reset form [0] gets the DOM element from the jQuery object
                $("#frmEditBranch")[0].reset();
                $("#global-loader").css('display', 'none');

            }
            else {
                //AlertError(response.message);
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