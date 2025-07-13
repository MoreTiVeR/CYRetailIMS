
var datatable;
$('.select2').select2();

// Custom search logic
//$('#customSearch').on('keyup', function () {
//    // Get the search value
//    var searchValue = $(this).val();
//    alert('customSearch: ' + searchValue);

//    // Use DataTables API to filter the table
//    datatable.search(searchValue).draw();
//});

datatable = $("#tbItemStock").DataTable({
    "processing": true,         // Show processing indicator
    "serverSide": true,        // Enable server-side processing
    "destroy": true,
    "bFilter": true,
    stateSave: true,
    //"sDom": '<"top"B>fr<"bottom"ilp><"clear">',
    "sDom": '<"top"fB>rt<"bottom"lpi><"clear">',
    "pagingType": 'numbers',
    "ordering": true,
    "ajax": {
        "url": "/Report/SearchItemStockReport",
        "type": "POST",
        "contentType": "application/json", // Add this line
        "data": function (data) {
            //data.startdate = $("#txtStartDate").val();
            //data.enddate = $("#txtEndDate").val();

            var selectedBranch = $('.ddl-branch').val();
            var branchid = isNaN(parseInt(selectedBranch, 10)) ? 1 : parseInt(selectedBranch, 10); // Parse and if NaN, set to -1

            data.branchid = branchid;
            data.draw = data.draw;
            data.start = data.start;
            data.length = data.length;
            data.searchValue = data.search.value;
            data.order = data.order;
            data.columns = data.columns;
            //console.log(JSON.stringify(data.order, null, 2));
            //console.log(JSON.stringify(data.columns, null, 2));
            return JSON.stringify(data);
        }
    },
    "columns": [
        {
            "render": function () {
                console.log('render columns : checkbox');
                return "<label class='checkboxs'><input type='checkbox' id='select-all'><span class='checkmarks'></span></label>";
            }
        },
        {
            "data": { branchid: "branchid" },  
            "render": function (data) {  
               return "<td class='text-center'>" + data.branchid + "</td>";  
            }
        },
        { "data": "branchname" },
        { "data": "itemcode" },
        { "data": "itemname" },
        { "data": "itemtypename" },
        { "data": "subitemtypename" },
        { "data": "brandname" },
        {
            "data": { qty: "qty" },
            "render": function (data) {
                if (parseInt(data.qty, 10) <= 0) {
                    return "<span class='badges bg-lightred'>" + data.qty + "</span>";
                }
                return "<span class='badges bg-lightgreen'>" + data.qty + "</span>";
            }
        },
        { "data": "cost" },
        { "data": "price" },
        { "data": "notifyminqty" },
        { "data": "notifymaxqty" },
        {
            "data": { refillamount: "refillamount" },
            "render": function (data) {
                return "<span class='badges bg-lightgreen'>" + data.refillamount + "</span>";
            }
        }
    ],
    "order": [[0, "desc"]],
    "columnDefs": [
        {
            "targets": [0],
            "visible": false
        },
        {
            "targets": [1],
            "className": "text-center"
        },
        {
            "targets": [13],
            "className": "text-center"
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
    initComplete: (settings, json) => {
        $('.dataTables_filter').appendTo("#tbItemStock");
        $('.dataTables_filter').appendTo('.search-input');
    },
    /*dom: 'Bfrtip',*/
    buttons: [
        {
            extend: 'excelHtml5',
            title: 'รายงานสต๊อกสินค้า',
            text: 'ดาวโหลดรายงานหน้าปัจจุบัน',
            class: 'btn-primary',
            //Columns to export
            exportOptions: {
                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13]
            }
        },
        {
            extend: 'excelHtml5',
            title: 'รายงานสต๊อกสินค้าทั้งหมด',
            text: 'ดาวโหลดรายงานทั้งหมด',
            class: 'btn-primary',
            exportOptions: {
                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13],
                modifier: {
                    page: 'all'
                },
                format: {
                    body: function (data, row, column, node) {
                        // If the column contains HTML, strip it
                        if (typeof data === 'string' && data.indexOf('<') > -1) {
                            var temp = document.createElement("div");
                            temp.innerHTML = data;
                            return temp.textContent || temp.innerText || "";
                        }
                        return data;
                    }
                }
            },
            action: function (e, dt, button, config) {

                ShowLoading();
                e.preventDefault();
                var self = this; // Store the DataTable instance

                console.log('draw: ' + dt.page.info().draw);
                console.log('start: ' + dt.page.info().start);
                console.log('length: ' + dt.page.info().length);

                var searchValue = dt.search();
                console.log('search.value: ' + searchValue);

                // Custom action to fetch all data
                $.ajax({
                    url: "/Report/SearchItemStockReport", // Create a new endpoint for all data
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({
                        //startdate: $("#txtStartDate").val(),
                        //enddate: $("#txtEndDate").val(),
                        branchid: $('.ddl-branch').val() || 1,
                        draw: dt.page.info().draw,
                        start: dt.page.info().start,
                        length: dt.page.info().length,
                        searchValue: dt.search(),
                        isexportalldata: true
                    }),
                    success: function (response) {

                        //Clear and add new data to the table
                        dt.clear().rows.add(response.data).draw();

                        //Trigger the Excel export using the DataTables API
                        $.fn.dataTable.ext.buttons.excelHtml5.action.call(self, e, dt, button, config);

                        HideLoading();
                        // Restore the original data (optional, if needed)
                        // $.ajax({
                        //     url: "/Item/GetItemTransferHistoryV2",
                        //     type: "POST",
                        //     contentType: "application/json",
                        //     data: function (d) {
                        //         // Your original data parameters
                        //     },
                        //     success: function (originalData) {
                        //         table.clear().rows.add(originalData).draw();
                        //     }
                        // });
                    },
                    error: function (xhr, status, error) {
                        console.error("Error fetching data for export:", error);
                    }
                });
            }
        },
        {
            extend: 'pdfHtml5',
            title: 'PDF',
            text: 'Export to PDF'
        }
    ]
});

$("#btnSearch").on('click', function (event) {
    ShowLoading();
    event.preventDefault(); // Prevent the default form submission
    datatable.ajax.reload(); // This will use the updated parameters automatically
    HideLoading();
});