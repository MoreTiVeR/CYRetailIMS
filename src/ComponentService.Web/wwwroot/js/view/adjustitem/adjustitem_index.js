var dataTable;

$("#btnSearch").on('click', function () {
    AlertWarn('ยังไม่เปิดให้ใช้งานค้นหา');
    //AlertSuccess($("#ddlItemType").val() + " | " + $("#ddlItemBrand").val());
});

$("#btnExportExcel").on('click', function () {
    AlertWarn('ยังไม่เปิดให้ใช้งานส่งออกไฟล์Excel');
});

function deleteItem(itemid) {

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
                url: '/Item/DeleteItem',
                data: JSON.stringify({ itemid: itemid }),
                contentType: 'application/json',
                success: function (data) {
                    if (data.result) {

                        AlertSuccess('ลบข้อมูลสำเร็จ');
                        $("#global-loader").css('display', 'none');
                        //ShowMessageSuccess(data.message);

                        //To do next?
                        //window.location = data.url;
                        //itemDataTable.row('.selected').remove().draw(false);
                        //dataTable.ajax.reload();
                        /*$("#tbItems").DataTable().ajax.reload();*/
                       /* $('#tbItems').DataTable().ajax.reload();*/
                        //$('#tbItems').DataTable().ajax.reload();

                        console.log("#rowid" + itemid);
                        $("#rowid" + itemid).closest("tr").remove();
                        $('#tbItems').DataTable().ajax.reload();
                        //$("#rowid" + itemid).closest("tr").remove().draw(false);
                        //console.log(row);
                        //$('#tbItems').DataTable().row(row).remove().draw(false);

                        //var row = $('#dataTable').DataTable().rows('.remove-row').closest('tr');
                        //alert('test -> ' + row);
                        //var rowdata = $('#tbItems').DataTable().row(row).data();
                        //alert('data -> ' + rowdata)
                        //AlertSuccess('ลบแถวสำเร็จ');
                    }
                    else {
                        //ShowMessageError(data.message);
                        AlertError(data.message);
                        $("#global-loader").css('display', 'none');
                    }
                }
            });
        }
    });
}