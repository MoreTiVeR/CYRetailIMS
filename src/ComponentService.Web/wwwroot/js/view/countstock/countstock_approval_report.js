var tableApprovalReport;

$(document).ready(function () {
    $('.select2').select2();

    tableApprovalReport = $('#tbCountStockApprovalReport').DataTable({
        processing: true,
        serverSide: true,
        deferRender: true,
        paging: true,
        pageLength: 20,
        ordering: true,
        bFilter: true,
        sDom: '<"top"f>rt<"bottom"lpi><"clear">',
        ajax: {
            url: '/Report/GetCountStockApprovalReport',
            type: 'POST',
            contentType: 'application/json',
            data: function (d) {
                return JSON.stringify({
                    branchid: parseInt($('#ddlReportBranch').val()) || null,
                    startdate: $('#txtApprovedStartDate').val() || null,
                    enddate: $('#txtApprovedEndDate').val() || null,
                    draw: d.draw,
                    start: d.start,
                    length: d.length,
                    searchValue: d.search ? d.search.value : ''
                });
            }
        },
        columns: [
            { data: 'countstockid' },
            {
                data: 'countstockdate',
                render: function (d) {
                    if (!d) return '-';
                    return formatDateTime ? formatDateTime(new Date(d)) : new Date(d).toLocaleString('th-TH');
                }
            },
            { data: 'branchname' },
            {
                data: 'counterrole',
                render: function (d) {
                    if (d === 'HeadPC') return '<span class="badges bg-lightpurple">หัวหน้า PC</span>';
                    return '<span class="badges bg-lightblue">PC</span>';
                }
            },
            { data: 'approvedby' },
            {
                data: 'approveddate',
                render: function (d) {
                    if (!d) return '-';
                    return formatDateTime ? formatDateTime(new Date(d)) : new Date(d).toLocaleString('th-TH');
                }
            },
            { data: 'totalitems', className: 'text-right' },
            { data: 'totalqtybefore', className: 'text-right' },
            { data: 'totalqtyafter', className: 'text-right' },
            {
                data: 'totaladjustedqty',
                className: 'text-right',
                render: function (d) {
                    var val = d || 0;
                    var color = val < 0 ? '#dc3545' : val > 0 ? '#198754' : '#555';
                    var sign = val > 0 ? '+' : '';
                    return '<strong style="color:' + color + '">' + sign + val + '</strong>';
                }
            },
            {
                data: 'countstockid',
                className: 'text-center',
                orderable: false,
                render: function (d) {
                    return '<a class="btn-detail" ' +
                        'href="/Report/CountStockApprovalReportDetail?countstockid=' + d + '" ' +
                        'title="ดูรายละเอียด">' +
                        '<i class="fe fe-eye" aria-hidden="true"></i>' +
                        '<span>รายละเอียด</span>' +
                        '</a>';
                }
            }
        ],
        language: {
            search: ' ',
            sLengthMenu: '_MENU_',
            searchPlaceholder: 'ค้นหา...',
            info: '_START_ - _END_ of _TOTAL_ รายการ',
            emptyTable: 'ไม่พบข้อมูล',
            infoEmpty: 'ไม่พบรายการ',
            paginate: { next: 'ถัดไป', previous: 'ก่อนหน้า' }
        },
        initComplete: function () {
            $('.dataTables_filter').appendTo('.search-input');
        }
    });

    $('#btnSearchApprovalReport').on('click', function (e) {
        e.preventDefault();
        tableApprovalReport.ajax.reload();
    });

    $('#btnExportApprovalReport').on('click', function (e) {
        e.preventDefault();

        var params = new URLSearchParams();
        var branchVal = parseInt($('#ddlReportBranch').val()) || null;
        var startDate = $('#txtApprovedStartDate').val() || null;
        var endDate = $('#txtApprovedEndDate').val() || null;

        if (branchVal) params.append('branchid', branchVal);
        if (startDate) params.append('startdate', startDate);
        if (endDate) params.append('enddate', endDate);

        window.location.href = '/Report/ExportCountStockApprovalReportExcel?' + params.toString();
    });
});
