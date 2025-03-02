
var datatable;
$('.select2').select2();

//datatable = $("#tbCountStockReport").DataTable({
//    //"processing": true,         // Show processing indicator
//    "serverSide": true,        // Enable server-side processing
//    "destroy": true,
//    "bFilter": true,
//    stateSave: true,
//    //"sDom": '<"top"B>fr<"bottom"ilp><"clear">',
//    "sDom": '<"top"fB>rt<"bottom"lpi><"clear">',
//    "pagingType": 'numbers',
//    "ordering": true,
//    "ajax": {
//        "url": "/Report/GetCountStockReportV2", // URL to your controller method
//        "type": "POST",         // Use GET or POST based on your implementation
//        "contentType": "application/json", // Add this line
//        "data": function (data) {
//            data.startdate = $("#txtStartDate").val();
//            data.enddate = $("#txtEndDate").val();

//            var selectedBranch = $('.ddl-branch').val();
//            var branchid = isNaN(parseInt(selectedBranch, 10)) ? 999 : parseInt(selectedBranch, 10); // Parse and if NaN, set to -1

//            //var selectedTransferStatus = $('.ddl-transferstatus').val();
//            //var transferstatusid = isNaN(parseInt(selectedTransferStatus, 10)) ? 999 : parseInt(selectedTransferStatus, 10); // Parse and if NaN, set to -1

//            data.branchid = branchid;
//            //data.transferstatusid = transferstatusid;
//            data.draw = data.draw;
//            data.start = data.start;
//            data.length = data.length;
//            data.searchValue = data.search.value;
//            // Return the serialized JSON string
//            return JSON.stringify(data); // Ensure data is being serialized to JSON
//        }
//    },
//    "columns": [
//        {
//            "render": function () {
//                console.log('render columns : checkbox');
//                return "<label class='checkboxs'><input type='checkbox' id='select-all'><span class='checkmarks'></span></label>";
//            }
//        },
//        { "data": "countstockid" },
//        { "data": "branchid" },
//        { "data": "branchname" },
//        {
//            "data": { createddate: "createddate" },
//            "render": function (data) {
//                if (data.createddate === null || data.createddate == null) {
//                    return data.createddate;
//                }
//                return formatDateTime(new Date(data.createddate));
//            }
//        },
//        { "data": "subitemtypename" },
//        { "data": "qtyinbranchofcountstockday" },
//        { "data": "qtyinbranch" },
//        { "data": "countedamountqty" },
//        { "data": "shortagesurplussystemqty" },
//        { "data": "shortagesurplusqty" },
//        { "data": "createdby" },
//        { "data": "remark" }
//    ],
//    "order": [[2, "desc"]],
//    "columnDefs": [
//        {
//            "targets": [0, 1, 2],
//            "visible": false
//        }
//    ],
//    "language": {
//        search: ' ',
//        sLengthMenu: '_MENU_',
//        searchPlaceholder: "ค้นหาข้อมูล...",
//        info: "_START_ - _END_ of _TOTAL_ items",
//        emptyTable: "ไม่พบข้อมูล.",
//        processing: '<div class="spinner"></div><div class="processing-text">Processing your request...</div>'
//    },
//    buttons: [
//        {
//            extend: 'excelHtml5',
//            title: 'รายงานนับสต๊อก',
//            text: 'ดาวโหลดไฟล์ Excel',
//            class: 'btn-primary',
//            exportOptions: {
//                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]
//            }
//        },
//        {
//            extend: 'pdfHtml5',
//            title: 'PDF',
//            text: 'Export to PDF'
//        }
//    ],
//    initComplete: (settings, json) => {
//        $('.dataTables_filter').appendTo("#tbCountStockReport");
//        $('.dataTables_filter').appendTo('.search-input');
//    },
//});


//$("#btnSearch").on('click', function (event) {
//    ShowLoading();
//    event.preventDefault(); // Prevent the default form submission
//    datatable.ajax.reload(); // This will use the updated parameters automatically
//    HideLoading();
//});

