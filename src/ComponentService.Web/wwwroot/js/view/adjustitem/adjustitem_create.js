var dataTable;

InitialTableAdjustItem();
$('.select2').select2();

function InitialTableAdjustItem() {
    dataTable = $('#tbItems').DataTable({
        destroy: true,
        "searching": false,
        "paging": false,
        "ordering": false,
        "info": false,
        "ajax": {
            "url": "/AdjustItem/GetTempItem",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "nseq" },
            { "data": "sbranchname" },
            { "data": "sadjusttypename" },
            { "data": "sitemname" },
            { "data": "nqty" },
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

function SaveAdjustItem(form) {

    $("#global-loader").css('display', '');
    var frmSaveAdjustItem = $("#frmSaveAdjustItem");
    frmSaveAdjustItem.validate();
    var isValid = frmSaveAdjustItem.valid();
    if (isValid) {
        console.log('Call => AddAdjustItem');
        $.validator.unobtrusive.parse(form);
        var data = $(form).serializeJSON();
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/AdjustItem/CreateAdjustItem',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.result) {
                    //popup.dialog('close');

                    AlertSuccess("ปรับสต๊อกสินค้าสำเร็จ");
                    //$("#global-loader").css('display', 'none');
                    //dataTable.ajax.reload();
                    datatable.ajax.reload(function () {
                        // This function will be executed after the data has been reloaded
                        HideLoading();
                        console.log("Load data success.");
                    });
                    //To do next?
                    //window.location = data.url;
                }
                else {
                    AlertError(data.message);
                    //$("#global-loader").css('display', 'none');
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

function AddAdjustItem(form) {
    console.log('Call => SubmitAddAdjustItem');
    console.log(form);
    $.validator.unobtrusive.parse(form);
    var data = $(form).serializeJSON();
    data = JSON.stringify(data);
    console.log(data);

    var frmAddAdjustItem = $("#frmAddAdjustItem");
    frmAddAdjustItem.validate();
    var isValid = frmAddAdjustItem.valid();
    if (!isValid) {
        ShowMessageError('กรุณาระบุข้อมูลให้ถูกต้องก่อนทำรายการ');
        return;
    }
    $.ajax({
        type: 'POST',
        url: '/AdjustItem/AddTempItem',
        data: data,
        contentType: 'application/json',
        success: function (data) {
            if (data.result) {
                //popup.dialog('close');
                ShowMessageSuccess(data.message);
                dataTable.ajax.reload();
                $("#btnCloseMdl").click();
            }
        }
    });
    return false;
}

function ShowPopup(id) {
    //alert(id);
    $.post("AddEditAsync", { id: id }).then(
        function (results) {
            //Success
            $("#divEditItem").html(results);
            $('#divEditItem').modal('show');
        }, function (results) {
            //Failed
            console.log('Filed:' + results);
        }, function () {
            console.log('this will run if the deferred generates a progress update.');
        }
    );
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
                url: '/AdjustItem/DeleteTempItem',
                dataType: 'JSON',
                data: { "seq": id },
                success: function (response) {
                    if (response.result) {

                        ShowMessageSuccess('ลบข้อมูลสำเร็จ');
                        //$("#global-loader").css('display', 'none');
                        //dataTable.ajax.reload();
                        datatable.ajax.reload(function () {
                            // This function will be executed after the data has been reloaded
                            HideLoading();
                            console.log("Load data success.");
                        });
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

$("#nbranchid").on("change", function () {
    var text = $('option:selected', $(this)).text();
    var val = $('option:selected', $(this)).val();
    //alert(text + ' | ' + val);

    var request = $.ajax({
        url: '/AdjustItem/FillItemByBranchID',
        async: true,
        type: 'POST',
        dataType: 'JSON',
        data: { "branchid": val },
        success: function (response) {

            if (response.result) {
                var items_selectList = $('#nitemid');
                items_selectList.html(""); // clear before appending new list
                items_selectList.append($('<option></option>').val("").html("--เลือกสินค้า--")); //Add first itemList
                $.each(response.data, function () {
                    $("<option />").val(this.value).text(this.text).appendTo(items_selectList);
                    //prevGroupName = this.group.name;
                });
            }
            else {
                var items_selectList = $('#nitemid');
                items_selectList.html(""); // clear before appending new list
                items_selectList.append($('<option></option>').val("").html("--เลือกสินค้า--")); //Add first itemList
                ShowMessageError(response.message);
            }
            

        },
        failure: function (response) {
            ShowMessageError(response.message);
        },
        error: function (response) {
            ShowMessageError(response.message);
        }
    });

});