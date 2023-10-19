
var datatable;

datatable = $("#tbItems").DataTable({
    "destroy": true,
    "bFilter": true,
    "sDom": 'fBtlpi',
    'pagingType': 'numbers',
    "ordering": true,
    "ajax": {
        "url": "/Item/GetItems",
        "type": "GET",
        "datatype": "json"
    },
    "columns": [
        {
            "render": function () {
                console.log('render columns : checkbox');
                return "<label class='checkboxs'><input type='checkbox' id='select-all'><span class='checkmarks'></span></label>";
            }
        },
        //{
        //    "data": { itemimageurl: "itemimageurl", name: "name" },
        //    "render": function (data) {
        //        console.log('columns : render => ' + data);
        //        return "<a asp-action='Detail' asp-controller='Item' asp-all-route-data='aItemID'>" + data.name + "</a>";
        //    }
        //},
        { "data": "name" },
        { "data": "itemtypename" },
        { "data": "itemcode" },
        { "data": "brandname" },
        { "data": "qty" },
        { "data": "cost" },
        { "data": "price" },
        { "data": "notifyminqty" },
        { "data": "description" },
        { "data": "createdby" },
        { "data": "updatedby" },
        { "data": "updateddate" },
        {
            "data": { itemid: "itemid", isiteminbranch: "isiteminbranch", searchbranchid: "searchbranchid" },
            "render": function (data) {
                var dict = {
                    "itemid": data.itemid,
                };
                console.log('data dic:' + dict);
                if (data.isiteminbranch) {
                    //Branch
                    return "<a href='EditItemBranch?itemid=" + data.itemid + "'  class='me-3' title='แก้ไขข้อมูลสินค้า'><img src='../assets/img/icons/edit.svg' alt='img'></a><a id='rowid" + data.itemid + "' onclick=deleteItemInBranch(" + data.itemid + ',' + data.searchbranchid +") class='me-3'><img src='../assets/img/icons/delete.svg' alt='img'></a>";
                }
                else {
                    //Warehouse
                    return "<a href='Edit?itemid=" + data.itemid + "'  class='me-3' title='แก้ไขข้อมูลสินค้า'><img src='../assets/img/icons/edit.svg' alt='img'></a><a id='rowid" + data.itemid + "' onclick=deleteItem(" + data.itemid + ") class='me-3'><img src='../assets/img/icons/delete.svg' alt='img'></a>";
                }
                //return "<a href='Edit?itemid=" + data.itemid + "'  class='me-3' title='แก้ไขข้อมูลสินค้า'><img src='../assets/img/icons/edit.svg' alt='img'></a><a id='rowid" + data.itemid + "' onclick=deleteItem(" + data.itemid + ") class='me-3'><img src='../assets/img/icons/delete.svg' alt='img'></a>";
            }
        }
    ],
    //"language": {
    //    "emptyTable": "ไม่พบข้อมูล."
    //},
    "order": [[0, "desc"]],
    "columnDefs": [
        {
            "targets": [0],
            "visible": false
        }
    ],
    "language": {
        search: ' ',
        sLengthMenu: '_MENU_',
        searchPlaceholder: "ค้นหาข้อมูล...",
        info: "_START_ - _END_ of _TOTAL_ items",
        "emptyTable": "ไม่พบข้อมูล."
    },
    initComplete: (settings, json) => {
        $('.dataTables_filter').appendTo("#tbItems");
        $('.dataTables_filter').appendTo('.search-input');
    },
    dom: 'Bfrtip',
    buttons: [
        {
            extend: 'excelHtml5',
            title: 'รายงานขายสินค้า',
            text: 'ดาวโหลดไฟล์ Excel',
            class: 'btn-primary'
            //Columns to export
            //exportOptions: {
            //     columns: [0, 1, 2, 3,4,5,6]
            // }
        },
        {
            extend: 'pdfHtml5',
            title: 'PDF',
            text: 'Export to PDF'
            //Columns to export
            //exportOptions: {
            //     columns: [0, 1, 2, 3, 4, 5, 6]
            //  }
        }
    ]
});

$("#btnSearch").on('click', function (event) {
    event.preventDefault(); // Prevent the default form submission

    var val = $("#ddlItemType").val();
    var branchid = parseInt(val);
    var request = $.ajax({
        url: '/Item/SearchItemByBranch',
        async: true,
        type: 'POST',
        dataType: 'JSON',
        data: { "branchid": branchid },
        success: function (response) {

            if (response.result) {
                ShowMessageSuccess(response.message);
                
                //Update the DataTable with the filtered data from the server
                console.log(response.data);
                $("#tbItems").DataTable().clear().rows.add(response.data).draw();
            }
            else {
                AlertError(response.message);
            }

        },
        failure: function (response) {
            AlertError(response.message);
        },
        error: function (response) {
            AlertError(response.message);
        }
    });

    //$.ajax({
    //    type: "POST",
    //    url: "/Item/SearchItem", // Replace with the URL of your search API
    //    data: JSON.stringify({ branchid: val }),
    //    success: function (response) {
    //        // Update the DataTable with the filtered data from the server
    //        console.log(response.data);
    //        dataTable.clear().rows.add(response.data).draw();
    //    },
    //    error: function (error) {
    //        console.log('Error:', error);
    //    }
    //});

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
                        //$("#rowid" + itemid).closest("tr").remove();

                        //Reload data
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

function deleteItemInBranch(itemid, searchbranchid) {
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
                url: '/Item/DeleteItemInBranch',
                data: JSON.stringify({ itemid: itemid, searchbranchid: searchbranchid }),
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

                        //Reload data
                        //$('#tbItems').DataTable().ajax.reload();

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
//$('#searchForm').submit(function (event) {
//    event.preventDefault(); // Prevent the default form submission

//    var searchQuery = $('input[name="searchQuery"]').val();

//    $.ajax({
//        type: 'POST',
//        url: '/your-search-endpoint', // Replace with the URL of your search API
//        data: { searchQuery: searchQuery },
//        success: function (data) {
//            // Update the DataTable with the filtered data from the server
//            dataTable.clear().rows.add(data.data).draw();
//        },
//        error: function (error) {
//            console.log('Error:', error);
//        }
//    });
//});

