
$(document).ready(function () {
    InitialDatePicker();
    InitialNumberInput();
    //InitialRepeater();
    InitialItemRepeater();
    $('.select2').select2();
});

$(document).on('change', '.select2', function (e) {
    // Get the selected value
    var selectedValue = $(this).val();
    // Get the data-row attribute to identify the row
    var row = $(this).data('row');
    console.log($(this).data('name'));
    // Log the selected value for the current row (you can replace this with your desired logic)
    console.log("Row " + row + ": " + selectedValue);
    ShowMessageInfo('Selected value :' + selectedValue);
});

$("#btnSave").on('click', function () {
    //var isValid = $("#frmCurrency").valid();
    var data = $($("#frmSelling")).serializeJSON();
    $.post("ItemDataValidation", { data }).then(
        function (results) {

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
            ShowMessageError('Unknow error => Create Sale data.');
            console.log('this will run if the deferred generates a progress update.');
        }
    );

});

$("#btnAdd").on('click', function () {
    var trows = parseInt($("#totalrow").val()) + parseInt(1);
    $("#totalrow").val(trows);

    //var $div = $('div[id^="another-participant"]:last');
    //console.log($div);

    //var num = parseInt($div.prop("id").match(/\d+/g), 10) + 1;
    //console.log(num);

    //var $klon = $div.clone().prop('id', 'another-participant' + num);
    //console.log($klon);

    //$klon.find('select').each(function () {
    //    var value = $(this).val();
    //    var text = $(this).find(':selected').text();
    //    var id = this.id;
    //    let name_number = this.name.match(/\d+/);
    //    name_number++;
    //    this.id = this.id + '_' + name_number;
    //    this.name = this.name.replace(/\[[0-9]\]+/, '[' + name_number + ']')
    //});
    //// Finally insert $klon after the last div
    //$div.after($klon);
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
    $('.repeater-default').repeater({
        initEmpty: false,
        show: function () {
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
                    //    alert('InitialRepeater -> ' + select_val);
                    //});
                    $('.ddl-searchitem-container').css('width', '100%');
                    $('.ddl-searchitem').next().next().remove();
                }
            });
        },
        hide: function (deleteElement) {

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

                //confirm
                if (result.value) {

                    $(this).slideUp(deleteElement);

                    var trows = parseInt($("#totalrow").val()) - 1;
                    $("#totalrow").val(trows);

                    //get the values of the inputs as a formatted object
                    var valr = $('.repeater').repeaterVal();
                    console.log(valr);
                    console.log('success get val');
                    var deletedCode = $(this).repeaterVal()["group-a"][0].ddlSearchItem;
                    alert(deletedCode);
                    //Total Price Calculator each row
                    var id;
                    var price;
                    var row = 1;
                    $(".group-a :input").each(function (e) {
                        id = this.id;

                        if (this.id == "ddlSearchItem") {
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
            //$('.select2').select2({
            //    placeholder: 'Select an option'
            //});
        },
        isFirstItemUndeletable: false
    });
}

function InitialItemRepeater() {
    window.outerRepeater = $('.repeater-default').repeater({
        isFirstItemUndeletable: false,
        initEmpty: false,
        //defaultValues: { 'text-input': 'outer-default' },
        show: function () {
            console.log('outer show');
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
                    //    alert('InitialRepeater -> ' + select_val);
                    //});
                    $('.ddl-searchitem-container').css('width', '100%');
                    $('.ddl-searchitem').next().next().remove();
                }
            });
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
            $("#txtSummaryTHB").val(currencyFormat(totalAmt));
        }
    });
}

function CalculatePriceByPrice(price, name) {
    var res = name.split('[');
    var resIdx = res[1].split(']');

    var qty = $("input[name='outer-item-group[" + resIdx[0] + "][txtItemQty]']").val() | 0;
    var total = parseFloat(price) * qty;

    $("input[name='outer-item-group[" + resIdx[0] + "][txtAmount]']").val(total.toFixed(2));

    //Sum total amount
    var totalRow = parseInt($("#totalrow").val());
    var totalAmt = 0;
    for (var i = 0; i < totalRow; i++) {
        var txtAmt = $("input[name='outer-item-group[" + i + "][txtAmount]']").val() | 0;
        totalAmt += parseFloat(txtAmt);
    }
    $("#txtSummaryTHB").val(currencyFormat(totalAmt));

}

function CalculatePriceByKey(itemkey, name) {
    var res = name.split('[');
    var resIdx = res[1].split(']');
    //alert('index -> ' + resIdx[0]);
    //var curCode = $("input[name='" + name + "']").val();
    //Set new Rate

    var ajaxRequest = $.ajax({
        url: 'GetItemPriceByID',
        async: true,
        type: 'POST',
        dataType: 'JSON',
        data: { "itemId": itemkey },
        success: function (response) {

            if (!response.result) {
                ShowMessageError(response.msg);
                return;
            }

            //Set item price
            $("input[name='outer-item-group[" + resIdx[0] + "][txtItemPrice]']").val(response.data);

            //Get & Re-check qty if is null
            var qty = $("input[name='outer-item-group[" + resIdx[0] + "][txtItemQty]']").val() | 0;
            if (isNaN(qty)) {
                qty = $("input[name='outer-item-group[" + resIdx[0] + "][txtItemQty]']").val();
            }

            //var curRate = $("input[name='outer-item-group[" + resIdx[0] + "][txtItemPrice]']").val();
            var total = parseFloat(response.data) * qty;
            $("input[name='outer-item-group[" + resIdx[0] + "][txtAmount]']").val(total.toFixed(2));

            //Sum total amount
            var totalAmt = 0;

            var totalRow = parseInt($("#totalrow").val());

            for (var i = 0; i < totalRow; i++) {
                var txtAmt = $("input[name='outer-item-group[" + i + "][txtAmount]']").val();
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

function CalculatePriceByQty(qty, name) {
    var res = name.split('[');
    var resIdx = res[1].split(']');

    var itemPrice = $("input[name='outer-item-group[" + resIdx[0] + "][txtItemPrice]']").val();
    var total = parseFloat(itemPrice) * qty;

    $("input[name='outer-item-group[" + resIdx[0] + "][txtAmount]']").val(total.toFixed(2));

    //Sum total amount
    var totalRow = parseInt($("#totalrow").val());
    var totalAmt = 0;
    for (var i = 0; i < totalRow; i++) {
        var txtAmt = $("input[name='outer-item-group[" + i + "][txtAmount]']").val() | 0;
        totalAmt += parseFloat(txtAmt);
    }
    $("#txtSummaryTHB").val(currencyFormat(totalAmt));
}

function OnSuccess(data) {
    $("#txtSummaryTHB").val(0);

    if (data.result) {
        ShowMessageSuccess(data.msg);
        /*InitialAddItemPartial();*/
        ResetForm();
    }
    else {
        ShowMessageError(data.msg);
    }
}

function ResetForm() {
    $('.outer-item-group').empty();
    $('#frmSelling')[0].reset(); // [0] gets the DOM element from the jQuery object
    //$(".ddl-searchitem").select2({
    //    allowClear: true
    //});
    /*$("#ddlSearchItem").on('change', function () { $(this).val("").select2(); });*/
    //$('.ddl-searchitem').select2({
    //    placeholder: "-- เลือกสินค้าขาย --",
    //    allowClear: true,
    //    width: '100%',
    //})
}