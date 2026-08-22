/**
 * countstock_newentry.js
 * หน้านับสต๊อกแบบใหม่ — พนักงานขาย (PC) และ หัวหน้า PC (HeadPC) กรอกข้อมูลนับสต๊อก
 *
 * คอลัมน์ตามสเปค:
 *   A รหัสสินค้า | B ชื่อสินค้า | C ประเภทย่อย | D สต๊อกจริง CY (ดึงอัตโนมัติ)
 *   E ยอดนับได้ | F รอเติม | G ชำรุด | H ขายก่อนนับ | I รวมนับได้ | J ขาด/เกิน
 *
 * สูตรคำนวณ:
 *   I รวมนับได้  = E + F + G + H   (ผลรวมจำนวนที่นับ/แยกสถานะได้จริง ไม่รวมสต๊อกระบบ D)
 *   J ขาด/เกิน   = E - D           (ยอดนับได้ − สต๊อกจริง CY)
 *
 * หมายเหตุ: สเปคต้นฉบับเขียน I = SUM(D+E+F+G+H) แต่แถวตัวอย่างในไฟล์ไม่ตรงกับสูตรนั้น
 *           (การรวมสต๊อกระบบ D เข้าไปด้วยจะเป็นการนับซ้ำ) จึงใช้ E+F+G+H ซึ่งตรงกับ
 *           ความหมาย "รวมนับได้" และตรงกับตัวอย่าง หากต้องการรวม D ให้ปรับที่ calcRow()
 */

var tableEntry;
var loadedData = [];   // raw data from API
var counterRole = 'PC';

// ปรับค่านี้เป็น true หากยืนยันแล้วว่าต้องการให้ "รวมนับได้" รวมสต๊อกจริง CY (D) ด้วย
var INCLUDE_CY_IN_TOTAL = false;

