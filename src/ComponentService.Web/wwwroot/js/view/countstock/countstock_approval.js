/**
 * countstock_approval.js
 * หน้ารออนุมัติ — รายการนับสต๊อก
 * Admin กดอนุมัติได้เฉพาะรายการ HeadPC ที่สถานะ = Submitted (1)
 */

var tableApproval;
var pendingApproveId = null;
var isAdmin = false;

$(document).ready(function () {

    isAdmin = ($('#hdnIsAdmin').val() === 'true');

    // select2
    $('.select2').select2();

    // Filter toggle
    $("#filter_search").on('click', function () {
        $("#filter_inputs").slideToggle("slow");
        $(this).toggleClass('setclose');
    });

    // ========== Load on page ready ==========
    loadApprovals();

    // ========== Search ==========
    $('#btnSearchApproval').on('click', function (e) {
        e.preventDefault();
        loadApprovals();
    });

    function loadApprovals() {
        ShowLoading();
        var counterRole = $('#ddlCounterRole').val() || null;
        var statusVal   = $('#ddlApproveStatus').val();
        var statusId    = statusVal !== '' && statusVal !== null ? parseInt(statusVal) : null;

        $.ajax({
            url: '/Stock/GetPendingApprovals',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                counterrole: counterRole,
                statuscid: statusId,
                draw: 1, start: 0, length: 9999, searchValue: ''
            }),
            success: function (res) {
                HideLoading();
                if (!res || !res.data) {
                    ShowMessageError('ไม่สามารถโหลดข้อมูลได้');
                    return;
                }
                renderTable(res.data);
            },
            error: function () {
                HideLoading();
                ShowMessageError('เกิดข้อผิดพลาดในการโหลดข้อมูล');
            }
        });
    }

    function renderTable(data) {
        if (tableApproval) {
            tableApproval.destroy();
            $('#tbPendingApproval tbody').empty();
        }

        var columns = [
            { data: 'countstockid',  visible: false },
            { data: 'branchid',      visible: false },
            {
                data: 'countstockdate',
                render: function (d) {
                    if (!d) return '-';
                    try {
                        return formatDateTime ? formatDateTime(new Date(d)) : new Date(d).toLocaleDateString('th-TH');
                    } catch (e) { return d; }
                }
            },
            { data: 'branchname' },
            {
                data: 'createdby',
                render: function (d, t, row) {
                    var badge = row.counterrole === 'HeadPC'
                        ? '<span class="badges bg-lightpurple">หัวหน้า PC</span>&nbsp;'
                        : '<span class="badges bg-lightblue">PC</span>&nbsp;';
                    return badge + escHtml(d);
                }
            },
            {
                data: 'counterstockstatusname',
                className: 'text-center',
                render: function (d, t, row) {
                    var cls = row.counterstockstatusid === 1 ? 'bg-lightyellow'
                            : row.counterstockstatusid === 2 ? 'bg-lightgreen'
                            : 'bg-lightgrey';
                    return '<span class="badges ' + cls + '">' + escHtml(d) + '</span>';
                }
            },
            {
                data: 'waitingdays',
                className: 'text-center',
                render: function (d) {
                    var days = d || 0;
                    return days > 3
                        ? '<span style="color:#FF0000;font-weight:600">' + days + ' วัน</span>'
                        : days + ' วัน';
                }
            },
            {
                data: 'exceldownloadurl',
                className: 'text-center',
                render: function (d) {
                    return '<a href="' + escHtml(d) + '" class="btn btn-sm btn-info" title="ดาวน์โหลด Excel">Excel</a>';
                }
            }
        ];

        if (isAdmin) {
            columns.push({
                data: null,
                className: 'text-center',
                render: function (d, t, row) {
                    if (row.counterrole === 'HeadPC' && row.counterstockstatusid === 1) {
                        return '<a class="btn btn-sm btn-added btn-approve" data-id="' + row.countstockid + '" href="javascript:void(0);">อนุมัติ</a>';
                    }
                    if (row.counterstockstatusid === 2) {
                        return '<span class="badges bg-lightgreen">อนุมัติแล้ว</span>';
                    }
                    return '<span class="text-muted">-</span>';
                }
            });
        }

        tableApproval = $('#tbPendingApproval').DataTable({
            destroy: true,
            data: data,
            paging: true,
            pageLength: 20,
            ordering: true,
            bFilter: true,
            sDom: '<"top"fB>rt<"bottom"lpi><"clear">',
            columns: columns,
            language: {
                search: ' ',
                sLengthMenu: '_MENU_',
                searchPlaceholder: 'ค้นหา...',
                info: '_START_ - _END_ of _TOTAL_ รายการ',
                emptyTable: 'ไม่พบข้อมูล',
                infoEmpty: 'ไม่พบรายการ',
                paginate: { next: 'ถัดไป', previous: 'ก่อนหน้า' }
            },
            buttons: [
                {
                    extend: 'excelHtml5',
                    title: 'รายงานรออนุมัตินับสต๊อก',
                    text: 'ดาวน์โหลด Excel',
                    className: 'btn-primary',
                    exportOptions: { columns: ':visible:not(:last-child)' }
                }
            ],
            initComplete: function () {
                $('.dataTables_filter').appendTo('.search-input');
            }
        });
    }

    // ========== Approve Button (event delegation) ==========
    $('#tbPendingApproval').on('click', '.btn-approve', function (e) {
        e.preventDefault();
        pendingApproveId = parseInt($(this).data('id'));
        if (!pendingApproveId) return;

        var $modal = $('#modalConfirmApprove');
        if (typeof bootstrap !== 'undefined' && bootstrap.Modal) {
            new bootstrap.Modal($modal[0]).show();
        } else {
            $modal.modal('show');
        }
    });

    $('#btnConfirmApprove').on('click', function () {
        if (!pendingApproveId) return;

        var $modal = $('#modalConfirmApprove');
        if (typeof bootstrap !== 'undefined' && bootstrap.Modal) {
            bootstrap.Modal.getInstance($modal[0]).hide();
        } else {
            $modal.modal('hide');
        }

        ShowLoading();
        $.ajax({
            url: '/Stock/ApproveCountStockNew',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ CountStockID: pendingApproveId }),
            success: function (res) {
                HideLoading();
                var savedId = pendingApproveId;
                pendingApproveId = null;
                if (res.result) {
                    Swal.fire({
                        icon: 'success',
                        title: 'อนุมัติสำเร็จ',
                        text: 'ปรับสต๊อกในระบบเรียบร้อยแล้ว',
                        confirmButtonText: 'ตกลง'
                    }).then(function () { loadApprovals(); });
                } else {
                    Swal.fire({ icon: 'error', title: 'ไม่สำเร็จ', text: res.message });
                }
            },
            error: function () {
                HideLoading();
                pendingApproveId = null;
                ShowMessageError('เกิดข้อผิดพลาด');
            }
        });
    });

    function escHtml(str) {
        if (!str) return '';
        return $('<div>').text(String(str)).html();
    }
});
