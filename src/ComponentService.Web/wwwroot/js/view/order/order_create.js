var dataTable;

$('.select2').select2();

InitialModalSelect2();
InitialTableOrder();

function InitialTableOrder() {
    dataTable = $('#tbItems').DataTable({
        destroy: true,
        "searching": false,
        "paging": false,
        "ordering": false,
        "info": false,
        "ajax": {
            "url": "/Order/GetTempItem",
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

function InitialModalSelect2() {
    $("#nitemid").select2({
        dropdownParent: $("#mdlPurchaseOrder")
    });
}

// This will be triggered everytime a user types anything
// in the input field with id as input-field
$("#trackingno").keyup(function (e) {
    // a-z => allow all lowercase alphabets
    // A-Z => allow all uppercase alphabets
    // 0-9 => allow all numbers
    // @ => allow @ symbol
    var regex = /^[a-zA-Z0-9@]+$/;
    // This is will test the value against the regex
    // Will return True if regex satisfied
    if (regex.test(this.value) !== true)
        //alert if not true
        //alert("Invalid Input");

        // You can replace the invalid characters by:
        this.value = this.value.replace(/[^a-zA-Z0-9@]+/, '');
});

function SavePurchaseOrder(form) {

    $("#global-loader").css('display', '');
    var frmSavePurchaseOrder = $("#frmSavePurchaseOrder");
    frmSavePurchaseOrder.validate();
    var isValid = frmSavePurchaseOrder.valid();
    if (isValid) {
        console.log('Call => PurchaseOrderItem');
        $.validator.unobtrusive.parse(form);
        var data = $(form).serializeJSON();
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Order/CreatePurchaseOrderItem',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.result) {
                    //popup.dialog('close');

                    AlertSuccess("สร้างรายการสั่งสินค้าสำเร็จ");
                    //$("#frmSavePurchaseOrder")[0].reset();
                    $("#global-loader").css('display', 'none');
                    dataTable.ajax.reload();

                    //To do next?
                    //window.location = data.url;
                    ResetForm();
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

function AddOrderItem(form) {
    console.log('Call => SubmitAddOrderItem');
    console.log(form);
    $.validator.unobtrusive.parse(form);
    var data = $(form).serializeJSON();
    data = JSON.stringify(data);
    console.log(data);

    var frmAddOrderItem = $("#frmAddOrderItem");
    frmAddOrderItem.validate();
    var isValid = frmAddOrderItem.valid();
    if (!isValid) {
        ShowMessageError('กรุณาระบุข้อมูลให้ถูกต้องก่อนทำรายการ');
        return;
    }
    $.ajax({
        type: 'POST',
        url: '/Order/AddTempItem',
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

                $("#amount").val(data.amount).trigger('change');;
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
                url: '/Order/DeleteTempItem',
                dataType: 'JSON',
                data: { "seq": id },
                success: function (response) {
                    if (response.result) {

                        ShowMessageSuccess('ลบข้อมูลสำเร็จ');
                        $("#global-loader").css('display', 'none');

                        dataTable.ajax.reload();

                        //Set sum amount
                        $("#amount").val(response.amount).trigger('change');
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

function CalculateAmountByItemPrice(price, name) {
    //var res = name.split('[');
    //var resIdx = res[1].split(']');

    var qty = $("#nqty").val() | 0;
    var total = parseFloat(price) * qty;

    $("#totalprice").val(total.toFixed(2));

    //Sum total amount
    //var totalRow = parseInt($("#totalrow").val());
    //var totalAmt = 0;
    //for (var i = 0; i < totalRow; i++) {
    //    var txtAmt = $("input[name='outer-item-group[" + i + "][txtAmount]']").val() | 0;
    //    totalAmt += parseFloat(txtAmt);
    //}
    //$("#amount").val(currencyFormat(totalAmt));

}

function CalculateAmountByItemQty(qty, name) {
    //var res = name.split('[');
    //var resIdx = res[1].split(']');

    var itemPrice = $("#price").val();
    var total = parseFloat(itemPrice) * qty;
    $("#totalprice").val(total.toFixed(2));
    //$("input[name='outer-item-group[" + resIdx[0] + "][txtAmount]']").val(total.toFixed(2));

    ////Sum total amount
    //var totalRow = parseInt($("#totalrow").val());
    //var totalAmt = 0;
    //for (var i = 0; i < totalRow; i++) {
    //    var txtAmt = $("input[name='outer-item-group[" + i + "][txtAmount]']").val() | 0;
    //    totalAmt += parseFloat(txtAmt);
    //}
    //$("#amount").val(currencyFormat(totalAmt));
}

function CalculateTotalAmountByAmount(amount) {
    var discount = $("#discount").val() | 0;
    var totalAmount = parseFloat(amount) - parseFloat(discount);

    $("#total").val(totalAmount.toFixed(2));
}

function CalculateTotalAmountByDiscount(discount) {
    var amount = $("#amount").val() | 0;
    var totalAmount = parseFloat(amount) - parseFloat(discount);

    $("#total").val(totalAmount.toFixed(2));
}

function ResetForm() {
    //Reset form
    $('#frmSavePurchaseOrder')[0].reset(); // [0] gets the DOM element from the jQuery object

    /*$("#npurchasetypeid").empty();*/
    /*var source_option = new Option("-- เลือกประเภทออเดอร์--", "", true, true);*/
    $("#npurchasetypeid").val("").trigger('change');

    $("#npaymenttypeid").val("").trigger('change');

    $("#nsupplierid").val("").trigger('change');
    
}
