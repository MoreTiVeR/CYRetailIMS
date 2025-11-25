
InitialDatePicker();
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