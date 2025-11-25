InitialDatePickerSetDateWhenNoInput();
InitialNumberInput();
InitialCharacterRemaining();

$(document).on('change', '.select2', function (e) {
    var selectedValue = $(this).val();
    var row = $(this).data('row');
    console.log($(this).data('name'));
    console.log("Row " + row + ": " + selectedValue);
});

function fetchTransactionSummary(dateVal) {
    if (!dateVal) return;
    ShowLoading();
    $.ajax({
        url: '/EndOfDaySummary/GetTransactionSummary',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ date: dateVal }),
        success: function (resp) {
            if (resp && resp.result) {
                var d = resp.data || {};
                console.log(resp.data);
                if (typeof d.totalcash !== 'undefined') {
                    $('#TotalCash').val(d.totalcash).trigger('change');
                }
                if (typeof d.totaltransfer !== 'undefined') {
                    $('#TotalTransfer').val(d.totaltransfer).trigger('change');
                }
                if (typeof d.totalamount !== 'undefined') {
                    $('#GrandTotal').val(d.totalamount).trigger('change');
                }
                if (typeof d.totaldepositfee !== 'undefined' && $('#Fee').length) {
                    $('#Fee').val(d.totaldepositfee).trigger('change');
                }

                ShowMessageInfo('โหลดข้อมูลสำเร็จ');
            }
            else {
                ShowMessageError(resp?.message || 'ไม่พบข้อมูลยอดขาย');
                SetEmptyValue();
            }
            HideLoading();
        },
        error: function (xhr) {
            HideLoading();
            ShowMessageError('ไม่สามารถโหลดข้อมูลยอดขายได้');
        }
    });
}


// fallback: if blur isn't fired (some browsers/plugins), also handle change with a forced blur first
$(document).on('change', '#txtSaleDate', function () {
    var dateVal = $(this).val();
    if (!dateVal) return;

    setTimeout(function () { fetchTransactionSummary(dateVal); }, 250);
});


function SetEmptyValue() {
    $('#TotalCash').val(null).trigger('change');
    $('#TotalTransfer').val(null).trigger('change');
    $('#GrandTotal').val(null).trigger('change');
    $('#Fee').val(null).trigger('change');
    $('#DepositedCash').val(null).trigger('change');
    $('#CustomerTransfer').val(null).trigger('change');
    $('#FinalTotal').val(null).trigger('change');
}

// Calculate FinalTotal when any related input changes
function calculateFinalTotal() {
    function valOrZero(selector) {
        var el = $(selector);
        if (!el.length) return 0;
        var v = el.val();
        if (v === undefined || v === null || v === '') return 0;
        var n = parseFloat(v);
        if (isNaN(n)) return 0;
        // prevent negative via JS enforcement
        if (n < 0) {
            n = 0;
            el.val(n);
        }
        return n;
    }

    var deposited = valOrZero('#DepositedCash');
    var customerTransfer = valOrZero('#CustomerTransfer');
    var substitute = valOrZero('#SubstituteWage');
    var fee = valOrZero('#Fee');
    var other = valOrZero('#OtherExpense');

    var total = deposited + customerTransfer + substitute + fee + other;

    // keep consistent formatting: show two decimals
    var display = total.toFixed(2);

    var finalEl = $('#FinalTotal');
    finalEl.val(display);

    // ensure FinalTotal not negative and has min attribute
    if (parseFloat(finalEl.val()) < 0) {
        finalEl.val('0.00');
    }

    finalEl.trigger('change');
}

// Attach handlers to recalculate when inputs change or user types
$(document).on('input change', '#DepositedCash, #CustomerTransfer, #SubstituteWage, #Fee, #OtherExpense', function () {
    // if user enters negative value manually (some browsers allow), clamp to 0
    var v = $(this).val();
    if (v !== undefined && v !== null && v !== '' && parseFloat(v) < 0) {
        $(this).val(0);
    }
    calculateFinalTotal();
});

// Initial calculation in case values are preset on load
$(function () {
    // set min attribute on FinalTotal as readonly protective measure
    $('#FinalTotal').attr('min', 0);
    calculateFinalTotal();
});