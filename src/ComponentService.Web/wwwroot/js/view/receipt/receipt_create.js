$('.select2').select2();
InitialCharacterRemaining();
function CreateReceiptTemp(form) {

    ShowLoading();

    var frmCreateReceiptTemplate = $("#frmCreateReceiptTemplate");
    frmCreateReceiptTemplate.validate();
    var isValid = frmCreateReceiptTemplate.valid();
    if (isValid) {
        console.log('Call => CreateReceiptTemplate');
        $.validator.unobtrusive.parse(form);
        var data = $(form).serializeJSON();
        console.log(data);
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Receipt/CreateReceiptTemplate',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.result) {

                    AlertSuccess('เพิ่มข้อมูลสำเร็จ');
                    $("#frmCreateReceiptTemplate")[0].reset();
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

//function PreviewReceiptTemp() {
//    var form = $("#frmCreateReceiptTemplate");
//    //frmCreateReceiptTemplate.validate();
//    //var isValid = frmCreateReceiptTemplate.valid();

//    $.validator.unobtrusive.parse(form);
//    var data = $(form).serializeJSON();
//    console.log(data);

//    data = JSON.stringify(data);
//    $.ajax({
//        type: 'POST',
//        url: '/Receipt/GenerateReceiptText',
//        data: data,
//        contentType: 'application/json',
//        success: function (data) {
//            if (data.result) {
//                //result true
//            }
//            else {
//                // result false
//            }
//        }
//    });
//}

//async function loadReceiptPreview(model) {
//    let res = await fetch('/Receipt/GenerateReceiveSlipText', {
//        method: 'POST',
//        headers: { 'Content-Type': 'application/json' },
//        body: JSON.stringify(model)
//    });
//    let result = await res.json();
//    document.getElementById("receiptPreview").textContent = result.text;
//    return result;
//}

function InitialCharacterRemaining() {
    $('textarea').charactersRemaining();
    $('textarea').charactersRemaining({
        singleCharacterText: '## จำนวนตัวอักษรที่พิมพ์ได้',
        multipleCharacterText: '## จำนวนตัวอักษรที่พิมพ์ได้'
    });
}

// Preview Receipt Function
function previewReceipt() {
    var form = $("#frmCreateReceiptTemplate");
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
