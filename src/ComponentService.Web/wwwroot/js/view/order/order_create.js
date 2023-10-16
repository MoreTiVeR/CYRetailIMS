


function SavePurchaseOrder(form) {

    $("#global-loader").css('display', '');
    var frmSavePurchaseOrder = $("#frmSavePurchaseOrder");
    frmSavePurchaseOrder.validate();
    var isValid = frmSavePurchaseOrder.valid();
    if (isValid) {
        console.log('Call => PurchaseOrderItem');
        $.validator.unobtrusive.parse(form);
        var data = $(form).serializeJSON();
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Order/CreateAdjustItem',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.result) {
                    //popup.dialog('close');

                    AlertSuccess("สร้างรายการสั่งสินค้าสำเร็จ");
                    //$("#frmSavePurchaseOrder")[0].reset();
                    $("#global-loader").css('display', 'none');
                    dataTable.ajax.reload();
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