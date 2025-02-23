
InitialCharacterRemaining();
$('.select2').select2();

// Initialize DataTable
let table = $('#countStockTable').DataTable({
    "destroy": true,
    "bFilter": true,
    //"sDom": 'Btlpi',
    "sDom": 'tlpi',
    //"sDom": 'fBtlpi',
    "pagingType": 'numbers',
    "ordering": true,
    "pageLength": 10,
    "autoWidth": false,
    "stateSave": true,
    columns: [
        { data: "itemtypecode" },
        { data: "subitemtypeid" },
        { data: "subitemcode" },
        { data: "itemid" },
        { data: "branchid" },
        { data: "qtyinbranchofstockday" },
        { data: "storestock" },
        { data: "countedqty" },
        { data: "waitingtorestock" },
        { data: "damaged" },
        { data: "soldbeforecount" },
        { data: "totalcounted" },
        { data: "difference" }
    ],
    "columnDefs": [
        {
            "targets": [0, 1, 3, 4, 5],
            "visible": false
        }
    ],
    "language": {
        search: ' ',
        sLengthMenu: '_MENU_',
        searchPlaceholder: "ค้นหาข้อมูล...",
        info: "_START_ - _END_ of _TOTAL_ items",
        emptyTable: "ไม่พบข้อมูล.",
        processing: '<div class="spinner"></div><div class="processing-text">Processing your request...</div>'
    },
    buttons: [
        {
            extend: 'excelHtml5',
            title: 'รายงานนับสต๊อก',
            text: 'ดาวโหลดไฟล์ Excel',
            class: 'btn-primary',
            exportOptions: {
                columns: [2, 5, 6, 7, 8, 9, 10, 11, 12]
            }
        },
        {
            extend: 'pdfHtml5',
            title: 'PDF',
            text: 'Export to PDF'
        }
    ]
});

InitialCountStockDataTable();

$("#btnSaveCountStock").on('click', function (event) {
    ShowLoading();
    event.preventDefault(); // Prevent the default form submission

    let updatedItems = [];

    var txtRemark = $("#dynamicTextarea").val();
    //console.log(txtRemark);

    // Loop through each row to collect data
    table.rows().every(function (rowIdx, tableLoop, rowLoop) {

        // Use jQuery to find the first <td> and get its text
        var rowData = this.data(); // Get row data

        // Get the row node (DOM element)
        let rowNode = this.node();

        updatedItems.push({
            ItemTypeCode: rowData.itemtypecode,
            SubItemTypeID: rowData.subitemtypeid,
            SubItemCode: rowData.subitemcode,
            ItemId: rowData.itemid,
            BranchID: rowData.branchid,
            QtyInBranchOfStockDay: rowData.qtyinbranchofstockday,
            StoreStock: $(rowNode).find('td:eq(1)').text(),
            CountedQty: $(rowNode).find('td:eq(2)').text(),
            WaitingToRestock: $(rowNode).find('td:eq(3)').text(),
            Damaged: $(rowNode).find('td:eq(4)').text(),
            SoldBeforeCount: $(rowNode).find('td:eq(5)').text(),
            TotalCounted: $(rowNode).find('td:eq(6)').text(),
            Difference: $(rowNode).find('td:eq(7)').text(),
            Remark: txtRemark
        });
        
    });

    // Send data to the server via AJAX
    $.ajax({
        url: '/Stock/UpdateCountStock',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(updatedItems),
        success: function (response) {
            if (response.result) {
                ShowMessageSuccess(response.message);

                setTimeout(function () {
                    window.location.href = "/Stock/Index";
                }, 1000);
            }
            else {
                ShowMessageError(response.message);
            }
            HideLoading();
        },
        error: function (xhr, status, message) {
            ShowMessageError('ขออภัย, พบข้อผิดพลาด! กรุณาทำรายการใหม่อีกครั้ง');
            HideLoading();
        }
    });
});

// Handle dropdown itemtype selection change
$('#ddlItemType').on('change', function () {
    let selectedValue = $(this).val(); // Get the selected value from the dropdown
    console.log('ddlItemType:' + selectedValue);

    // Apply search filter to the DataTable
    table.column(0) // Assuming the first column (index 0) corresponds to the branch/type
        .search(selectedValue)
        .draw(); // Redraw the table with the filtered data
});

// Handle dropdown branch selection change
$('#ddlBranch').on('change', function () {
    let selectedValue = $(this).val(); // Get the selected value from the dropdown

    // Destroy the existing DataTable if it's already initialized
    //if ($.fn.DataTable.isDataTable('#countStockTable')) {
    //    $('#countStockTable').DataTable().destroy();
    //}

    // Initialize and bind the DataTable with the new data
    var reqdata = { "branchid": selectedValue };
    var jsonData = JSON.stringify(reqdata);

    var request = $.ajax({
        type: 'POST',
        url: '/Stock/GetStockDataByBranch',
        data: jsonData,
        contentType: 'application/json',
        success: function (response) {

            if (response.result) {

                // Clear the current DataTable data and add the new data
                table.clear().rows.add(response.data).draw();
            }
            else {
                AlertErrorNoTitle(response.message);
                table.clear().rows.add(response.data).draw();
            }
            HideLoading();
        },
        failure: function (response) {
            AlertError(response.message);
        },
        error: function (response) {
            AlertError(response.message);
        },
        done: function (response) {
            AlertError(response.message);
        }
    });

    // Add contenteditable and classes after DataTable initialization
    // Make the second column editable
    table.on('draw', function () {
        //สต๊อกหน้าร้าน
        $('#countStockTable tbody td:nth-child(2)').attr('contenteditable', 'true').addClass('editable number-only storestock');

        //ยอดนับได้
        $('#countStockTable tbody td:nth-child(3)').attr('contenteditable', 'true').addClass('editable number-only countedqty');

        //รอเติม
        $('#countStockTable tbody td:nth-child(4)').attr('contenteditable', 'true').addClass('editable number-only waitingtorestock');

        //ชำรุด
        $('#countStockTable tbody td:nth-child(5)').attr('contenteditable', 'true').addClass('editable number-only damaged');

        //ขายก่อนนับ
        $('#countStockTable tbody td:nth-child(6)').attr('contenteditable', 'true').addClass('editable number-only soldbeforecount');

        //รวมนับได้
        $('#countStockTable tbody td:nth-child(7)').attr('contenteditable', 'false').addClass('editable number-only totalcounted');

        //ขาดเกิน
        $('#countStockTable tbody td:nth-child(8)').attr('contenteditable', 'false').addClass('editable number-only difference');
    });
});

