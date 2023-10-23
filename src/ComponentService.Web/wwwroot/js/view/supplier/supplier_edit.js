
function EditSupplier(form) {

    $("#global-loader").css('display', '');
    var frmEditSupplier = $("#frmEditSupplier");
    frmEditSupplier.validate();
    var isValid = frmEditSupplier.valid();
    if (isValid) {
        console.log('Call => PurchaseOrderItem');
        $.validator.unobtrusive.parse(form);
        var data = $(form).serializeJSON();
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/SupplierManagement/UpdateSupplier',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.result) {
                    //popup.dialog('close');

                    AlertSuccess("ปรับปรุงซัฟพลายเออร์สำเร็จ");
                    //$("#frmEditPurchaseOrder")[0].reset();
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