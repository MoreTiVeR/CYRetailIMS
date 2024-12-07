
var datatable;

$('.select2').select2();
InitialDatePicker();
InitialTimePicker();
InitialCharacterRemaining();
InitialItemRepeater();

$("#btnSave").on('click', function () {

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

            $("#frmUpdateMoneyTransfer").trigger("submit");
        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            //Condition
        }
    });
});

$("#btnCancel").on('click', function (e) {
    window.location.href = '/MoneyTransfer/Index';
});

$("#btnAdd").on('click', function () {
    var trows = parseInt($("#totalrow").val()) + parseInt(1);
    console.log('add total row: ' + trows);
    $("#totalrow").val(trows);
});

function InitialItemRepeater() {
    window.outerRepeater = $('.repeater-default').repeater({
        isFirstItemUndeletable: false,
        initEmpty: false,
        //defaultValues: { 'text-input': 'outer-default' },
        show: function () {
            var seen = {}; // Object to store encountered values
            var isDuplicate = false;

            //Add row
            $(this).slideDown();

            //Focus on txtTransferAmount
            $("input[ID='txtTransferAmount']").trigger("focus");
            //$("input[ID='txtTransferTime']").addClass("timepicker");
            //$("input[ID='txtTransferTime']").datepicker({
            //    dateFormat: 'dd/mm/yy'
            //});

            InitialTimePicker();

            //pickatime
            //$('.timepicker').pickatime({
            //    format: 'HH:i',
            //    interval: 150
            //});

            //bootstrap datetimepicker
            //$('.timepicker').datetimepicker({
            //    format: 'HH:mm',
            //    pickDate: false,
            //    pickSeconds: false,
            //    pick12HourFormat: false
            //});

            //$('.timepicker').datetimepicker({
            //    format: 'LT'
            //});

            //$("input[ID='txtTransferTime']").datetimepicker({
            //    pickDate: false,
            //    minuteStepping: 30,
            //    format: 'hh:mm',
            //    pickTime: true,
            //    language: 'en',
            //    use24hours: true
            //});
        },
        hide: function (deleteElement, e) {

            //Delete row
            $(this).slideUp(deleteElement);

            //Remove total row
            var trows = parseInt($("#totalrow").val()) - 1;
            $("#totalrow").val(trows);

            //Get ItemCode-Key from delete row from select2
            //var deletedCode = $(this).repeaterVal()["outer-item-group"][0].txtTransferAmount;
            //console.log('deletedCode -> ' + deletedCode);

            //Re-calculate price
            ReCalculateTotalAmountTransfer();
        }
    });
}

function CalculateTotalAmountTransfer(amt, name) {

    var res = name.split('[');
    var resIdx = res[1].split(']');
    console.log('CalculateTotalAmountTransfer -> value: ' + amt);
    console.log('CalculateTotalAmountTransfer -> input: ' + name)
    //var txtTransferAmount = $("input[name='" + name + "']").val();
    //var transferAmount = parseInt(txtTransferAmount);

    //var curRate = $("input[name='outer-item-group[" + resIdx[0] + "][txtCurRate]']").val();
    //var total = parseFloat(curRate) * qty;
    //$("input[name='outer-item-group[" + resIdx[0] + "][txtTransferAmount]']").val(total.toFixed(2));

    //Sum total amount
    var totalAmountTransfer = 0;
    var totalRow = parseInt($("#totalrow").val());
    console.log('read total row: ' + totalRow);

    for (var i = 0; i < totalRow; i++) {
        var txtAmt = $("input[name='outer-item-group[" + i + "][txtTransferAmount]']").val();
        console.log('read amount transfer -> ' + txtAmt)
        totalAmountTransfer += parseFloat(txtAmt);
        console.log('total amount transfer -> ' + totalAmountTransfer)
    }

    $("#AmountTransfer").val(currencyFormat(totalAmountTransfer));

}

function OnBegin(data) {
    console.log(data);
    ShowLoading();
}

function OnSuccess(data) {
    if (data.result) {
        $("#frmUpdateMoneyTransfer")[0].reset();
        ShowMessageSuccess(data.message);
        HideLoading();
    }
    else {
        ShowMessageError(data.message);
        HideLoading();
    }
}

function OnFailed(data) {
    console.log(data);
    ShowMessageError(data.message);
    HideLoading();
}