datatable = $("#tbCountStockReport").DataTable({
    "destroy": true,
    "bFilter": true,
    "sDom": 'fBtlpi',
    'pagingType': 'numbers',
    "ordering": true,
    "ajax": {
        "url": "/Report/GetCountStockReportV1",
        "type": "GET",
        "datatype": "json"
    },
    "columns": [
        {
            "render": function () {
                console.log('render columns : checkbox');
                return "<label class='checkboxs'><input type='checkbox' id='select-all'><span class='checkmarks'></span></label>";
            }
        },
        { "data": "countstockid" },
        { "data": "branchid" },
        { "data": "branchname" },
        {
            "data": { createddate: "createddate" },
            "render": function (data) {
                if (data.createddate === null || data.createddate == null) {
                    return data.createddate;
                }
                return formatDateTime(new Date(data.createddate));
            }
        },
        { "data": "subitemtypename" },
        { "data": "qtyinbranchofcountstockday" },
        { "data": "qtyinbranch" },
        { "data": "countedamountqty" },
        { "data": "shortagesurplussystemqty" },
        { "data": "shortagesurplusqty" },
        { "data": "createdby" },
        { "data": "remark" }
    ],
    "order": [[2, "desc"]],
    "columnDefs": [
        {
            "targets": [0, 1, 2],
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
                columns: [3, 4, 5, 6, 7, 8, 9, 10, 11, 12]
            },
            customize: function (xlsx) {
                // Access the Excel file data
                var sheet = xlsx.xl.worksheets['sheet1.xml'];
                console.log(sheet); // Log the raw XML to inspect its structure

                // Parse the XML to modify it
                var parser = new DOMParser();
                var xmlDoc = parser.parseFromString(sheet, 'application/xml');
                // Check if parsing was successful
                if (!xmlDoc) {
                    console.error('Failed to parse XML');
                    return;
                }
                console.log(xmlDoc);

                // Find the <sheetData> element
                var sheetData = xmlDoc.getElementsByTagName('sheetData')[0];
                if (!sheetData) {
                    console.error('<sheetData> element not found in XML');
                    return;
                }

                // Get the selected branch value (replace this with your actual branch selection logic)
                var branchSelected = "NATTAPONG";

                // Create a new custom row with the branch information
                var newRow = xmlDoc.createElement('row');
                newRow.setAttribute('r', '1'); // Row index
                var newCell = xmlDoc.createElement('c');
                newCell.setAttribute('t', 'inlineStr');
                newCell.setAttribute('r', 'A1'); // Cell reference
                var is = xmlDoc.createElement('is');
                var t = xmlDoc.createElement('t');
                t.textContent = 'Branch Selected: ' + branchSelected;
                is.appendChild(t);
                newCell.appendChild(is);
                newRow.appendChild(newCell);

                // Insert the new row at the top of the sheet
                var sheetData = xmlDoc.getElementsByTagName('sheetData')[0];
                sheetData.insertBefore(newRow, sheetData.firstChild);

                // Serialize the modified XML back to a string
                var serializer = new XMLSerializer();
                xlsx.xl.worksheets['sheet1.xml'] = serializer.serializeToString(xmlDoc);
            }
        },
        {
            extend: 'pdfHtml5',
            title: 'PDF',
            text: 'Export to PDF'
        }
    ],
    initComplete: (settings, json) => {
        $('.dataTables_filter').appendTo("#tbCountStockReport");
        $('.dataTables_filter').appendTo('.search-input');
    },
});

$("#btnSearch").on('click', function (event) {
    ShowLoading();

    event.preventDefault(); // Prevent the default form submission

    var startdate = $("#txtStartDate").val();
    var enddate = $("#txtEndDate").val();
    var val = $("#ddlBranch").val();
    var branchid = parseInt(val);

    var reqdata = { "startdate": startdate, "enddate": enddate, "branchid": branchid };
    var jsonreqdata = JSON.stringify(reqdata);
    console.log(jsonreqdata);
    var request = $.ajax({
        type: 'POST',
        url: '/Report/SearchCountStockReport',
        data: jsonreqdata,
        contentType: 'application/json',
        success: function (response) {

            if (response.result) {
                ShowMessageSuccess(response.message);
            }
            else {
                AlertErrorNoTitle(response.message);
            }

            console.log(response.data);
            $("#tbCountStockReport").DataTable().clear().rows.add(response.data).draw();
            HideLoading();
        },
        failure: function (response) {
            AlertError(response.message);
        },
        error: function (response) {
            AlertError(response.message);
        }
    });
});