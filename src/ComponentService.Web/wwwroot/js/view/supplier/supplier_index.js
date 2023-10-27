
var datatable;

datatable = $("#tbSuppliers").DataTable({
    "destroy": true,
    "bFilter": true,
    "sDom": 'fBtlpi',
    'pagingType': 'numbers',
    "ordering": true,
    "ajax": {
        "url": "/SupplierManagement/GetSuppliers",
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
        { "data": "suppliername_th" },
        //{ "data": "suppliername_en" },
        //{ "data": "suppliertypename" },
        //{ "data": "suppliercontacttypename" },
        //{ "data": "contactaccountname" },
        { "data": "contactperson" },
        { "data": "mobileno" },
        { "data": "description" },
        { "data": "createdby" },
        {
            "data": { creadeddate: "creadeddate" },
            "render": function (data) {
                //var _creadeddate = new Date(data.creadeddate).toLocaleDateString("en-US");
                //return _creadeddate;
                if (data.createddate === null || data.createddate == null) {
                    return data.createddate;
                }
                return formatDateTime(new Date(data.createddate));
            }
        },
        {
            "data": { supplierid: "supplierid" },
            "render": function (data) {
                return "<a href='Edit?supplierid=" + data.supplierid + "'  class='me-3' title='แก้ไขข้อมูลซัฟพลายเออร์'><img src='../assets/img/icons/edit.svg' alt='img'></a><a id='rowid" + data.supplierid + "' onclick=deleteSupplier(" + data.supplierid + ") class='me-3'><img src='../assets/img/icons/delete.svg' alt='img'></a>";
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
        $('.dataTables_filter').appendTo("#tbSuppliers");
        $('.dataTables_filter').appendTo('.search-input');
    },
    /*dom: 'Bfrtip',*/
    buttons: [
        {
            extend: 'excelHtml5',
            title: 'รายงานซัฟพลายเออร์',
            text: 'ดาวโหลดไฟล์ Excel',
            class: 'btn-primary',
            //Columns to export
            exportOptions: {
                 columns: [0, 1, 2, 3, 4, 5]
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

$("#btnSearch").on('click', function (event) {
    event.preventDefault(); // Prevent the default form submission

    var val = $("#ddlItemType").val();
    var branchid = parseInt(val);
    var request = $.ajax({
        url: '/SupplierManagement/SearchItemByBranch',
        async: true,
        type: 'POST',
        dataType: 'JSON',
        data: { "branchid": branchid },
        success: function (response) {

            if (response.result) {
                ShowMessageSuccess(response.message);
                
                //Update the DataTable with the filtered data from the server
                console.log(response.data);
                $("#tbSuppliers").DataTable().clear().rows.add(response.data).draw();
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

});

$("#btnExportExcel").on('click', function () {
    AlertWarn('ยังไม่เปิดให้ใช้งานส่งออกไฟล์Excel');
});

function deleteSupplier(supplierid) {

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
                url: '/SupplierManagement/DeleteSupplier',
                data: JSON.stringify({ supplierid: supplierid }),
                contentType: 'application/json',
                success: function (data) {
                    if (data.result) {

                        AlertSuccess('ลบข้อมูลสำเร็จ');
                        $("#global-loader").css('display', 'none');
                        //ShowMessageSuccess(data.message);

                        console.log("#rowid" + supplierid);
                        //$("#rowid" + itemid).closest("tr").remove();

                        //Reload data
                        $('#tbSuppliers').DataTable().ajax.reload();
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
