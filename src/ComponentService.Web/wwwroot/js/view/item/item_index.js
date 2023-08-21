var dataTable;

$(document).ready(function () {
    
    //dataTable = $('#tbItems').DataTable({
    //    destroy: true,
    //    "searching": false,
    //    "ajax": {
    //        "url": "/Item/GetItems",
    //        "type": "GET",
    //        "datatype": "json"
    //    },
    //    "columns": [
    //        {
    //            "render": function () {
    //                console.log('render columns : checkbox');
    //                return "<label class='checkboxs'><input type='checkbox' id='select-all'><span class='checkmarks'></span></label>";
    //            }
    //        },
    //        {
    //            "data": { itemimageurl: "itemimageurl", name: "name" },
    //            "render": function (data) {
    //                console.log('columns : render => ' + data);
    //                return "<a asp-action='Detail' asp-controller='Item' asp-all-route-data='aItemID' title='คลิกเพื่อดูรายละเอียด' class='product-img'><img src='" + data.itemimageurl + "' alt='product'></a><a asp-action='Detail' asp-controller='Item' asp-all-route-data='aItemID'>" + data.name +"</a>";
    //            }
    //        },
    //        { "data": "itemtypename" },
    //        { "data": "itemcode" },
    //        { "data": "brandname" },
    //        { "data": "qty" },
    //        { "data": "cost" },
    //        { "data": "price" },
    //        { "data": "description" },
    //        { "data": "createdby" },
    //        { "data": "updatedby" },
    //        { "data": "updateddate" },
    //        {
    //            "data": "itemid",
    //            "render": function (data) {
    //                var dict = {
    //                    "itemid": data,
    //                };
    //                console.log('data dic:' + dict);
    //                return "<a id='delete' onclick=removerow(this) class='me-3'><img src='/assets/img/icons/eye.svg' alt='img'></a>";
    //            }
    //        }
    //    ],
    //    "language": {
    //        "emptyTable": "ไม่พบข้อมูล."
    //    },
    //    "order": [[0, "desc"]],
    //    "columnDefs": [
    //        {
    //            //"targets": [0],
    //            //"visible": false
    //        }
    //    ]
    //});
});


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
                data: JSON.stringify({ ItemID: itemid }),
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