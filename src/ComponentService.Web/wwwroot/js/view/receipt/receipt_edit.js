
InitialCharacterRemaining();
function EditReceiptTemp(form) {

    ShowLoading();

    var frmEditReceiptTemplate = $("#frmEditReceiptTemplate");
    frmEditReceiptTemplate.validate();
    var isValid = frmEditReceiptTemplate.valid();
    if (isValid) {
        console.log('Call => EditReceiptTemp');
        $.validator.unobtrusive.parse(form);
        var data = $(form).serializeJSON();
        console.log(data);
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Receipt/EditReceiptTemplate',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.result) {

                    AlertSuccess('ปรับปรุงข้อมูลสำเร็จ');
                    $("#frmEditReceiptTemplate")[0].reset();
                    HideLoading();

                    //To do next?
                    //window.location = data.url;
                }
                else {
                    //ShowMessageError(data.message);
                    AlertError(data.message);
                    HideLoading();
                }
            }
        });
        return false;
    }
    else {
        HideLoading();
    }
}

function InitialCharacterRemaining() {
    $('textarea').charactersRemaining();
    $('textarea').charactersRemaining({
        singleCharacterText: '## จำนวนตัวอักษรที่พิมพ์ได้',
        multipleCharacterText: '## จำนวนตัวอักษรที่พิมพ์ได้'
    });
}


// Preview Receipt Function
function previewReceipt() {
    var form = $("#frmEditReceiptTemplate");
    var model = form.serializeJSON();

    $.ajax({
        url: '/Receipt/GenerateReceiptText',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(model),
        success: function (response) {
            if (response.result) {
                // Put text into the <pre> so spacing/newlines are preserved
                $("#receiptPreview").text(response.text);

                // Show modal
                $("#receiptPreviewModal").modal("show");

                // Reset scroll
                $("#receiptPreview").closest('.modal-body').scrollTop(0);
            } else {
                Swal.fire({ title: 'Error!', text: response.message, icon: 'error' });
            }
        },
        error: function () {
            Swal.fire({ title: 'Error!', text: 'Failed to generate receipt preview', icon: 'error' });
        }
    });
}