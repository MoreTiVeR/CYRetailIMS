
var dataTable;

datatable = $("#tbBranchs").DataTable({
    "destroy": true,
    "bFilter": true,
    "sDom": 'fBtlpi',
    'pagingType': 'numbers',
    "ordering": true,
    "ajax": {
        "url": "/Branch/GetBranchs",
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
        { "data": "branchcode" },
        { "data": "branchname" },
        { "data": "address1", "className": "wrap-text" },
        {
            "data": { isactive: "isactive" },
            "render": function (data) {
                if (data.isactive) {
                    return "<span class='badges bg-lightgreen'>ใช้งาน</span>";
                }
                else {
                    return "<span class='badges bg-lightyellow'>ยกเลิก</span>";
                }
            }
        },
        {
            "data": { branchid: "branchid" },
            "render": function (data) {
                return "<a href='Edit?branchid=" + data.branchid + "' class='me-3' title='แก้ไขข้อมูลสาขา'><img src='../assets/img/icons/edit.svg' alt='img'></a><a id='rowid" + data.branchid + "' onclick=deleteBranch(" + data.branchid + ") title='ลบสาขา' class='me-3'><img src='../assets/img/icons/delete.svg' alt='img'></a>";
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
        $('.dataTables_filter').appendTo("#tbBranchs");
        $('.dataTables_filter').appendTo('.search-input');
    },
    /*dom: 'Bfrtip',*/
    buttons: [
        {
            extend: 'excelHtml5',
            title: 'รายชื่อสาขา',
            text: 'ดาวโหลดไฟล์ Excel',
            class: 'btn-primary',
            //Columns to export
            exportOptions: {
                columns: [0, 1, 2, 3]
            }
        },
        {
            extend: 'pdfHtml5',
            title: 'PDF',
            text: 'Export to PDF'
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

function deleteBranch(branchid) {

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
                url: '/Branch/DeleteBranch',
                data: JSON.stringify({ branchid: branchid }),
                contentType: 'application/json',
                success: function (data) {
                    if (data.result) {

                        AlertSuccess('ลบข้อมูลสำเร็จ');
                        $("#global-loader").css('display', 'none');

                        console.log("#rowid" + branchid);
                        $("#rowid" + branchid).closest("tr").remove();
                        $('#tbBranchs').DataTable().ajax.reload();
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