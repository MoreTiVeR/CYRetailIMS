/**
 * countstock_newentry.js
 * หน้านับสต๊อกแบบใหม่ — PC และ HeadPC กรอกข้อมูลนับสต๊อก
 * สามารถกรองตาม ItemType และค้นหารายการสินค้าได้
 * บันทึกแบบร่าง (Draft) หรือบันทึกส่งข้อมูล (Submit)
 */

var tableEntry;
var loadedData = [];   // raw data from API
var counterRole = 'PC';

$(document).ready(function () {

    counterRole = $('#hdnCounterRole').val() || 'PC';

    // select2
    $('.select2').select2();

    // Filter toggle (existing theme pattern)
    $("#filter_search").on('click', function () {
        var dv = $("#filter_inputs");
        dv.slideToggle("slow");
        $(this).toggleClass('setclose');
    });

    // ====== Load Stock Button ======
    $('#btnLoadStock').on('click', function () {
        var branchId = $('#ddlBranch').val();
        if (!branchId) {
            Swal.fire({ icon: 'warning', title: 'กรุณาเลือกสาขา', confirmButtonText: 'ตกลง' });
            return;
        }
        loadStockData(parseInt(branchId));
    });

    // ====== Item Type Filter (client-side re-filter) ======
    $('#ddlItemType').on('change', function () {
        applyClientFilter();
    });

    // ====== Text Search Filter ======
    $('#txtItemSearch').on('keyup', function () {
        if (tableEntry) {
            tableEntry.search($(this).val()).draw();
        }
    });

    // ====== Load Data ======
    function loadStockData(branchId) {
        ShowLoading();
        $('#emptyPlaceholder').hide();
        $('#stockTableWrapper').hide();
        $('#stockTableActions').hide();
        $('#alertZeroQty').addClass('d-none');

        $.ajax({
            url: '/Stock/GetStockDataByBranch',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ branchid: branchId }),
            success: function (res) {
                HideLoading();
                if (!res.result) {
                    ShowMessageError(res.message || 'ไม่สามารถโหลดข้อมูลได้');
                    $('#emptyPlaceholder').show();
                    return;
                }
                loadedData = res.data || [];
                applyClientFilter();
            },
            error: function () {
                HideLoading();
                ShowMessageError('เกิดข้อผิดพลาดในการโหลดข้อมูล');
                $('#emptyPlaceholder').show();
            }
        });
    }

    function applyClientFilter() {
        var itemTypeFilter = $('#ddlItemType').val() || '';
        var filtered = loadedData;

        if (itemTypeFilter) {
            filtered = loadedData.filter(function (d) {
                return d.itemtypecode && d.itemtypecode.toLowerCase().indexOf(itemTypeFilter.toLowerCase()) >= 0;
            });
        }

        if (!filtered || filtered.length === 0) {
            if (tableEntry) { tableEntry.destroy(); tableEntry = null; }
            $('#stockTableWrapper').hide();
            $('#stockTableActions').hide();
            $('#emptyPlaceholder').text('ไม่พบรายการสินค้าสำหรับตัวกรองที่เลือก').show();
            return;
        }

        renderTable(filtered);
    }

    // ====== Render Table ======
    function renderTable(data) {
        if (tableEntry) {
            tableEntry.destroy();
            tableEntry = null;
        }
        $('#tbNewCountStockBody').empty();

        var rows = '';
        $.each(data, function (i, d) {
            var cyQty = d.qtyinbranchofstockday || 0;
            rows += '<tr>'
                + '<td hidden>' + (d.itemid || 0) + '</td>'
                + '<td hidden>' + (d.branchid || 0) + '</td>'
                + '<td hidden>' + (d.subitemtypeid || 0) + '</td>'
                + '<td hidden>' + escHtml(d.itemtypecode || '') + '</td>'
                + '<td>' + escHtml(d.subitemcode || '') + '</td>'
                + '<td style="text-align:right;font-weight:600" class="cy-stock">' + cyQty + '</td>'
                // Editable input columns
                + '<td><input type="number" class="form-control form-control-sm inp-counted text-right" value="0" min="0" style="width:80px;text-align:right"></td>'
                + '<td><input type="number" class="form-control form-control-sm inp-restock text-right" value="0" min="0" style="width:80px;text-align:right"></td>'
                + '<td><input type="number" class="form-control form-control-sm inp-damaged text-right" value="0" min="0" style="width:80px;text-align:right"></td>'
                + '<td><input type="number" class="form-control form-control-sm inp-sold text-right" value="0" min="0" style="width:80px;text-align:right"></td>'
                + '<td style="text-align:right;font-weight:600" class="td-total">0</td>'
                + '<td style="text-align:right;font-weight:600" class="td-diff">0</td>'
                + '<td><input type="text" class="form-control form-control-sm inp-remark" placeholder="หมายเหตุ (ถ้านับ 0 กรุณาระบุ)" maxlength="200" style="width:180px"></td>'
                + '</tr>';
        });
        $('#tbNewCountStockBody').html(rows);

        tableEntry = $('#tbNewCountStock').DataTable({
            destroy: true,
            paging: true,
            pageLength: 15,
            ordering: true,
            stateSave: false,
            sDom: 'frtlpi',
            columnDefs: [{ targets: [0, 1, 2, 3], visible: false }],
            language: {
                search: ' ',
                sLengthMenu: '_MENU_',
                searchPlaceholder: 'ค้นหาประเภทย่อย...',
                info: '_START_ - _END_ of _TOTAL_ รายการ',
                emptyTable: 'ไม่พบข้อมูล',
                infoEmpty: 'ไม่พบรายการ',
                paginate: { next: 'ถัดไป', previous: 'ก่อนหน้า' }
            }
        });

        // Append DataTable search into existing search-input
        $('.dataTables_filter').appendTo('.search-input');

        $('#stockTableWrapper').show();
        $('#stockTableActions').show();
        $('#emptyPlaceholder').hide();

        // Bind input events for live calc (use event delegation on tbody)
        $('#tbNewCountStock tbody').off('input', 'input[type="number"]')
            .on('input', 'input[type="number"]', function () {
                recalcRow($(this).closest('tr'));
                checkZeroAlert();
            });
    }

    // ====== Recalculate row ======
    function recalcRow(row) {
        var cy = parseInt(row.find('.cy-stock').text()) || 0;
        var counted = parseInt(row.find('.inp-counted').val()) || 0;
        var restock = parseInt(row.find('.inp-restock').val()) || 0;
        var damaged = parseInt(row.find('.inp-damaged').val()) || 0;
        var sold = parseInt(row.find('.inp-sold').val()) || 0;

        // รวมนับได้ = CY + counted + restock + damaged + sold (spec: SUM D+E+F+G+H)
        var total = cy + counted + restock + damaged + sold;
        // ขาด/เกิน = counted - CY
        var diff = counted - cy;

        row.find('.td-total').text(total);
        var tdDiff = row.find('.td-diff');
        tdDiff.text(diff);
        tdDiff.css('color', diff < 0 ? '#FF0000' : diff > 0 ? '#28a745' : '');
    }

    // ====== Zero-qty Alert ======
    function checkZeroAlert() {
        var zeroList = [];
        $('#tbNewCountStock tbody tr').each(function () {
            var counted = parseInt($(this).find('.inp-counted').val()) || 0;
            var remark = $(this).find('.inp-remark').val().trim();
            var subType = $(this).find('td:eq(4)').text().trim(); // visible col 0 = subitemcode
            if (counted === 0 && !remark && subType) {
                zeroList.push(subType);
            }
        });
        var unique = [...new Set(zeroList)];
        if (unique.length > 0) {
            $('#alertZeroQtyList').text(unique.join(', '));
            $('#alertZeroQty').removeClass('d-none');
        } else {
            $('#alertZeroQty').addClass('d-none');
        }
    }

    // ====== Collect Items ======
    function collectItems() {
        var branchId = parseInt($('#ddlBranch').val());
        if (!branchId) {
            Swal.fire({ icon: 'warning', title: 'กรุณาเลือกสาขา' });
            return null;
        }
        var remark = $('#txtGlobalRemark').val().trim();
        var items = [];

        // Iterate all rows (across all pages)
        $('#tbNewCountStock tbody tr').each(function () {
            var cells = $(this).find('td');
            var itemId      = parseInt($(cells[0]).text()) || 0;
            var branchCell  = parseInt($(cells[1]).text()) || branchId;
            var subTypeId   = parseInt($(cells[2]).text()) || 0;
            var itemType    = $(cells[3]).text().trim();
            var subCode     = $(cells[4]).text().trim();
            var cyStock     = parseInt($(cells[5]).text()) || 0;

            var counted  = parseInt($(this).find('.inp-counted').val()) || 0;
            var restock  = parseInt($(this).find('.inp-restock').val()) || 0;
            var damaged  = parseInt($(this).find('.inp-damaged').val()) || 0;
            var sold     = parseInt($(this).find('.inp-sold').val()) || 0;
            var total    = cyStock + counted + restock + damaged + sold;
            var diff     = counted - cyStock;
            var itemRmk  = $(this).find('.inp-remark').val().trim();

            items.push({
                BranchID: branchCell,
                ItemId: itemId,
                SubItemTypeID: subTypeId,
                SubItemCode: subCode,
                ItemCode: subCode,
                ItemName: subCode,
                CYStockQty: cyStock,
                CountedQty: counted,
                WaitingToRestock: restock,
                Damaged: damaged,
                SoldBeforeCount: sold,
                TotalCounted: total,
                Difference: diff,
                ItemRemark: itemRmk,
                Remark: remark,
                CounterRole: counterRole
            });
        });

        if (items.length === 0) {
            Swal.fire({ icon: 'warning', title: 'ไม่พบรายการ', text: 'กรุณาโหลดข้อมูลก่อนบันทึก' });
            return null;
        }
        return items;
    }

    // ====== Save Draft ======
    $('#btnSaveDraft').on('click', function (e) {
        e.preventDefault();
        var items = collectItems();
        if (!items) return;

        Swal.fire({
            title: 'บันทึกแบบร่าง?', icon: 'question',
            showCancelButton: true, confirmButtonText: 'บันทึก', cancelButtonText: 'ยกเลิก'
        }).then(function (r) {
            if (r.value) { postSave('/Stock/SaveDraftCountStock', items, false); }
        });
    });

    // ====== Submit ======
    $('#btnSubmitStock').on('click', function (e) {
        e.preventDefault();
        var items = collectItems();
        if (!items) return;

        var zeros = items.filter(function (i) { return i.CountedQty === 0 && !i.ItemRemark; });
        if (zeros.length > 0) {
            var names = [...new Set(zeros.map(function (i) { return i.SubItemCode; }))].join(', ');
            Swal.fire({ icon: 'warning', title: 'กรุณาระบุหมายเหตุ', text: 'รายการที่นับได้ 0: ' + names });
            return;
        }

        Swal.fire({
            title: 'ยืนยันการส่งข้อมูล?',
            text: 'ระบบจะส่งข้อมูลให้ audit ตรวจสอบ',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'ส่งข้อมูล',
            cancelButtonText: 'ยกเลิก',
            confirmButtonColor: '#28a745'
        }).then(function (r) {
            if (r.value) { postSave('/Stock/SubmitNewCountStock', items, true); }
        });
    });

    function postSave(url, items, redirect) {
        ShowLoading();
        $.ajax({
            url: url, type: 'POST', contentType: 'application/json',
            data: JSON.stringify(items),
            success: function (res) {
                HideLoading();
                if (res.result) {
                    Swal.fire({ icon: 'success', title: 'สำเร็จ', text: res.message, confirmButtonText: 'ตกลง' })
                        .then(function () { if (redirect) window.location.href = '/Stock/Index'; });
                } else {
                    Swal.fire({ icon: 'error', title: 'ไม่สำเร็จ', text: res.message });
                }
            },
            error: function () { HideLoading(); ShowMessageError('เกิดข้อผิดพลาด'); }
        });
    }

    function escHtml(str) {
        return $('<div>').text(str).html();
    }
});