$("#btnCancel").on('click', function(e){
    e.preventDefault();
    window.location = "/Stock/Index";
    //setTimeout(function () {
    //    window.location.href = "/Inventory/Index";
    //}, 1000);
});

// Restrict input to numbers only
document.addEventListener('sinput', function (event) {
    if (event.target.matches('.number-only')) {
        const element = event.target;
        const value = element.innerText;

        // Get the current cursor position
        const selection = window.getSelection();
        const range = selection.getRangeAt(0);
        const cursorPosition = range.startOffset;

        // Replace any non-numeric characters
        const newValue = value.replace(/[^0-9]/g, '');

        // Update the content only if it has changed
        if (value !== newValue) {
            element.innerText = newValue;

            // Reset the cursor position to where it was before
            const newRange = document.createRange();
            newRange.setStart(element.childNodes[0], Math.min(cursorPosition, newValue.length));
            newRange.collapse(true);

            selection.removeAllRanges();
            selection.addRange(newRange);
        }
    }
});

// Prevent invalid characters from being entered
document.addEventListener('keypress', function (event) {
    //alert('keypress');
    if (event.target.matches('.number-only')) {
        const char = String.fromCharCode(event.which);
        if (!/[0-9]/.test(char)) {
            event.preventDefault();
        }
    }
});

// Event listener for inputs
$('#countStockTable').on('input', '.countedqty, .waitingtorestock, .damaged, .soldbeforecount, .storestock', function () {
    // Find the row of the input
    let row = $(this).closest('tr');
    // Recalculate the values for the row
    recalculateRow(row);
});

// Function to recalculate totalcounted and difference
function recalculateRow(row) {
    let countedqty = parseInt($(row).find('.countedqty').text()) || 0;
    let waitingtorestock = parseInt($(row).find('.waitingtorestock').text()) || 0;
    let damaged = parseInt($(row).find('.damaged').text()) || 0;
    let soldbeforecount = parseInt($(row).find('.soldbeforecount').text()) || 0;
    let storestock = parseInt($(row).find('.storestock').text()) || 0;

    // Calculate totalcounted
    let totalcounted = countedqty + waitingtorestock + damaged + soldbeforecount;
    $(row).find('.totalcounted').text(totalcounted);

    // Calculate difference
    let difference = totalcounted - storestock;
    $(row).find('.difference').text(difference);
}

function InitialCountStockDataTable() {

    // Get the value of countstockid from Razor
    //var countStockId = "@countstockid";
    var countStockId = document.getElementById("countStockIdHidden").value;

    console.log(countStockId);

    var reqdata = { "countstockid": countStockId };
    var jsonData = JSON.stringify(reqdata);

    var request = $.ajax({
        type: 'POST',
        url: '/Stock/GetStockDataByCountStockID',
        data: jsonData,
        contentType: 'application/json',
        success: function (response) {

            if (response.result) {

                // Clear the current DataTable data and add the new data
                table.clear().rows.add(response.data).draw();

                
            }
            else {
                AlertErrorNoTitle(response.message);
                table.clear().rows.add(response.data).draw();
            }
            HideLoading();
        },
        failure: function (response) {
            AlertError(response.message);
        },
        error: function (response) {
            AlertError(response.message);
        },
        done: function (response) {
            AlertError(response.message);
        }
    });

    // Add contenteditable and classes after DataTable initialization
    // Make the second column editable
    table.on('draw', function () {
        //สต๊อกหน้าร้าน
        $('#countStockTable tbody td:nth-child(2)').attr('contenteditable', 'true').addClass('editable number-only storestock');

        //ยอดนับได้
        $('#countStockTable tbody td:nth-child(3)').attr('contenteditable', 'true').addClass('editable number-only countedqty');

        //รอเติม
        $('#countStockTable tbody td:nth-child(4)').attr('contenteditable', 'true').addClass('editable number-only waitingtorestock');

        //ชำรุด
        $('#countStockTable tbody td:nth-child(5)').attr('contenteditable', 'true').addClass('editable number-only damaged');

        //ขายก่อนนับ
        $('#countStockTable tbody td:nth-child(6)').attr('contenteditable', 'true').addClass('editable number-only soldbeforecount');

        //รวมนับได้
        $('#countStockTable tbody td:nth-child(7)').attr('contenteditable', 'false').addClass('editable number-only totalcounted');

        //ขาดเกิน
        $('#countStockTable tbody td:nth-child(8)').attr('contenteditable', 'false').addClass('editable number-only difference');
    });
}