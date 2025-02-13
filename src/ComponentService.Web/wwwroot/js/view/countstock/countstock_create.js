
var datatable;

$('.select2').select2();

// Initialize DataTable
//let table = $('#countStockTable').DataTable({
//    paging: true,
//    searching: true,
//    ordering: true,
//    info: true,
//    autoWidth: false,
//});
let table = $('#countStockTable').DataTable({
    "destroy": true,
    "bFilter": true,
    "sDom": 'Btlpi',
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

$("#btnSaveCountStock").on('click', function (event) {
    ShowLoading();
    event.preventDefault(); // Prevent the default form submission

    let updatedItems = [];

    // Loop through each row to collect data
    table.rows().every(function (rowIdx, tableLoop, rowLoop) {

        // Use jQuery to find the first <td> and get its text
        //let firstCellText = $(rowNode).find('td:eq(0)').text();
        var rowData = this.data(); // Get row data
        //updatedItems.push(rowData);

        //// Get the row node (DOM element)
        let rowNode = this.node();
        //var StoreStock = $(rowNode).find('td:eq(1)').text();
        //var CountedQty = $(rowNode).find('td:eq(2)').text(),
        //var WaitingToRestock = $(rowNode).find('td:eq(3)').text(),
        //var Damaged = $(rowNode).find('td:eq(4)').text(),
        //var SoldBeforeCount = $(rowNode).find('td:eq(5)').text(),
        //var TotalCounted = $(rowNode).find('td:eq(6)').text(),
        //var Difference = $(rowNode).find('td:eq(7)').text()
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
            Difference: $(rowNode).find('td:eq(7)').text()
        });
        
    });

    //console.log(JSON.stringify(updatedItems));

    // Send data to the server via AJAX
    $.ajax({
        url: '/Stock/SaveV2',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(updatedItems),
        success: function (response) {
            if (response.result) {
                ShowMessageSuccess(response.message);
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

    // Apply search filter to the DataTable
    table.column(0) // Assuming the first column (index 0) corresponds to the branch/type
        .search(selectedValue)
        .draw(); // Redraw the table with the filtered data
});

// Handle dropdown branch selection change
$('#ddlBranch').on('change', function () {
    let selectedValue = $(this).val(); // Get the selected value from the dropdown

    // Apply search data by branchid
    // Update the DataTable and redraw it
    //table.ajax.url('/Stock/GetStockData?branchId=' + selectedValue).load();

    // Destroy the existing DataTable if it's already initialized
    //if ($.fn.DataTable.isDataTable('#countStockTable')) {
    //    $('#countStockTable').DataTable().destroy();
    //}

    // Initialize and bind the DataTable with the new data
    var reqdata = { "branchid": selectedValue };
    var jsonData = JSON.stringify(reqdata);
    console.log(jsonData);
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
        $('#countStockTable tbody td:nth-child(2)').attr('contenteditable', 'true').addClass('editable number-only');
        $('#countStockTable tbody td:nth-child(3)').attr('contenteditable', 'true').addClass('editable number-only');
        $('#countStockTable tbody td:nth-child(4)').attr('contenteditable', 'true').addClass('editable number-only');
        $('#countStockTable tbody td:nth-child(5)').attr('contenteditable', 'true').addClass('editable number-only');
        $('#countStockTable tbody td:nth-child(6)').attr('contenteditable', 'true').addClass('editable number-only');
        $('#countStockTable tbody td:nth-child(7)').attr('contenteditable', 'true').addClass('editable number-only');
        $('#countStockTable tbody td:nth-child(8)').attr('contenteditable', 'true').addClass('editable number-only');
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
document.addEventListener('input', function (event) {
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
    if (event.target.matches('.number-only')) {
        const char = String.fromCharCode(event.which);
        if (!/[0-9]/.test(char)) {
            event.preventDefault();
        }
    }
});