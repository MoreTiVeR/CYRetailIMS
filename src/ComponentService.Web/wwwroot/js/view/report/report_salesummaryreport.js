
var datatable;

datatable = $("#tbSaleSummaryReport").DataTable({
    "destroy": true,
    "bFilter": true,
    "sDom": 'fBtlpi',
    'pagingType': 'numbers',
    "ordering": true,
    "ajax": {
        "url": "/Report/GetSaleSummaryReport",
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
        {
            "data": { transactiondate: "transactiondate" },
            "render": function (data) {
                if (data.transactiondate === null || data.transactiondate == null) {
                    return data.transactiondate;
                }
                return formatDateTime(new Date(data.transactiondate));
                //var _createddate = new Date(data.createddate).toLocaleDateString("en-US");
                //return _createddate;
            }
        },
        { "data": "branchname" },
        {
            "data": { auditid: "auditid", totalauditamount: "totalauditamount", transactionid: "transactionid" },
            "render": function (data) {
                var _auditid = parseInt(data.auditid);
                if (_auditid > 0) {
                    return "<span class='badges bg-lightgreen'>" + data.totalauditamount + "</span>";
                }
                else {
                    return "<a href='AuditSaleSummaryReportByBranch?branchid=" + data.branchid + "'  class='me-3' title='คลิก เพื่อตรวจสอบ'><span class='badges bg-lightyellow'>รอตรวจสอบ</span></a>";
                }
            }
        },
        { "data": "totalamount" },
        { "data": "amounttransfer" },
        { "data": "amountdeposit" },
        { "data": "amountcash" },
        { "data": "depositfee" },
        { "data": "createdbystaff" },
        { "data": "auditdescription" }
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
        $('.dataTables_filter').appendTo("#tbSaleSummaryReport");
        $('.dataTables_filter').appendTo('.search-input');
    },
    /*dom: 'Bfrtip',*/
    buttons: [
        {
            extend: 'excelHtml5',
            title: 'รายงานสรุปยอด',
            text: 'ดาวโหลดไฟล์ Excel',
            class: 'btn-primary',
            //Columns to export
            exportOptions: {
                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
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