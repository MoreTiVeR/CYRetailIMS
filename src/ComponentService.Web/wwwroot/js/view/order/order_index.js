var dataTable;

datatable = $("#tbOrders").DataTable({
    "destroy": true,
    "bFilter": true,
    "sDom": 'fBtlpi',
    'pagingType': 'numbers',
    "ordering": true,
    "ajax": {
        "url": "/Order/GetOrders",
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
        { "data": "purchaseorderno" },
        { "data": "suppliername" },
        {
            "data": { orderdate: "orderdate" },
            "render": function (data) {
                //var _orderdate = new Date(data.orderdate).toLocaleDateString("en-US");
                //return _orderdate;
                if (data.orderdate === null || data.orderdate == null) {
                    return data.orderdate;
                }
                return formatDateTime(new Date(data.orderdate));
            }
        },
        {
            "data": { receiveddate: "receiveddate" },
            "render": function (data) {
                //var _receiveddate = new Date(data.receiveddate).toLocaleDateString("en-US");
                //return _receiveddate;
                if (data.receiveddate === null || data.receiveddate == null) {
                    return data.receiveddate;
                }
                return formatDateTime(new Date(data.receiveddate));
            }
        },
        { "data": "paymentypename" },
        { "data": "amount" },
        { "data": "discount" },
        { "data": "total" },
        {
            "data": { approvestatus: "approvestatus", purchaseorderid: "purchaseorderid" },
            "render": function (data) {
                if (data.approvestatus == 1) {
                    return "<span class='badges bg-lightgreen'>รับสินค้าแล้ว</span>";
                }
                else if (data.approvestatus == 0) {
                    return "<a class='me-3' href='Edit?orderid=" + data.purchaseorderid + "' title='คลิก เพื่อรับสินค้า'><span class='badges bg-lightyellow'>อยู่ระหว่างการขนส่ง</span></a>";
                }
                else {
                    return "<span class='badges bg-lightred'>ยกเลิก</span>";
                }
            }
        },
        {
            //"data": "shipment.trackingno",
            "data": { trackingnos: "shipment.trackingno", approvestatus: "approvestatus" },
            "render": function (data) {
                if (data.shipment.trackingno == null) {
                    return "<span class='badges bg-lightred'>ยังไม่มีเลขพัสดุ</span>";
                }
                else {
                    return "<a class='me-3' href='https://www.best-inc.co.th/track?bills=" + data.shipment.trackingno + "' target='_blank' title='คลิก เพื่อตรวจสอบสถานะการส่งสินค้า'>" + data.shipment.trackingno + "</a>";
                }
            }
        },
        { "data": "remarks" },
        { "data": "createdby" },
        {
            "data": { purchaseorderid: "purchaseorderid" },
            "render": function (data) {
                var dict = {
                    "purchaseorderid": data.purchaseorderid,
                };
                console.log('data dic:' + dict);
                return "<a class='me-3' href='Edit?orderid=" + data.purchaseorderid + "' title='แก้ไขออเดอร์'><img src='../assets/img/icons/edit.svg' alt='img'></a><a href='#' id='rowid" + data.purchaseorderid + "' class='me-3' title='ลบข้อมูลออเดอร์' onclick='deleteItem(" + data.purchaseorderid +")'><img src='../assets/img/icons/delete.svg' alt='img'></a>";
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
        $('.dataTables_filter').appendTo("#tbOrders");
        $('.dataTables_filter').appendTo('.search-input');
    },
    /*dom: 'Bfrtip',*/
    buttons: [
        {
            extend: 'excelHtml5',
            title: 'รายงานการสั่งสินค้า',
            text: 'ดาวโหลดไฟล์ Excel',
            class: 'btn-primary',
            //Columns to export
            exportOptions: {
                 columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
             }
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

$("#btnSearch").on('click', function () {
    AlertWarn('ยังไม่เปิดให้ใช้งานค้นหา');
    //AlertSuccess($("#ddlItemType").val() + " | " + $("#ddlItemBrand").val());
});

$("#btnExportExcel").on('click', function () {
    AlertWarn('ยังไม่เปิดให้ใช้งานส่งออกไฟล์Excel');
});

function deleteItem(orderid) {

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
                url: '/Order/DeleteItem',
                data: JSON.stringify({ purchaseorderid: orderid }),
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

                        console.log("#rowid" + orderid);
                        $("#rowid" + orderid).closest("tr").remove();
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