
var datatable;

InitialDatePicker();
$('.select2').select2();

$("#btnCreate").on('click', function () {

    Swal.fire({
        title: '<strong>ยืนยันการบันทึกข้อมูล?</strong>',
        icon: 'warning',
        html: '<u><span style="color:red">กรุณาตรวจสอบข้อมูลก่อนทำการบันทึก!</span></u>',
        showCancelButton: true,
        //showDenyButton: true,
        confirmButtonColor: '#04B431',
        confirmButtonText: 'บันทึก',
        cancelButtonColor: '#D33',
        cancelButtonText: "ยกเลิก",
        //denyButtonText: 'ยืนยัน-ไม่ออกใบเสร็จ',
        //denyButtonColor: '#D33',
        customClass: {
            confirmButton: 'btn btn-success',
            denyButton: 'btn btn-warning ml-1',
            cancelButton: 'btn btn-danger ml-1'
        },
        buttonsStyling: false,
        focusConfirm: true
    }).then(function (result) {
        if (result.value) {

            $("#frmCreateMoneyTransfer").submit();
        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            //Condition
        }
    });
});

$("#btnCancel").on('click', function (e) {
    window.location.href = '/MoneyTransfer/Index';
});

function OnBegin(data) {
    ShowMessageWarning('OnBegin!!');
    console.log(data);
    ShowLoading();
}

function OnSuccess(data) {
    ShowMessageWarning('OnSuccess!!');
    if (data.result) {
        $("#frmCreateMoneyTransfer")[0].reset();
        ShowMessageSuccess(data.message);
    }
    else {
        ShowMessageError(data.message);
        HideLoading();
    }
}

function OnFailed(data) {
    ShowMessageWarning('OnFailed!!');
    console.log(data);
    ShowMessageError(data.message);
    HideLoading();
}

