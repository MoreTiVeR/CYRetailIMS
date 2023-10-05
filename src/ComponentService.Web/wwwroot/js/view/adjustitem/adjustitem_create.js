var dataTable;


function SaveAdjustItem(form) {

    $("#global-loader").css('display', '');
    var frmAdjustItem = $("#frmAdjustItem");
    frmAdjustItem.validate();
    var isValid = frmAdjustItem.valid();
    if (isValid) {
        console.log('Call => AddAdjustItem');
        $.validator.unobtrusive.parse(form);
        var data = $(form).serializeJSON();
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/AdjustItem/CreateAdjustItem',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.result) {
                    //popup.dialog('close');

                    AlertSuccess("ปรับสต๊อกสินค้าสำเร็จ");
                    $("#frmAdjustItem")[0].reset();
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