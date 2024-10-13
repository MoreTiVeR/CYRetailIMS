
InitialCharacterRemaining();
$('.select2').select2();

$(document).on('change', '.select2', function (e) {
    // Get the selected value
    var selectedValue = $(this).val();
    // Get the data-row attribute to identify the row
    var row = $(this).data('row');
    console.log($(this).data('name'));
    // Log the selected value for the current row (you can replace this with your desired logic)
    console.log("Row " + row + ": " + selectedValue);
    //ShowMessageInfo('Selected value :' + selectedValue);
});

function SaveItemTransfer(form) {

    $("#global-loader").css('display', '');

    var frmTransferItem = $("#frmTransferItem");
    frmTransferItem.validate();
    var isValid = frmTransferItem.valid();
    if (isValid) {
        console.log('Call => EditItem');
        $.validator.unobtrusive.parse(form);
        var data = $(form).serializeJSON();
        console.log(data);
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Item/TransferItem',
            data: data,
            contentType: 'application/json',
            success: function (response) {
                if (response.result) {
                    //popup.dialog('close');

                    console.log(response);
                    //AlertSuccess('ปรับปรุงข้อมูลสำเร็จ');
                    ShowMessageSuccess('รับโอนสินค้าสำเร็จ');

                    /*$("#frmTransferItem")[0].reset();*/
                    $("#global-loader").css('display', 'none');
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


function OnSuccess(data) {
    //$("#txtSummaryTHB").val(0);

    if (data.result) {
        ShowMessageSuccess(data.message);
        AlertSuccess(data.message);
    }
    else {
        ShowMessageError(data.message);
    }
}

function InitialCharacterRemaining() {
    $('textarea').charactersRemaining();
    $('textarea').charactersRemaining({
        singleCharacterText: '## จำนวนตัวอักษรที่พิมพ์ได้',
        multipleCharacterText: '## จำนวนตัวอักษรที่พิมพ์ได้'
    });
}