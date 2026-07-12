$(document).ready(function () {

    // Initialize Select2
    $('#ddlBranch').select2({ placeholder: '-- กรุณาเลือกสาขา --' });

    // Initialize Bootstrap tooltips
    $('[data-bs-toggle="tooltip"]').tooltip();

    // Branch selection
    $('#ddlBranch').on('change', function () {
        const branchId = $(this).val();
        if (!branchId) { resetView(); return; }
        loadStockData(parseInt(branchId));
    });

    // Load per-item stock data by branch
    function loadStockData(branchId) {
        resetView();
        $('#loadingOverlay').show();
        $.ajax({
            url: '/Stock/GetStockDataByBranchV2',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ branchid: branchId }),
            success: function (response) {
                $('#loadingOverlay').hide();
                if (response.result && response.data && response.data.length > 0) {
                    renderTable(response.data);
                    $('#summarySection').show();
                    $('#tableSection').show();
                    updateSummary();
                } else {
                    $('#emptyState').show();
                    AlertErrorNoTitle(response.message || 'ไม่พบข้อมูลสินค้าในสาขานี้');
                }
            },
            error: function () {
                $('#loadingOverlay').hide();
                $('#emptyState').show();
                AlertErrorNoTitle('ขออภัย เกิดข้อผิดพลาดในการโหลดข้อมูล');
            }
        });
    }

    // Render table rows (one row per item)
    function renderTable(data) {
        const tbody = $('#countStockV2Body');
        tbody.empty();
        data.forEach(function (item, index) {
            const systemQty = item.qtyinbranch || 0;
            const subCode   = item.subitemcode  || '-';
            const subTypeId = item.subitemtypeid || 0;
            const row = '<tr data-itemid="' + item.itemid + '" data-branchid="' + item.branchid + '" data-subitemtypeid="' + subTypeId + '" data-systemqty="' + systemQty + '" data-itemcode="' + escapeHtml(item.itemcode) + '" data-itemname="' + escapeHtml(item.itemname) + '">'
                + '<td class="text-center text-muted small ps-3">' + (index + 1) + '</td>'
                + '<td><code class="text-dark">' + escapeHtml(item.itemcode) + '</code></td>'
                + '<td class="fw-semibold">' + escapeHtml(item.itemname) + '</td>'
                + '<td class="text-muted small">' + escapeHtml(subCode) + '</td>'
                + '<td class="text-center"><span class="badge bg-light text-dark border fs-6">' + systemQty + '</span></td>'
                + '<td class="text-center"><input type="number" class="form-control form-control-sm text-center physical-count" min="0" value="0" style="width:110px;margin:auto;" aria-label="จำนวนที่มีอยู่จริง" /></td>'
                + '<td class="text-center difference-cell"><span class="badge diff-badge-balanced px-3 py-2">0</span></td>'
                + '</tr>';
            tbody.append(row);
        });
        updateVisibleCount();
    }

    // Real-time difference update
    $(document).on('input', '.physical-count', function () {
        const row         = $(this).closest('tr');
        const systemQty   = parseInt(row.data('systemqty')) || 0;
        const physicalQty = parseInt($(this).val()) || 0;
        const diff        = physicalQty - systemQty;
        let cssClass = 'diff-badge-balanced', prefix = '';
        if (diff > 0)      { cssClass = 'diff-badge-surplus';  prefix = '+'; }
        else if (diff < 0) { cssClass = 'diff-badge-shortage'; }
        row.find('.difference-cell span').attr('class', 'badge ' + cssClass + ' px-3 py-2').text(prefix + diff);
        updateSummary();
    });

    // Search filter (code or name)
    $('#txtSearch').on('input', function () {
        const keyword = $(this).val().toLowerCase().trim();
        let visible = 0;
        $('#countStockV2Body tr').each(function () {
            const code = ($(this).data('itemcode') || '').toLowerCase();
            const name = ($(this).data('itemname') || '').toLowerCase();
            const show = !keyword || code.includes(keyword) || name.includes(keyword);
            $(this).toggle(show);
            if (show) visible++;
        });
        updateVisibleCount(visible);
    });

    // Reset all inputs to 0
    $('#btnResetAll').on('click', function () {
        $('.physical-count').val(0).trigger('input');
    });

    // Summary calculation
    function updateSummary() {
        let totalItems = 0, sumSystem = 0, sumCounted = 0;
        $('#countStockV2Body tr').each(function () {
            totalItems++;
            sumSystem  += parseInt($(this).data('systemqty')) || 0;
            sumCounted += parseInt($(this).find('.physical-count').val()) || 0;
        });
        const sumDiff = sumCounted - sumSystem;
        $('#sumTotalItems').text(totalItems);
        $('#sumSystemQty').text(sumSystem);
        $('#sumCountedQty').text(sumCounted);
        $('#sumDifference')
            .removeClass('text-success text-danger text-secondary')
            .text((sumDiff >= 0 ? '+' : '') + sumDiff)
            .addClass(sumDiff > 0 ? 'text-success' : sumDiff < 0 ? 'text-danger' : 'text-secondary');
        $('#diffSummaryCard')
            .removeClass('border-success border-danger border-secondary')
            .addClass(sumDiff > 0 ? 'border-success' : sumDiff < 0 ? 'border-danger' : 'border-secondary');
    }

    function updateVisibleCount(count) {
        const total   = $('#countStockV2Body tr').length;
        const visible = (count !== undefined) ? count : total;
        $('#visibleRowCount').text(visible === total ? ('แสดง ' + total + ' รายการ') : ('แสดง ' + visible + ' จาก ' + total + ' รายการ'));
    }

    // Reset view
    function resetView() {
        $('#summarySection, #tableSection, #emptyState, #loadingOverlay').hide();
        $('#countStockV2Body').empty();
        $('#txtSearch').val('');
    }

    // Save
    $('#btnSaveCountStockV2').on('click', function () {
        const branchId = parseInt($('#ddlBranch').val());
        if (!branchId) { AlertErrorNoTitle('กรุณาเลือกสาขาก่อนบันทึก'); return; }
        const items = [];
        let hasAnyInput = false;
        $('#countStockV2Body tr').each(function () {
            const physicalQty = parseInt($(this).find('.physical-count').val()) || 0;
            if (physicalQty > 0) hasAnyInput = true;
            const subTypeId = parseInt($(this).data('subitemtypeid')) || 0;
            items.push({
                ItemID:                parseInt($(this).data('itemid'))    || 0,
                SubItemTypeID:         subTypeId > 0 ? subTypeId : null,
                QtyInBranchOfStockDay: parseInt($(this).data('systemqty')) || 0,
                PhysicalCountQty:      physicalQty
            });
        });
        if (!hasAnyInput) { AlertErrorNoTitle('กรุณาระบุจำนวนที่มีอยู่จริงอย่างน้อย 1 รายการ'); return; }
        const saveModel = { BranchID: branchId, Remark: $('#txtRemark').val().trim(), Items: items };
        ShowLoading();
        $.ajax({
            url: '/Stock/CreateCountStockV2',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(saveModel),
            success: function (response) {
                HideLoading();
                if (response.result) {
                    ShowMessageSuccess(response.message);
                    setTimeout(function () { window.location.href = '/Stock/Index'; }, 1200);
                } else {
                    ShowMessageError(response.message);
                }
            },
            error: function () { HideLoading(); ShowMessageError('ขออภัย, พบข้อผิดพลาด! กรุณาทำรายการใหม่อีกครั้ง'); }
        });
    });

    // Utility: escape HTML
    function escapeHtml(text) {
        if (!text) return '';
        return String(text).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#039;');
    }

});
