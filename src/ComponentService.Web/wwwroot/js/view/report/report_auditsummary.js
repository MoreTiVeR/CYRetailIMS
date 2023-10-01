
InitialNumberInput();
InitialDatePicker();
InitialCharacterRemaining();
$('.select2').select2();

function SaveAutiSaleSummaryReport(form) {

    $("#global-loader").css('display', '');

    var frmAutiSaleSummaryReport = $("#frmAutiSaleSummaryReport");
    frmAutiSaleSummaryReport.validate();
    var isValid = frmAutiSaleSummaryReport.valid();
    if (isValid) {
        console.log('Call => EditItem');
        $.validator.unobtrusive.parse(form);
        var data = $(form).serializeJSON();
        console.log(data);
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Report/SaveAuditSaleSummaryReport',
            data: data,
            contentType: 'application/json',
            success: function (response) {
                if (response.result) {
                    //popup.dialog('close');

                    console.log(response);
                    //AlertSuccess('ปรับปรุงข้อมูลสำเร็จ');
                    ShowMessageSuccess('ปรับปรุงข้อมูลสำเร็จ');

                    //$("#frmAutiSaleSummaryReport")[0].reset();
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