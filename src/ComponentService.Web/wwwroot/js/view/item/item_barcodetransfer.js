var dataTable;

$('.select2').select2();
//InitialDatePicker();
InitialNumberInput();
InitialCharacterRemaining();
InitialTableItemTransfer();


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

$("#btnSave").on('click', function () {
    var isValid = $('#frmTransferItem').valid();
    if (!isValid) {
        ShowMessageError('กรุณาตรวจสอบข้อมูลก่อนบันทึกข้อมูล!');
    }
    else {
        var data = $($("#frmTransferItem")).serializeJSON();
        $.post("ItemTransferDataValidation", { data }).then(
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

$("#transfertypeid").on("change", function () {
    var text = $('option:selected', $(this)).text();
    var transfertypeid = parseInt($('option:selected', $(this)).val());
    var request = $.ajax({
        url: '/Item/FillSourceDestinationBranch',
        async: true,
        type: 'POST',
        dataType: 'JSON',
        data: { "transferTypeID": transfertypeid },
        success: function (response) {

            if (response.result) {
                //สาขาต้นทาง
                var selectList_source_branchid = $('#source_branchid');
                selectList_source_branchid.html(""); // clear before appending new list
                selectList_source_branchid.append($('<option></option>').val("").html("--เลือกสาขาต้นทาง--")); //Add first itemList
                $.each(response.data_source, function () {
                    $("<option />").val(this.value).text(this.text).appendTo(selectList_source_branchid);
                    //prevGroupName = this.group.name;
                });

                //สาขาปลายทาง
                var selectList_destination_branchid = $('#destination_branchid');
                selectList_destination_branchid.html(""); // clear before appending new list
                selectList_destination_branchid.append($('<option></option>').val("").html("--เลือกสาขาปลายทาง--")); //Add first itemList
                $.each(response.data_destination, function () {
                    $("<option />").val(this.value).text(this.text).appendTo(selectList_destination_branchid);
                    //prevGroupName = this.group.name;
                });

                //ItemList
                //var selectList_itembranchtransfer = $('#itembranchtransfer');
                //selectList_itembranchtransfer.html(""); // clear before appending new list
                //selectList_itembranchtransfer.append($('<option></option>').val("").html("--เลือกสินค้าที่ต้องการโอน--")); //Add first itemList
                //$.each(response.data_itemlist, function () {
                //    alert(this.text);
                //    $("<option />").val(this.value).text(this.text).appendTo(selectList_itembranchtransfer);
                //    //prevGroupName = this.group.name;
                //});

                $('.item-transfer-repeater').find('select').html("");
                $("<option />").val("").text("--เลือกสินค้าที่ต้องการโอน--").appendTo($('.item-transfer-repeater').find('select'));
                $('.item-transfer-repeater').find('select').each(function (e) {
                    if (this.type == 'select-one') {
                        if (this.value == '') {
                            $.each(response.data_itemlist, function () {
                                $("<option />").val(this.value).text(this.text).appendTo($('.item-transfer-repeater').find('select'));
                                //prevGroupName = this.group.name;
                            });
                        }
                    }
                });
            }

        },
        failure: function (response) {
            ShowMessageError(response);
        },
        error: function (response) {
            ShowMessageWarning(response);
        }
    });

});

$("#source_branchid").on("change", function () {
    var text = $('option:selected', $(this)).text();
    var sbranchid = parseInt($('option:selected', $(this)).val());
    var transferTypeid = parseInt($("#transfertypeid").val());
    //ShowMessageWarning('source_branchid - change' + 'val -> ' + sbranchid + 'text -> ' + text);
    var request = $.ajax({
        url: '/Item/FillItemTransferByBranchID',
        async: true,
        type: 'POST',
        dataType: 'JSON',
        data: { "transferTypeID": transferTypeid, "branchID": sbranchid },
        success: function (response) {

            if (response.result) {

                //Fill destination branchid selection สาขาปลายทาง
                var selectList_destination_branchid = $('#destination_branchid');
                selectList_destination_branchid.html(""); // clear before appending new list
                selectList_destination_branchid.append($('<option></option>').val("").html("--เลือกสาขาปลายทาง--")); //Add first itemList
                $.each(response.data_destination, function () {
                    $("<option />").val(this.value).text(this.text).appendTo(selectList_destination_branchid);
                    //prevGroupName = this.group.name;
                });

                //Fill item transfer selection in repeater
                $('.item-transfer-repeater').find('select').html("");
                $("<option />").val("").text("--เลือกสินค้าที่ต้องการโอน--").appendTo($('.item-transfer-repeater').find('select'));
                $('.item-transfer-repeater').find('select').each(function (e) {
                    if (this.type == 'select-one') {
                        if (this.value == '') {
                            $.each(response.data_itemlist, function () {
                                $("<option />").val(this.value).text(this.text).appendTo($('.item-transfer-repeater').find('select'));
                                //prevGroupName = this.group.name;
                            });
                        }
                    }
                });
            }
            else {
                ShowMessageError(response.msg);
            }

        },
        failure: function (response) {
            ShowMessageError(response);
        },
        error: function (response) {
            ShowMessageWarning(response);
        }
    });
});

$("#ddlSearchItem").on("change", function () {
    ShowMessageWarning('ddlSearchItem - change');
});

$("#itembranchtransfer").on("change", function () {
    ShowMessageWarning('itembranchtransfer - change');
});

function InitialTableItemTransfer() {
    dataTable = $('#tbItems').DataTable({
        destroy: true,
        "searching": false,
        "paging": false,
        "ordering": false,
        "info": false,
        "ajax": {
            "url": "/Item/GetTempItemTransfer",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "nseq" },
            { "data": "sitemname" },
            { "data": "nqty" },
            { "data": "price" },
            { "data": "totalprice" },
            {
                "data": "nseq",
                "render": function (data) {
                    console.log('nseq=' + data);
                    return "<a class='me-3' style='margin-left:5px' onclick=Delete(" + data + ")><img src='../assets/img/icons/delete.svg' alt='img'></a>";
                }
            }
        ],
        "language": {
            "emptyTable": "ไม่พบข้อมูล."
        },
        "order": [[0, "desc"]],
        "columnDefs": [
            {
                "targets": [0],
                "visible": true
            }
        ]
    });
}

function AddTransferItem(form) {
    console.log('Call => SubmitAddTransferItem');
    console.log(form);
    $.validator.unobtrusive.parse(form);
    var data = $(form).serializeJSON();
    data = JSON.stringify(data);
    console.log(data);

    var frmAddOrderItem = $("#frmAddTransferItem");
    frmAddOrderItem.validate();
    var isValid = frmAddOrderItem.valid();
    if (!isValid) {
        ShowMessageError('กรุณาระบุข้อมูลให้ถูกต้องก่อนทำรายการ');
        return;
    }
    $.ajax({
        type: 'POST',
        url: '/Item/AddTempItemTransfer',
        data: data,
        contentType: 'application/json',
        success: function (data) {
            if (data.result) {
                //popup.dialog('close');
                ShowMessageSuccess(data.message);
                dataTable.ajax.reload();
                //$('#frmAddOrderItem')[0].reset();

                //$('#mdlAddItem').modal('toggle');
                //$('#mdlAddItem').modal('hide');
                //$("#btnCloseMdl").click();

                $("#sbarcode").val('');
            }
            else {
                AlertError(data.message);
            }
        }
    });
    return false;
}

function Delete(id) {
    //alert(id);
    console.log('Call => Delete => ' + id);
    Swal.fire({
        title: "ยืนยันการลบข้อมูล?",
        //text: "เมื่อลบข้อมูลแล้ว จะไม่สามารถทำการยกเลิกได้!",
        html: "<span class='text-danger'>เมื่อลบข้อมูลแล้ว จะไม่สามารถทำการยกเลิกได้!</span>",
        icon: 'warning',
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "ยืนยัน",
        confirmButtonClass: "btn btn-primary",
        cancelButtonText: "ยกเลิก",
        cancelButtonClass: "btn btn-danger ml-1",
        buttonsStyling: false,
    }).then(function (t) {
        if (t.value) {

            //Delete
            $.ajax({
                type: 'POST',
                url: '/Item/DeleteTempItemTransfer',
                dataType: 'JSON',
                data: { "seq": id },
                success: function (response) {
                    if (response.result) {

                        ShowMessageSuccess('ลบข้อมูลสำเร็จ');
                        $("#global-loader").css('display', 'none');

                        dataTable.ajax.reload();

                        //Set sum amount
                        //$("#amount").val(response.amount);
                    }
                    else {
                        //ShowMessageError(data.message);
                        ShowMessageError(response.message);
                        $("#global-loader").css('display', 'none');
                    }
                }
            });
        }
    });
    
}

function CalculatePriceByKey(itemkey, name) {
    var res = name.split('[');
    var resIdx = res[1].split(']');

    var seen = {}; // Object to store encountered values
    var isDuplicate = false;
    $('.item-transfer-repeater').find('select').each(function (e) {
        if (this.type == 'select-one') {
            if (this.value != '') {
                seen[this.value];
                if (seen[this.value]) {
                    // Duplicate found
                    isDuplicate = true;
                    return;
                }
                else {
                    seen[this.value] = true;
                }
            }
        }
    });
    if (isDuplicate) {
        //ShowMessageError('ขออภัย, ไม่สามารถระบุสินค้าชนิดเดียวกันได้!');
        $("select[name='outer-item-group[" + resIdx[0] + "][ddlSearchItem]']").val('').trigger('change.select2');
        //$("select[name='outer-item-group[" + resIdx[0] + "][ddlSearchItem]']").val('');
        $("input[name='outer-item-group[" + resIdx[0] + "][txtCurrentQty]']").val('');
        $("input[name='outer-item-group[" + resIdx[0] + "][txtTransferQty]']").val('');
        return;
    }
    else {
        var transferTypeid = parseInt($("#transfertypeid").val());
        var sbranchid = parseInt($("#source_branchid").val());
        var ajaxRequest = $.ajax({
            url: 'GetItemByID',
            async: true,
            type: 'POST',
            dataType: 'JSON',
            data: { "itemId": itemkey, "transfertypeID": transferTypeid, "sourceBranchID": sbranchid },
            success: function (response) {
                if (!response.result) {
                    $("input[name='outer-item-group[" + resIdx[0] + "][txtItemPrice]']").val('');
                    ShowMessageError(response.msg);
                    return;
                }

                //Set item price
                $("input[name='outer-item-group[" + resIdx[0] + "][txtItemPrice]']").val(response.data.price);

                //Set current item qty
                $("input[name='outer-item-group[" + resIdx[0] + "][txtCurrentQty]']").val(response.data.qty);

                //Get & Re-check qty if is null
                var qty = $("input[name='outer-item-group[" + resIdx[0] + "][txtTransferQty]']").val() | 0;
                if (isNaN(qty)) {
                    qty = $("input[name='outer-item-group[" + resIdx[0] + "][txtTransferQty]']").val();
                }

                //var curRate = $("input[name='outer-item-group[" + resIdx[0] + "][txtItemPrice]']").val();
                var total = parseFloat(response.data.price) * qty;
                //$("input[name='outer-item-group[" + resIdx[0] + "][txtAmount]']").val(total.toFixed(2));

                //Sum total amount
                var totalAmt = 0;

                var totalRow = parseInt($("#totalrow").val());

                //for (var i = 0; i < totalRow; i++) {
                //    var txtAmt = $("input[name='outer-item-group[" + i + "][txtAmount]']").val();
                //    totalAmt += parseFloat(txtAmt);
                //}

                //$("#txtSummaryTHB").val(currencyFormat(totalAmt));
            },
            failure: function (response) {
                ShowMessageError(response.msg);
            },
            error: function (response) {
                ShowMessageError(response.msg);
            }
        });
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

function ValidateTransferQty(transferqty, name) {
    var res = name.split('[');
    var resIdx = res[1].split(']');

    if (parseInt(transferqty) < 0) {
        ShowMessageError('ระบุจำนวนโอนไม่ถูกต้อง! กรุณาลองใหม่อีกครั้ง');
        $("input[name='outer-item-group[" + resIdx[0] + "][txtTransferQty]']").val(0);
        return;
    }
    //Get current qty
    var currentQry = $("input[name='outer-item-group[" + resIdx[0] + "][txtCurrentQty]']").val() | 0;
    if (isNaN(currentQry)) {
        currentQry = $("input[name='outer-item-group[" + resIdx[0] + "][txtCurrentQty]']").val();
    }
    if (parseInt(currentQry) < 0) {
        ShowMessageError('จำนวนสินค้าคงเหลือไม่เพียงพอในการทำรายการ! กรุณาติดต่อผู้ดูแลระบบ');
        $("input[name='outer-item-group[" + resIdx[0] + "][txtTransferQty]']").val(0);
        return;
    }

    //Validate is Available Transfer
    var availablelQty = parseInt(currentQry) - parseInt(transferqty);
    if (parseInt(availablelQty) < 0) {
        ShowMessageError('จำนวนสินค้าในสต๊อกไม่เพียงพอ! กรุณาระบุจำนวนใหม่อีกครั้ง');
        $("input[name='outer-item-group[" + resIdx[0] + "][txtTransferQty]']").val(0);
        return;
    }
}

function OnSuccess(data) {
    if (data.result) {
        ShowMessageSuccess(data.msg);
        AlertSuccess(data.msg);
        ResetForm();
    }
    else {
        ShowMessageError(data.msg);
    }
}

function ResetForm() {
    //Reset Repeater
    $('.outer-item-group').empty();

    //Reset form
    $('#frmTransferItem')[0].reset(); // [0] gets the DOM element from the jQuery object

    //Reset select2
    $("#transfertypeid").val('').trigger('change.select2');

    $("#source_branchid").empty();
    var source_option = new Option("--เลือกสาขาต้นทาง--", "", true, true);
    $("#source_branchid").append(source_option).trigger('change');

    $("#destination_branchid").empty();
    var destination_option = new Option("--เลือกสาขาปลายทาง--", "", true, true);
    $("#destination_branchid").append(destination_option).trigger('change');
}