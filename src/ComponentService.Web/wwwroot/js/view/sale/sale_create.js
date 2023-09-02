
$(document).ready(function () {
    InitialDatePicker();
    InitialNumberInput();
    InitialAddItemPartial();

    $('.ddl-ddlBranch').select2();
    $('.ddl-seatchitem').select2();

});

$('.select2').on('change', function () {
    var value = $(this).val();
    var text = $(this).find(':selected').text();
    alert(value + ' | ' + text);
    // Set selected 
    $('#txtItem').val(value);

});

$("#btnAdd").on('click', function () {
    var trows = parseInt($("#totalrow").val()) + parseInt(1);
    $("#totalrow").val(trows);
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

function InitialAddItemPartial() {
    $.ajax({
        url: 'GetSellingItemPartialPage',
        type: "POST",
        xhrFields: {
            withCredentials: true
        }
    }).done(function (results) {

        $("#divSellingItem").html(results);
        InitialRepeater();
        $('.ddl-seatchitem').select2();
        //$('.select2').select2();
        

    }).fail(function (results) {
        console.log('Invalid Data.');
    });
}

function ValidationEnglishKeyPress() {
    $("input[ID='txtItemCode']").on("keypress", function (event) {

        // Disallow anything not matching the regex pattern (A to Z uppercase, a to z lowercase and white space)
        // For more on JavaScript Regular Expressions, look here: https://developer.mozilla.org/en-US/docs/JavaScript/Guide/Regular_Expressions
        var englishAlphabetAndWhiteSpace = /[A-Za-z0-9]/g;

        // Retrieving the key from the char code passed in event.which
        // For more info on even.which, look here: http://stackoverflow.com/q/3050984/114029
        var key = String.fromCharCode(event.which);

        //alert(event.keyCode);

        // For the keyCodes, look here: http://stackoverflow.com/a/3781360/114029
        // keyCode == 8  is backspace
        // keyCode == 37 is left arrow
        // keyCode == 39 is right arrow
        // englishAlphabetAndWhiteSpace.test(key) does the matching, that is, test the key just typed against the regex pattern
        if (event.keyCode == 8 || event.keyCode == 37 || event.keyCode == 39 || englishAlphabetAndWhiteSpace.test(key)) {
            return true;
        }

        // If we got this far, just return false because a disallowed key was typed.
        return false;
    });
    $("input[ID='txtItemCode']").on("paste", function (e) {
        e.preventDefault();
    });
}

function InitialRepeater() {
    initEmpty: false,
    $('.file-repeater, .contact-repeater, .repeater-default').repeater({
        show: function () {
            $(this).slideDown();

            //form.find('select').next('.select2-container').remove();
            //form.find('select').select2();

            //$(".search-box select").select2();
            //$(this).find('.select2').removeClass('select2-hidden-accessible');
            //$(this).find('.select2-container').remove();
            //$(this).find('.select2').select2();
            //alert('slidedow');

            //$(".group-a :input").each(function (e) {
            //    //console.log(this);
            //    //console.log(e);
            //    if (this.id == "ddlSearchItem") {
            //        console.log('id: ' + this.id + ' | name: ' + this.name);
            //        console.log("input[name='group-a[" + e + "][" + this.id + "]']");
            //        //$("#group-a[1][ddlSearchItem]").select2();
            //        $("input[name='group-a[" + e + "][" + this.id + "]']").select2();
            //        $("#ddlSearchItem").select2();
            //    }
            //});

            $('.select2-container').remove();
            $('select').select2({
                //width: '100%',
                //placeholder: "เลือกสินค้าขาย",
                //allowClear: true
            });
        },
        hide: function (deleteElement, e) {

            Swal.fire({
                //title: 'ยืนยันการบันทึกข้อมูล?',
                //text: 'กรุณาตรวจสอบข้อมูลก่อนทำการบันทึก!',
                //type: 'warning',
                title: '<strong>ยืนยันการลบข้อมูล?</strong>',
                icon: 'warning',
                html: '<u><span style="color:red">กรุณาตรวจสอบข้อมูลก่อนทำการลบ!</span></u>'
                    + '<br><br>คุณต้องการลบรายการนี้หรือไม่?',
                showCancelButton: true,
                //showDenyButton: true,
                confirmButtonColor: '#04B431',
                confirmButtonText: 'ตกลง',
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

                //if (result.isConfirmed) {
                //    $.post("SetIsPrint", { isPrint: true }).then(
                //        function (results) {
                //            if (results.result) {
                //                $("#frmCurrency").submit();
                //            }
                //            else {
                //                Swal.fire(results.msg);
                //            }
                //        });
                //} else if (result.isDenied) {
                //    $.post("SetIsPrint", { isPrint: false }).then(
                //        function (results) {
                //            if (results.result) {
                //                $("#frmCurrency").submit();
                //            }
                //            else {
                //                Swal.fire(results.msg);
                //            }
                //        });
                //}

                //confirm
                if (result.value) {

                    $(this).slideUp(deleteElement);

                    var trows = parseInt($("#totalrow").val()) - 1;
                    $("#totalrow").val(trows);

                    var deletedCode = $(this).repeaterVal()["group-a"][0].txtItemCode;

                    //Total Price Calculator each row
                    var id;
                    var price;
                    var row = 1;
                    $(".group-a :input").each(function (e) {
                        id = this.id;

                        if (this.id == "txtItemCode") {
                            if (this.value == deletedCode) {
                                console.log(this.name);
                                var res = this.name.split('[');
                                var resIdx = res[1].split(']');
                                var txtAmt = $("input[name='group-a[" + resIdx[0] + "][txtAmount]']").val();
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
                    $(".group-a :input").each(function (e) {
                        if (this.id == "txtAmount") {
                            console.log('row : ' + idx);
                            var resAmt = $("input[name='group-a[" + idx + "][txtAmount]']").val();
                            if (new Number(resAmt) == price) {
                                //Do nothing
                            }
                            else {
                                totalAmt += new Number(resAmt);
                            }
                            idx += 1;
                        }
                    });

                    console.log(totalAmt);
                    //$("#txtSummaryTHB").val(currencyFormat(totalAmt));
                    $("#frmSelling").submit();
                }
                else if (result.dismiss === Swal.DismissReason.cancel) {
                    //
                }
            });
        },
        ready: function (setIndexes) {
            //
        },
        isFirstItemUndeletable: false
    });
}

function CalculateRateByCode(qty, name) {
    var res = name.split('[');
    var resIdx = res[1].split(']');

    var curCode = $("input[name='" + name + "']").val();

    //Set currency code to upper case
    $("input[name='" + name + "']").val(curCode.toUpperCase());

    var ajaxRequest = $.ajax({
        url: 'GetItemPriceByCode',
        async: true,
        type: 'POST',
        dataType: 'JSON',
        data: { "curCode": curCode },
        success: function (response) {

            if (!response.result) {
                ShowMessageError(response.msg);
                return;
            }

            //Set new Rate
            $("input[name='group-a[" + resIdx[0] + "][txtItemPrice]']").val(response.data);

            //Re-check qty if is null
            if (isNaN(qty)) {
                qty = $("input[name='group-a[" + resIdx[0] + "][txtItemQty]']").val();
            }

            var curRate = $("input[name='group-a[" + resIdx[0] + "][txtItemPrice]']").val();
            var total = parseFloat(curRate) * qty;
            $("input[name='group-a[" + resIdx[0] + "][txtAmount]']").val(total.toFixed(2));

            //Sum total amount
            var totalAmt = 0;

            var totalRow = parseInt($("#totalrow").val());

            for (var i = 0; i < totalRow; i++) {
                var txtAmt = $("input[name='group-a[" + i + "][txtAmount]']").val();
                totalAmt += parseFloat(txtAmt);
            }

            $("#txtSummaryTHB").val(currencyFormat(totalAmt));
        },
        failure: function (response) {
            ShowMessageError(response.msg);
        },
        error: function (response) {
            ShowMessageError(response.msg);
        }
    });

}

$("#btnSave").on('click', function () {
    //var isValid = $("#frmCurrency").valid();
    var data = $($("#frmSelling")).serializeJSON();
    $.post("ItemDataValidation", { data }).then(
        function (results) {

            //ShowMessageSuccess(results.msg + '|' + results.result);

            if (results.result) {

                console.log(results.msg);

                Swal.fire({
                    //title: 'ยืนยันการบันทึกข้อมูล?',
                    //text: 'กรุณาตรวจสอบข้อมูลก่อนทำการบันทึก!',
                    //type: 'warning',
                    title: '<strong>ยืนยันการบันทึกข้อมูล?</strong>',
                    icon: 'warning',
                    html: '<u><span style="color:red">กรุณาตรวจสอบข้อมูลก่อนทำการบันทึก!</span></u>'
                        + '<br>' + results.msg + '',
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
                        $("#frmSelling").submit();
                    }
                    else if (result.dismiss === Swal.DismissReason.cancel) {
                        //Code
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
            ShowMessageError('Unknow error => UpdateOrderDetailStatus.');
            console.log('this will run if the deferred generates a progress update.');
        }
    );

});