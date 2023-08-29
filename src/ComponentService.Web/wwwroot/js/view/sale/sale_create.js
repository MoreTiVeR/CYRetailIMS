
$(document).ready(function () {
    InitialDatePicker();
    InitialNumberInput();

    //$('#ddlSaleItem').select2();
    $('#ddlSearchItem').select2();
    
});

$('#ddlSearchItem').on('change', function () {
    var value = $(this).val();
    var text = $(this).find(':selected').text();
    alert(value + ' | ' + text);
    // Set selected 
    $('#txtItem').val(value);

});

function AddItem(form) {

    $("#global-loader").css('display', '');

    var frmAddItem = $("#frmAddItem");
    frmAddItem.validate();
    var isValid = frmAddItem.valid();
    if (isValid) {
        console.log('Call => AddItem');
        $.validator.unobtrusive.parse(form);
        var data = $(form).serializeJSON();
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Item/AddItem',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.result) {
                    //popup.dialog('close');

                    AlertSuccess("เพิ่มสินค้าสำเร็จ");
                    $("#frmAddItem")[0].reset();
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