$(document).ready(function () {

    counterRole = $('#hdnCounterRole').val() || 'PC';

    // select2
    $('.select2').select2();
    // Note: #filter_search toggle is handled globally by assets/js/script.js

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
        $('#stockBottomActions').hide();
        $('#alertZeroQty').addClass('d-none');

        $.ajax({
            url: '/Stock/GetItemStockDataByBranch',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ branchid: branchId, itemlevel: true }),
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
            $('#stockBottomActions').hide();
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
            var rowItemId = pickValue(d, ['itemid', 'itemId', 'ItemID', 'ItemId'], 0);
            var rowBranchId = pickValue(d, ['branchid', 'branchId', 'BranchID', 'BranchId'], 0);
            var rowSubItemTypeId = pickValue(d, ['subitemtypeid', 'subItemTypeId', 'SubItemTypeID', 'SubItemTypeId'], 0);
            var rowItemTypeCode = pickValue(d, ['itemtypecode', 'itemTypeCode', 'ItemTypeCode'], '');
            var rowItemCode = pickValue(d, ['itemcode', 'itemCode', 'ItemCode'], '');
            var rowItemName = pickValue(d, ['itemname', 'itemName', 'ItemName'], '');
            var rowSubItemCode = pickValue(d, ['subitemcode', 'subItemCode', 'SubItemCode', 'subitemtypename', 'subItemTypeName', 'SubItemTypeName'], '');
            var cyQty = parseInt(pickValue(d, ['qtyinbranchofstockday', 'qtyInBranchOfStockDay', 'QtyInBranchOfStockDay'], 0)) || 0;
            if (i == 1) {
                console.log(d);
            }
            rows += '<tr>'
                + '<td hidden>' + rowItemId + '</td>'
                + '<td hidden>' + rowBranchId + '</td>'
                + '<td hidden>' + rowSubItemTypeId + '</td>'
                + '<td hidden>' + escHtml(rowItemTypeCode) + '</td>'
                + '<td class="col-itemcode">' + escHtml(rowItemCode) + '</td>'
                + '<td class="col-itemname">' + escHtml(rowItemName) + '</td>'
                + '<td class="col-subtype">' + escHtml(rowSubItemCode) + '</td>'
                + '<td style="text-align:right;font-weight:600" class="cy-stock">' + cyQty + '</td>'
                // Editable input columns (E,F,G,H)
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
            sDom: 'Bfrtlpi',
            columnDefs: [{ targets: [0, 1, 2, 3], visible: false }],
            language: {
                search: ' ',
                sLengthMenu: '_MENU_',
                searchPlaceholder: 'ค้นหารหัส/ชื่อสินค้า/ประเภทย่อย...',
                info: '_START_ - _END_ of _TOTAL_ รายการ',
                emptyTable: 'ไม่พบข้อมูล',
                infoEmpty: 'ไม่พบรายการ',
                paginate: { next: 'ถัดไป', previous: 'ก่อนหน้า' }
            },
            buttons: [
                {
                    extend: 'excelHtml5',
                    title: null,
                    text: 'ดาวน์โหลด Excel',
                    className: 'buttons-excel d-none',
                    // ส่งออกตามรูปแบบมาตรฐาน: รหัสสินค้า, ชื่อสินค้า, ประเภท, ประเภทย่อย,
                    // สต๊อกจริงCY, ยอดนับได้, รอเติม, ชำรุด, ขายก่อนนับ, รวมนับได้, ขาด-เกิน
                    exportOptions: {
                        columns: [4, 5, 3, 6, 7, 8, 9, 10, 11, 12, 13],
                        format: {
                            // อ่านค่าจาก <input> (DataTables อ่าน text ของ cell ไม่ได้อ่าน value ของ input)
                            body: function (data, row, column, node) {
                                var $inp = $(node).find('input');
                                if ($inp.length) return $inp.val();
                                return data;
                            }
                        }
                    },
                    customize: function (xlsx) {
                        // เพิ่มหัวรายงาน: ชื่อสาขา + วันที่ ไว้ด้านบนสุด (ตามรูปแบบ Export Excel)
                        try {
                            var branchName = $('#ddlBranch option:selected').text() || '';
                            var today = new Date();
                            var dateStr = ('0' + today.getDate()).slice(-2) + '-'
                                + ('0' + (today.getMonth() + 1)).slice(-2) + '-'
                                + today.getFullYear();

                            var sheet = xlsx.xl.worksheets['sheet1.xml'];
                            var sheetData = sheet.getElementsByTagName('sheetData')[0];

                            function makeInfoRow(label, value) {
                                var row = sheet.createElement('row');
                                var c1 = sheet.createElement('c');
                                c1.setAttribute('t', 'inlineStr');
                                var is1 = sheet.createElement('is');
                                var t1 = sheet.createElement('t');
                                t1.textContent = label + ': ' + value;
                                is1.appendChild(t1); c1.appendChild(is1); row.appendChild(c1);
                                return row;
                            }

                            sheetData.insertBefore(makeInfoRow('วันที่', dateStr), sheetData.firstChild);
                            sheetData.insertBefore(makeInfoRow('ชื่อสาขา', branchName), sheetData.firstChild);
                        } catch (e) {
                            // ถ้าปรับแต่งหัวไม่สำเร็จ ก็ยังส่งออกข้อมูลได้ตามปกติ
                            console.warn('customize export header failed', e);
                        }
                    }
                }
            ]
        });

        // Append DataTable search into existing search-input
        $('.dataTables_filter').appendTo('.search-input');

        $('#stockTableWrapper').show();
        $('#stockTableActions').show();
        $('#stockBottomActions').show();
        $('#emptyPlaceholder').hide();

        // Bind input events for live calc (event delegation on tbody)
        $('#tbNewCountStock tbody').off('input', 'input')
            .on('input', 'input', function () {
                recalcRow($(this).closest('tr'));
                checkZeroAlert();
            });
    }

    // ====== Recalculate a single row ======
    function recalcRow(row) {
        var cy = parseInt(row.find('.cy-stock').text()) || 0;          // D
        var counted = parseInt(row.find('.inp-counted').val()) || 0;   // E
        var restock = parseInt(row.find('.inp-restock').val()) || 0;   // F
        var damaged = parseInt(row.find('.inp-damaged').val()) || 0;   // G
        var sold = parseInt(row.find('.inp-sold').val()) || 0;         // H

        var total = calcTotal(cy, counted, restock, damaged, sold);    // I
        var diff = counted - cy;                                       // J = E - D

        row.find('.td-total').text(total);
        var tdDiff = row.find('.td-diff');
        tdDiff.text(diff);
        tdDiff.css('color', diff < 0 ? '#FF0000' : diff > 0 ? '#28a745' : '');
    }

    // I รวมนับได้ = E+F+G+H (ค่าเริ่มต้น) หรือ D+E+F+G+H หาก INCLUDE_CY_IN_TOTAL = true
    function calcTotal(cy, counted, restock, damaged, sold) {
        var base = counted + restock + damaged + sold;
        return INCLUDE_CY_IN_TOTAL ? base + cy : base;
    }

    // ====== Zero-qty Alert (แจ้งเตือนตามประเภทย่อย เผื่อลืมกรอก) ======
    function checkZeroAlert() {
        var zeroSet = {};
        $('#tbNewCountStock tbody tr').each(function () {
            var counted = parseInt($(this).find('.inp-counted').val()) || 0;
            var remark = ($(this).find('.inp-remark').val() || '').trim();
            var subType = ($(this).find('.col-subtype').text() || '').trim();
            if (counted === 0 && !remark && subType) {
                zeroSet[subType] = true;
            }
        });
        var unique = Object.keys(zeroSet);
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
        var remark = ($('#txtGlobalRemark').val() || '').trim();
        var items = [];

        // Iterate all rows (across all pages) via DataTables API for reliability
        tableEntry.rows().every(function () {
            var node = this.node();
            var $row = $(node);
            var cells = $row.find('td');

            var itemId = parseInt($(cells[0]).text()) || 0;
            var branchCell = parseInt($(cells[1]).text()) || branchId;
            var subTypeId = parseInt($(cells[2]).text()) || 0;
            var itemType = ($(cells[3]).text() || '').trim();
            var itemCode = ($row.find('.col-itemcode').text() || '').trim();
            var itemName = ($row.find('.col-itemname').text() || '').trim();
            var subCode = ($row.find('.col-subtype').text() || '').trim();
            var cyStock = parseInt($row.find('.cy-stock').text()) || 0;

            var counted = parseInt($row.find('.inp-counted').val()) || 0;
            var restock = parseInt($row.find('.inp-restock').val()) || 0;
            var damaged = parseInt($row.find('.inp-damaged').val()) || 0;
            var sold = parseInt($row.find('.inp-sold').val()) || 0;
            var total = calcTotal(cyStock, counted, restock, damaged, sold);
            var diff = counted - cyStock;
            var itemRmk = ($row.find('.inp-remark').val() || '').trim();

            items.push({
                BranchID: branchCell,
                ItemId: itemId,
                ItemTypeCode: itemType,
                SubItemTypeID: subTypeId,
                SubItemCode: subCode,
                ItemCode: itemCode,
                ItemName: itemName,
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

    // ====== Export Excel ======
    $('#btnExportEntry').on('click', function (e) {
        e.preventDefault();
        if (!tableEntry) {
            Swal.fire({ icon: 'warning', title: 'ยังไม่มีข้อมูล', text: 'กรุณาโหลดข้อมูลก่อนดาวน์โหลด' });
            return;
        }
        tableEntry.button('.buttons-excel').trigger();
    });

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
            var names = uniq(zeros.map(function (i) { return i.SubItemCode; })).join(', ');
            Swal.fire({ icon: 'warning', title: 'กรุณาระบุหมายเหตุ', text: 'รายการที่นับได้ 0 (ตามประเภทย่อย): ' + names });
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

    function uniq(arr) {
        var seen = {}, out = [];
        arr.forEach(function (x) { if (x && !seen[x]) { seen[x] = true; out.push(x); } });
        return out;
    }

    function escHtml(str) {
        return $('<div>').text(str == null ? '' : str).html();
    }

    function pickValue(obj, keys, defaultValue) {
        if (!obj) return defaultValue;
        for (var i = 0; i < keys.length; i++) {
            var key = keys[i];
            if (Object.prototype.hasOwnProperty.call(obj, key) && obj[key] !== null && obj[key] !== undefined) {
                return obj[key];
            }
        }
        return defaultValue;
    }
});
