var datatable;

datatable = $("#tbBrands").DataTable({
    "destroy": true,
    "bFilter": true,
    "sDom": 'fBtlpi',
    'pagingType': 'numbers',
    "ordering": true,
    "ajax": {
        "url": "/ItemBrand/GetItemBrands",
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
        { "data": "brandname" },
        { "data": "brandshortname" },
        { "data": "description" },
        { "data": "createdby" },
        {
            "data": { createddate: "createddate" },
            "render": function (data) {
                if (data.createddate === null || data.createddate == null) {
                    return data.createddate;
                }
                return formatDateTime(new Date(data.createddate));
            }
        },
        { "data": "updatedby" },
        {
            "data": { updateddate: "updateddate" },
            "render": function (data) {
                if (data.updateddate === null || data.updateddate == null) {
                    return data.updateddate;
                }
                return formatDateTime(new Date(data.updateddate));
            }
        },
        {
            "data": { isactive: "isactive" },
            "render": function (data) {
                if (data.isactive) {
                    return "<span class='badges bg-lightgreen'>ใช้งาน</span>";
                }
                else {
                    return "<span class='badges bg-lightyellow'>ไม่ใช้งาน</span>";
                }
            }
        },
        {
            "data": { brandid: "brandid" },
            "render": function (data) {
                return "<a class='me-3' href='Edit?brandID=" + data.brandid + "' title='แก้ไขข้อมูลแบรนด์'><img src='../assets/img/icons/edit.svg' alt='img'></a><a id='rowid" + data.brandid + "' onclick=deleteBrand(" + data.brandid + ") class='me-3'><img src='../assets/img/icons/delete.svg' alt='img'></a>";
            }
        }
    ],
    "order": [[1, "desc"]],
    "columnDefs": [
        {
            "targets": [0],
            "visible": false
        },
        {
            "targets": [7],
            "className": "text-center"
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
        $('.dataTables_filter').appendTo("#tbBrands");
        $('.dataTables_filter').appendTo('.search-input');
    },
    buttons: [
        {
            extend: 'excelHtml5',
            title: 'รายงานบันทึกการโอนเงิน',
            text: 'ดาวโหลดไฟล์ Excel',
            class: 'btn-primary',
            //Columns to export
            exportOptions: {
                columns: [2, 3, 4, 5, 6]
            }
        },
        {
            extend: 'pdfHtml5',
            title: 'PDF',
            text: 'Export to PDF'
        }
    ]
});

function deleteBrand(brandid) {
    Swal.fire({
        title: "ยืนยันการลบข้อมูล?",
        html: "<span class='text-danger'>เมื่อลบข้อมูลแล้ว จะไม่สามารถทำการยกเลิกได้!</span>",
        icon: 'warning',
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "ยืนยัน",
        cancelButtonText: "ยกเลิก",
        //confirmButtonClass: "btn btn-primary",
        //cancelButtonClass: "btn btn-danger ml-1",
        customClass: {
            confirmButton: "btn btn-primary",
            cancelButton: "btn btn-danger ml-1"
        },
        buttonsStyling: false,
    }).then(function (t) {
        if (t.value) {

            ShowLoading();

            //Delete
            $.ajax({
                statusCode: {
                    404: function () {
                        AlertError("ไม่พบหน้าเพจที่เรียกรายการ");
                        HideLoading();
                    }
                },
                type: 'POST',
                url: '/ItemBrand/DeleteBrand',
                data: JSON.stringify({ brandid: brandid }),
                contentType: 'application/json',
                success: function (data) {
                    if (data.result) {

                        AlertSuccess('ลบข้อมูลสำเร็จ');
                        HideLoading();

                        //$("#rowid" + itemid).closest("tr").remove();

                        //Reload data
                        //$('#tbBrands').DataTable().ajax.reload();
                        datatable.ajax.reload(null, false);
                        //$('#tbBrands').DataTable().clear().rows.add(response.data).draw();
                    }
                    else {
                        AlertError(data.message);
                        HideLoading();
                    }
                }
            });
        }
    });
}