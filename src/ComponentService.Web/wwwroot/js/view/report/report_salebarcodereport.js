var datatable;
$('.select2').select2();
InitialDataTable();

function InitialDataTable() {
    datatable = $("#tblSaleBarcodeReport").DataTable({
        "processing": true,         // Show processing indicator
        "serverSide": true,        // Enable server-side processing
        "destroy": true,
        "bFilter": true,
        stateSave: true,
        //"sDom": '<"top"B>fr<"bottom"ilp><"clear">',
        "sDom": '<"top"fB>rt<"bottom"lpi><"clear">',
        "pagingType": 'numbers',
        "ordering": true,
        "ajax": {
            "url": "/Report/SearchSaleBarcodeReport",
            "type": "POST",
            "contentType": 'application/json',
            "data": function (data) {
                data.startdate = $("#txtStartDate").val();
                data.enddate = $("#txtEndDate").val();

                var selectedBranch = $('.ddl-branch').val();
                var branchid = isNaN(parseInt(selectedBranch, 10)) ? null : parseInt(selectedBranch, 10); // Parse and if NaN, set to -1

                data.branchid = branchid;
                data.draw = data.draw;
                data.start = data.start;
                data.length = data.length;
                data.searchValue = data.search.value;
                console.log(data);
                return JSON.stringify(data);
            }
        },
        "columns": [
            {
                "render": function () {
                    console.log('render columns : checkbox');
                    return "<label class='checkboxs'><input type='checkbox' id='select-all'><span class='checkmarks'></span></label>";
                }
            },
            {
                "data": { transactiondate: "transactiondate" },
                "render": function (data) {
                    if (data === null || data == null) {
                        return '';
                    }
                    // if data is object with transactiondate property, handle both
                    var dt = data && data.transactiondate ? new Date(data.transactiondate) : new Date(data);
                    if (isNaN(dt.getTime())) return '';
                    return formatDate(dt);
                }
            },
            //{
            //    "data": "monthyear",
            //    "render": function (data, type, row) {
            //        if (data) return data;
            //        if (row.transactiondate) {
            //            var dt = new Date(row.transactiondate);
            //            if (isNaN(dt.getTime())) return '';
            //            return ('0' + (dt.getMonth() + 1)).slice(-2) + '-' + dt.getFullYear();
            //        }
            //        return '';
            //    }
            //},
            { "data": "branchname" },
            { "data": "username" },
            { "data": "amountcash" },
            { "data": "amounttransfer" },
            { "data": "substitutefee" },
            { "data": "depositfee" },
            { "data": "otherfee" },
            { "data": "totalamount" },
            { "data": "vat" },
            { "data": "discount" },
            { "data": "remark" },
            {
                "data": { auditid: "auditid" },
                "render": function (data) {
                    var _auditid = parseInt(data.auditid);
                    if (_auditid > 0) {
                        return "<span class='badges bg-lightgreen'>ถูกต้อง</span>";
                    }
                    else {
                        return "<span class='badges bg-lightred'>รอตรวจสอบ</span>";
                    }
                }
            },
            { "data": "auditorname" },
            { "data": "referenceno" }
        ],
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
            $('.dataTables_filter').appendTo('.search-input');
        },
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'รายงานสรุปยอดสิ้นวันบาร์โค้ด',
                text: 'ดาวโหลดไฟล์ Excel',
                class: 'btn-primary',
                //Columns to export (exclude checkbox column index0)
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15]
                }
            },
            {
                extend: 'pdfHtml5',
                title: 'รายงานสรุปยอดสิ้นวันบาร์โค้ด',
                text: 'Export to PDF',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15]
                }
            }
        ]
    });
}

$("#btnSearch").on('click', function (event) {
    ShowLoading();
    event.preventDefault(); // Prevent the default form submission
    datatable.ajax.reload(); // This will use the updated parameters automatically
    HideLoading();
});

$(function () {
    $('.select2').select2();
    if ($.fn.datetimepicker) {
        $('.datetimepicker').datetimepicker({ format: 'DD-MM-YYYY' });
    }
});

// utility formatDate (simple)
function formatDate(d) {
    if (!d) return '';
    var dd = ('0' + d.getDate()).slice(-2);
    var mm = ('0' + (d.getMonth() + 1)).slice(-2);
    var yyyy = d.getFullYear();
    return dd + '/' + mm + '/' + yyyy;
}