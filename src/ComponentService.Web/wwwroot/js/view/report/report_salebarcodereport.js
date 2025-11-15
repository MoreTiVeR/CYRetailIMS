var datatable;

datatable = $("#tblSaleBarcodeReport").DataTable({
    "destroy": true,
    "bFilter": true,
    "sDom": 'fBtlpi',
    'pagingType': 'numbers',
    "ordering": true,
    "ajax": {
        "url": "/Report/SearchSaleBarcodeReport",
        "type": "POST",
        "contentType": 'application/json',
        "data": function (d) {
            // Build viewmodel payload expected by server
            var payload = {
                startdate: $("#txtStartDate").val() || "",
                enddate: $("#txtEndDate").val() || "",
                branchid: (function () { var v = $('#ddlBranch').val(); return v && v !== "" ? parseInt(v) : null; })(),
                // include common fields used by some server endpoints to avoid null binding issues
                items: [],
                isexportalldata: false,
                draw: d && d.draw ? d.draw : 0,
                start: d && d.start ? d.start : 0,
                length: d && d.length ? d.length : 0
            };
            return JSON.stringify(payload);
        },
        "dataSrc": function (json) {
            // server returns { result:true, message:'', data: [...] }
            if (!json) return [];
            if (json.result === false) {
                // optionally show error
                if (typeof AlertError === 'function') AlertError(json.message || 'ไม่สามารถดึงข้อมูลได้');
                console.error('SearchSaleBarcodeReport error:', json.message, json);
                return [];
            }
            return json.data || [];
        },
        "error": function (xhr, textStatus, errorThrown) {
            var msg = 'เกิดข้อผิดพลาดในการดึงข้อมูล';
            try {
                var res = JSON.parse(xhr.responseText);
                if (res && res.message) msg = res.message;
            } catch (e) { }
            console.error('AJAX error:', xhr.responseText || errorThrown);
            if (typeof AlertError === 'function') AlertError(msg + (errorThrown ? ' | ' + errorThrown : ''));
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
        {
            "data": "monthyear",
            "render": function (data, type, row) {
                if (data) return data;
                if (row.transactiondate) {
                    var dt = new Date(row.transactiondate);
                    if (isNaN(dt.getTime())) return '';
                    return ('0' + (dt.getMonth() + 1)).slice(-2) + '-' + dt.getFullYear();
                }
                return '';
            }
        },
        { "data": "branchname" },
        { "data": "username" },
        { "data": "amountcash", "render": $.fn.dataTable.render.number(',', '.', 2) },
        { "data": "amounttransfer", "render": $.fn.dataTable.render.number(',', '.', 2) },
        { "data": "substitutefee", "render": $.fn.dataTable.render.number(',', '.', 2) },
        { "data": "depositfee", "render": $.fn.dataTable.render.number(',', '.', 2) },
        { "data": "otherfee", "render": $.fn.dataTable.render.number(',', '.', 2) },
        { "data": "totalamount", "render": $.fn.dataTable.render.number(',', '.', 2) },
        { "data": "vat", "render": $.fn.dataTable.render.number(',', '.', 2) },
        { "data": "discount", "render": $.fn.dataTable.render.number(',', '.', 2) },
        { "data": "othernote" },
        { "data": "status" },
        { "data": "auditorname" },
        { "data": "referenceno" }
    ],
    "order": [[1, "desc"]],
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
            title: 'SaleBarcodeReport',
            text: 'ดาวโหลดไฟล์ Excel',
            class: 'btn-primary',
            //Columns to export (exclude checkbox column index0)
            exportOptions: {
                columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16]
            }
        },
        {
            extend: 'pdfHtml5',
            title: 'SaleBarcodeReport',
            text: 'Export to PDF',
            exportOptions: {
                columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16]
            }
        }
    ]
});

$("#btnSearch").on('click', function (event) {
    event.preventDefault(); // Prevent the default form submission
    // reload datatable which will send the SearchSaleBarcodeReportViewModel payload
    datatable.ajax.reload();
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