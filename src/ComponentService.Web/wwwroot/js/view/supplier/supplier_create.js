
function SaveSupplier(form) {

    $("#global-loader").css('display', '');
    var frmSaveSupplier = $("#frmSaveSupplier");
    frmSaveSupplier.validate();
    var isValid = frmSaveSupplier.valid();
    if (isValid) {
        console.log('Call => PurchaseOrderItem');
        $.validator.unobtrusive.parse(form);
        var data = $(form).serializeJSON();
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/SupplierManagement/CreateSupplier',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.result) {
                    //popup.dialog('close');

                    AlertSuccess("เพิ่มข้อมูลซัฟพลายเออร์สำเร็จ");
                    //$("#frmSavePurchaseOrder")[0].reset();
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
