/**
 * countstock_compare.js
 * หน้าเทียบข้อมูลสต๊อก — เปรียบเทียบสต๊อกระบบ CY กับยอดที่นับได้
 */

var tableCompare;

$(document).ready(function () {

    // select2
    $('.select2').select2();
    // Note: #filter_search toggle is handled globally by assets/js/script.js

    // ========== Search ==========
    $('#btnSearchCompare').on('click', function (e) {
        e.preventDefault();
        var branchId = parseInt($('#ddlCompareBranch').val());
        if (!branchId) {
            Swal.fire({ icon: 'warning', title: 'กรุณาเลือกสาขา', confirmButtonText: 'ตกลง' });
            return;
        }
        loadComparison(branchId);
    });

    function loadComparison(branchId) {
        ShowLoading();
        $.ajax({
            url: '/Stock/GetCountStockComparison',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                branchid: branchId,
                subitemtypename: $('#ddlCompareItemType').val() || null,
                salesstartdate: $('#txtSalesStart').val() || null,
                salesenddate: $('#txtSalesEnd').val() || null,
                auditstartdate: $('#txtAuditStart').val() || null,
                auditenddate: $('#txtAuditEnd').val() || null,
                draw: 1,
                start: 0,
                length: 9999,
                searchValue: ''
            }),
            success: function (res) {
                HideLoading();
                if (!res || !res.data) {
                    ShowMessageError('ไม่สามารถโหลดข้อมูลได้');
                    return;
                }
                initTable(res.data);
            },
            error: function () {
                HideLoading();
                ShowMessageError('เกิดข้อผิดพลาดในการโหลดข้อมูล');
            }
        });
    }

    function initTable(data) {
        if (tableCompare) {
            tableCompare.destroy();
            $('#tbCompare tbody').empty();
        }

        tableCompare = $('#tbCompare').DataTable({
            destroy: true,
            data: data,
            paging: true,
            pageLength: 20,
            ordering: true,
            bFilter: true,
            sDom: '<"top"fB>rt<"bottom"lpi><"clear">',
            columns: [
                { data: 'subitemtypeid', visible: false },
                { data: 'itemcode' },
                { data: 'itemname' },
                { data: 'subitemtypename' },
                {
                    data: 'headpc_countedqty',
                    className: 'text-right',
                    render: function (d) {
                        return d !== null && d !== undefined
                            ? '<span style="font-weight:600">' + d + '</span>'
                            : '<span class="badges bg-lightgrey">-</span>';
                    }
                },
                { data: 'cy_stockqty', className: 'text-right', render: function (d) { return '<strong>' + (d || 0) + '</strong>'; } },
                { data: 'salesqty', className: 'text-right', render: function (d) { return d || 0; } },
                { data: 'stockinqty', className: 'text-right', render: function (d) { return d || 0; } },
                { data: 'stockoutqty', className: 'text-right', render: function (d) { return d || 0; } },
                { data: 'pc_countedqty', className: 'text-right', render: function (d) { return '<strong>' + (d || 0) + '</strong>'; } },
                {
                    data: 'shortagesurplusqty',
                    className: 'text-right',
                    render: function (d) {
                        var val = d || 0;
                        var color = val < 0 ? 'color:#FF0000' : val > 0 ? 'color:#28a745' : 'color:#555';
                        return '<strong style="' + color + '">' + (val > 0 ? '+' : '') + val + '</strong>';
                    }
                }
            ],
            language: {
                search: ' ',
                sLengthMenu: '_MENU_',
                searchPlaceholder: 'ค้นหา...',
                info: '_START_ - _END_ of _TOTAL_ รายการ',
                emptyTable: 'ไม่พบข้อมูล กรุณาเลือกสาขาและกดค้นหา',
                infoEmpty: 'ไม่พบรายการ',
                paginate: { next: 'ถัดไป', previous: 'ก่อนหน้า' }
            },
            buttons: [
                {
                    extend: 'excelHtml5',
                    title: 'รายงานเทียบสต๊อก',
                    text: 'ดาวน์โหลด Excel',
                    className: 'btn-primary d-none',
                    exportOptions: { columns: ':visible' }
                }
            ],
            initComplete: function () {
                $('.dataTables_filter').appendTo('.search-input');
            }
        });

        // Show/bind export button
        $('#btnExportCompare').show().off('click').on('click', function (e) {
            e.preventDefault();
            tableCompare.button('.buttons-excel').trigger();
        });
    }

});
