var dataTable;

datatable = $("#tbEmployees").DataTable({
    "destroy": true,
    "bFilter": true,
    "sDom": 'fBtlpi',
    'pagingType': 'numbers',
    "ordering": true,
    "ajax": {
        "url": "/EmployeeManagement/GetEmployees",
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
        { "data": "firstname" },
        { "data": "lastname" },
        { "data": "mobileno" },
        { "data": "email" },
        {
            "data": { isregister: "isregister" },
            "render": function (data) {
                console.log("isregister : " + data.isregister);
                if (data.isregister === true) {
                    return "<span class='badges bg-lightgreen'>ผูกบัญชีแล้ว</span>";
                }
                else {
                    return "<span class='badges bg-lightred'>ยังไม่ผูกบัญชี</span>";
                }
            }
        },
        { "data": "createdby" },
        {
            "data": { createddate: "createddate" },
            "render": function (data) {
                if (data.createddate === null || data.createddate == null) {
                    return data.createddate;
                }
                return formatDateTime(new Date(data.createddate));
                //var _createddate = new Date(data.createddate).toLocaleDateString("en-US");
                //return _createddate;
            }
        },
        {
            "data": { isactive: "isactive" },
            "render": function (data) {
                console.log("isactive : " + data.isactive);
                if (data.isactive === true) {
                    return "<span class='badges bg-lightgreen'>ใช้งาน</span>";
                }
                else {
                    return "<span class='badges bg-lightred'>ไม่ใช้งาน</span>";
                }
            }
        },
        {
            "data": { empid: "empid" },
            "render": function (data) {
                return "<a href='Edit?empid=" + data.empid + "' class='me-3' title='แก้ไขข้อมูล'><img src='../assets/img/icons/edit.svg' alt='img'></a><a id='rowid" + data.empid + "' onclick=deleteEmp(" + data.empid + ") title='ลบข้อมูล' class='me-3'><img src='../assets/img/icons/delete.svg' alt='img'></a>";
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
        $('.dataTables_filter').appendTo("#tbEmployees");
        $('.dataTables_filter').appendTo('.search-input');
    },
    /*dom: 'Bfrtip',*/
    buttons: [
        {
            extend: 'excelHtml5',
            title: 'รายชื่อพนักงาน',
            text: 'ดาวโหลดไฟล์ Excel',
            class: 'btn-primary',
            //Columns to export
            exportOptions: {
                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8]
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

function deleteEmp(id) {

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
                url: '/EmployeeManagement/DeleteEmployee',
                data: JSON.stringify({ empid: id }),
                contentType: 'application/json',
                success: function (data) {
                    if (data.result) {

                        AlertSuccess(data.message);
                        $("#global-loader").css('display', 'none');

                        console.log("#rowid" + id);
                        $("#rowid" + id).closest("tr").remove();
                        /*$('#tbEmployees').DataTable().ajax.reload();*/
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