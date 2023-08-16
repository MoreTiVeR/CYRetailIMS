
$(document).ready(function (){
    
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

                    Swal.fire({
                        title: "สำเร็จ!",
                        text: data.message,
                        type: "success",
                        confirmButtonClass: "btn btn-primary",
                        buttonsStyling: !1,
                    });
                    $("#frmAddItem")[0].reset();
                    $("#global-loader").css('display', 'none');
                    //ShowMessageSuccess(data.message);

                    //To do next?
                    //window.location = data.url;
                }
                else {
                    //ShowMessageError(data.message);
                    Swal.fire({
                        title: "ทำรายการไม่สำเร็จ!",
                        text: data.message,
                        type: "success",
                        confirmButtonClass: "btn btn-dander",
                        buttonsStyling: !1,
                    });
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