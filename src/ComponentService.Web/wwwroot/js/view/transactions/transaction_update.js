var datepicker;

InitialDatePickerWithoutSetCurrentData();
InitialNumberInput();
$('.select2').select2();


$("#btnUpdateTrasaction").on('click', function () {
    Swal.fire({
        title: "ยืนยันการแก้ไขข้อมูล?",
        html: "<span class='text-success'>ระบบจะทำการปรับปรุงข้อมูล <span class='text-danger'><u>วันที่ขาย</u></span> เท่านั้น</span>",
        icon: 'warning',
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "ยืนยัน",
        confirmButtonClass: "btn btn-primary",
        cancelButtonText: "ยกเลิก",
        cancelButtonClass: "btn btn-primary ml-1",
        buttonsStyling: false,
    }).then(function (t) {
        if (t.value) {
            
            $("#global-loader").css('display', '');
            var frmEditTransaction = $("#frmEditTransaction");
            frmEditTransaction.validate();
            var isValid = frmEditTransaction.valid();

            if (isValid) {
                console.log('Call => UpdateTransaction');
                $.validator.unobtrusive.parse(frmEditTransaction);
                var data = $(frmEditTransaction).serializeJSON();
                console.log(data);
                data = JSON.stringify(data);
                $.ajax({
                    type: 'POST',
                    url: '/Transactions/UpdateTransaction',
                    data: data,
                    contentType: 'application/json',
                    success: function (data) {
                        if (data.result) {
                            //popup.dialog('close');

                            console.log(data);
                            AlertSuccess('ปรับปรุงข้อมูลสำเร็จ');
                            /*$("#frmEditItem")[0].reset();*/
                            $("#global-loader").css('display', 'none');
                            //ShowMessageSuccess(data.message);

                            //To do next?
                            //window.location = data.url;
                        }
                        else {
                            //ShowMessageError(data.message);
                            AlertError(data.message);
                            $("#global-loader").css('display', 'none');
                        }
                    }
                });
                return false;
            }
            else {
                ShowMessageWarning('ข้อมูลไม่ถูกต้อง กรุณาตรวจสอบใหม่อีกครั้ง!');
                $("#global-loader").css('display', 'none');
            }
        }
    });
});

function editTransactionV2(form) {
    $("#global-loader").css('display', '');

    var frmEditTransaction = $("#frmEditTransaction");
    frmEditTransaction.validate();
    var isValid = frmEditTransaction.valid();

    if (isValid) {
        console.log('Call => UpdateTransaction');
        $.validator.unobtrusive.parse(form);
        var data = $(form).serializeJSON();
        console.log(data);
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Transactions/UpdateTransaction',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.result) {
                    //popup.dialog('close');

                    console.log(data);
                    AlertSuccess('ปรับปรุงข้อมูลสำเร็จ');
                    /*$("#frmEditItem")[0].reset();*/
                    $("#global-loader").css('display', 'none');
                    //ShowMessageSuccess(data.message);

                    //To do next?
                    //window.location = data.url;
                }
                else {
                    //ShowMessageError(data.message);
                    AlertError(data.message);
                    $("#global-loader").css('display', 'none');
                }
            }
        });
        return false;
    }
    else {
        ShowMessageWarning('ข้อมูลไม่ถูกต้อง กรุณาตรวจสอบใหม่อีกครั้ง!');
        $("#global-loader").css('display', 'none');
    }
}