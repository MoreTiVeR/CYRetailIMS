
$('.select2').select2();

function EditItem(form) {

    $("#global-loader").css('display', '');

    var frmEditItem = $("#frmEditItem");
    frmEditItem.validate();
    var isValid = frmEditItem.valid();
    if (isValid) {
        console.log('Call => EditItem');
        $.validator.unobtrusive.parse(form);
        var formData = $(form).serializeJSON();

        // Check NotifyMaxQty is null value
        if (formData.NotifyMaxQty === null || formData.NotifyMaxQty === undefined || formData.NotifyMaxQty === '') {

            // Set default NotifyMaxQty is 0
            formData.NotifyMaxQty = 0;
        }

        if (formData.SubItemTypeID === null || formData.SubItemTypeID === undefined || formData.SubItemTypeID === '') {

            // Set default NotifyMaxQty is 0
            formData.SubItemTypeID = null;
        }


        var jsonData = JSON.stringify(formData);
        $.ajax({
            type: 'POST',
            url: '/Item/EditItem',
            data: jsonData,
            contentType: 'application/json',
            success: function (response) {
                if (response.result) {

                    console.log(response);
                    //AlertSuccess('ปรับปรุงข้อมูลสำเร็จ');
                    ShowMessageSuccess('ปรับปรุงข้อมูลสำเร็จ');

                    //$("#frmEditItem")[0].reset();
                    $("#global-loader").css('display', 'none');
                    //ShowMessageSuccess(response.message);

                    //To do next?
                    //window.location = data.url;
                }
                else {
                    //AlertError(response.message);
                    ShowMessageError(response.message);
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