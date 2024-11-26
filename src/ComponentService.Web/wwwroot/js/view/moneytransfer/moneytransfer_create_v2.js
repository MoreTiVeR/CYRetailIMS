
var datatable;

$('.select2').select2();
InitialDatePicker();
InitialCharacterRemaining();
InitialItemRepeater();


$("#btnSave").on('click', function () {
    var isValid = $('#frmMoneyTransfer').valid();
    if (!isValid) {
        ShowMessageError('กรุณาตรวจสอบข้อมูลก่อนบันทึกข้อมูล!');
    }
    else {
        var data = $($("#frmMoneyTransfer")).serializeJSON();
        $.post("MoneyTransferDataValidation", { data }).then(
            function (results) {

                if (results.result) {
                    console.log(results.msg);
                    Swal.fire({
                        //title: 'ยืนยันการบันทึกข้อมูล?',
                        //text: 'กรุณาตรวจสอบข้อมูลก่อนทำการบันทึก!',
                        //type: 'warning',
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
                            $("#frmTransferItem").submit();
                        }
                        else if (result.dismiss === Swal.DismissReason.cancel) {
                            //Code
                            ShowMessageInfo('ยกเลิก');
                        }
                    });
                }
                else {
                    ShowMessageError(results.msg);
                    return;
                }

            }, function (results) {
                //Failed
                console.log('Failed');
                ShowMessageError(results.message);

            }, function () {
                ShowMessageError('Unknow error => Create Sale data.');
                console.log('this will run if the deferred generates a progress update.');
            }
        );
    }
});

$("#btnCancel").on('click', function (e) {
    window.location.href = '/MoneyTransfer/Index';
});

function InitialItemRepeater() {
    window.outerRepeater = $('.repeater-default').repeater({
        isFirstItemUndeletable: false,
        initEmpty: true,
        //defaultValues: { 'text-input': 'outer-default' },
        show: function () {
            console.log('outer show');

            var seen = {}; // Object to store encountered values
            var isDuplicate = false;
            $(".outer-item-group :input").each(function (e) {
                if (this.type == 'select-one') {
                    if (this.value != '') {
                        seen[this.value];
                        if (seen[this.value]) {
                            // Duplicate found
                            isDuplicate = true;
                        }
                        else {
                            seen[this.value] = true;
                        }
                    }
                }
            });
            if (!isDuplicate) {
                $(this).slideDown();
                $(this).find('select').each(function () {
                    if (typeof $(this).attr('id') === "undefined") {
                        // ...
                    } else {
                        $('.ddl-searchitem').removeAttr("id").removeAttr("data-select2-id"); //some times id was not unique So select2 not working, so i remove id
                        $('.ddl-searchitem').select2();
                        //$('.ddl-searchitem').on('change', function (event) {
                        //    var selected_element = $(event.currentTarget);
                        //    var select_val = selected_element.val();
                        //    alert('InitialItemRepeater -> ' + select_val);
                        //});
                        $('.ddl-searchitem-container').css('width', '100%');
                        $('.ddl-searchitem').next().next().remove();
                    }
                });
            }

            ////PREVIUOS
            //$(this).slideDown();
            //$(this).find('select').each(function () {
            //    if (typeof $(this).attr('id') === "undefined") {
            //        // ...
            //    } else {
            //        $('.ddl-searchitem').removeAttr("id").removeAttr("data-select2-id"); //some times id was not unique So select2 not working, so i remove id
            //        $('.ddl-searchitem').select2();
            //        //$('.ddl-searchitem').on('change', function (event) {
            //        //    var selected_element = $(event.currentTarget);
            //        //    var select_val = selected_element.val();
            //        //    alert('InitialItemRepeater -> ' + select_val);
            //        //});
            //        $('.ddl-searchitem-container').css('width', '100%');
            //        $('.ddl-searchitem').next().next().remove();
            //    }
            //});
        },
        hide: function (deleteElement) {

            //Remove total row
            var trows = parseInt($("#totalrow").val()) - 1;
            $("#totalrow").val(trows);

            //Delete row
            $(this).slideUp(deleteElement);
            console.log('row deleted');

            //Get ItemCode-Key from delete row from select2
            var deletedCode = $(this).repeaterVal()["outer-item-group"][0].ddlSearchItem;

            //Re-calculate price
            var price;
            $(".outer-item-group :input").each(function (e) {
                if (this.type == 'select-one') {
                    if (this.value == deletedCode) {
                        console.log(this.name);
                        var res = this.name.split('[');
                        var resIdx = res[1].split(']');
                        var txtAmt = $("input[name='outer-item-group[" + resIdx[0] + "][txtAmount]']").val();
                        price = txtAmt;
                        console.log('Set delete => price : ' + price);
                    }
                    else {
                        //Code here
                    }
                }
            });

            var totalAmt = 0;
            var idx = 0;
            $(".outer-item-group :input").each(function (e) {
                if (this.id == "txtAmount") {
                    console.log('row : ' + idx);
                    var resAmt = $("input[name='outer-item-group[" + idx + "][txtAmount]']").val();
                    if (new Number(resAmt) == price) {
                        //Do nothing
                    }
                    else {
                        totalAmt += new Number(resAmt);
                    }
                    idx += 1;
                }
            });

            console.log('Re-calculate price:' + totalAmt);
            //$("#txtSummaryTHB").val(currencyFormat(totalAmt));
        }
    });
}

function OnBegin(data) {
    console.log(data);
    ShowLoading();
}

function OnSuccess(data) {
    if (data.result) {
        $("#frmCreateMoneyTransfer")[0].reset();
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

