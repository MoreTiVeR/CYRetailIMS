
var datatable;
$('.select2').select2();

//tbSaleReport
datatable = $("#tbReceipts").DataTable({
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
        "url": "/Receipt/SearchReceipt",
        "type": "POST",
        "contentType": "application/json", // Add this line
        "data": function (data) {
            data.startdate = $("#txtStartDate").val();
            data.enddate = $("#txtEndDate").val();

            var selectedBranch = $('.ddl-branch').val();
            var branchid = isNaN(parseInt(selectedBranch, 10)) ? 0 : parseInt(selectedBranch, 10); // Parse and if NaN, set to -1

            data.branchid = branchid;
            data.draw = data.draw;
            data.start = data.start;
            data.length = data.length;
            data.searchValue = data.search.value;
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
        //{
        //    "data": { createddate: "transactiondate" },
        //    "render": function (data) {
        //        if (data.transactiondate === null || data.transactiondate == null) {
        //            return data.transactiondate;
        //        }
        //        return formatDate(new Date(data.transactiondate));
        //    }
        //},
        { "data": "receivetempid" },
        { "data": "branchname" },
        { "data": "printername" },
        { "data": "shopheadernametext" },
        { "data": "shopfootertext" },
        { "data": "shopheaderaddresstext" },
        { "data": "createdby" },
        {
            "data": { createddate: "createddate" },
            "render": function (data) {
                if (data.createddate === null) {
                    return data.createddate;
                }
                return formatDateTime(new Date(data.createddate));
            }
        },
        { "data": "updatedby" },
        {
            "data": { updateddate: "updateddate" },
            "render": function (data) {
                if (data.updateddate === null) {
                    return data.updateddate;
                }
                return formatDateTime(new Date(data.updateddate));
            }
        },
        {
            "data": { isactive: "isactive" },
            "render": function (data) {
                if (data.isactive) {
                    return "<span class='badges bg-lightgreen'>ใช้งาน</span>";
                }
                else {
                    return "<span class='badges bg-lightyellow'>ยกเลิก</span>";
                }
            }
        },
        {
            "data": { receivetempid: "receivetempid", branchid: "branchid" },
            "render": function (data) {
                var dict = {
                    "receivetempid": data.receivetempid,
                };
                return "<a href='Edit?receivetempid=" + data.receivetempid + "'  class='me-3' title='แก้ไขข้อมูลใบเสร็จ'><img src='../assets/img/icons/edit.svg' alt='img'></a><a id='rowid" + data.receivetempid + "' title='ลบข้อมูลใบเสร็จ' onclick=deleteReceipt(" + data.receivetempid + ") class='me-3'><img src='../assets/img/icons/delete.svg' alt='img'></a>";
            }
        },
    ],
    "order": [[0, "desc"]],
    "columnDefs": [
        {
            "targets": 1, // index of receivetempid column
            //"className": "text-center"
        },
        {
            "targets": [0],
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
    initComplete: (settings, json) => {
        $('.dataTables_filter').appendTo("#tbReceipts");
        $('.dataTables_filter').appendTo('.search-input');
    },
    /*dom: 'Bfrtip',*/
    buttons: [
        {
            extend: 'excelHtml5',
            title: 'ข้อมูลใบเสร็จสาขา',
            text: 'ดาวโหลดรายงานหน้าปัจจุบัน',
            class: 'btn-primary',
            //Columns to export
            exportOptions: {
                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
            }
        },
        {
            extend: 'excelHtml5',
            title: 'ข้อมูลใบเสร็จสาขาทั้งหมด',
            text: 'ดาวโหลดรายงานทั้งหมด',
            class: 'btn-primary',
            exportOptions: {
                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9],
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
                    url: "/Receipt/SearchReceipt", // Create a new endpoint for all data
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({
                        startdate: $("#txtStartDate").val(),
                        enddate: $("#txtEndDate").val(),
                        branchid: $('.ddl-branch').val() || 0,
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

function deleteReceipt(receiptid) {

    Swal.fire({
        title: "ยืนยันการลบข้อมูล?",
        //text: "เมื่อลบข้อมูลแล้ว จะไม่สามารถทำการยกเลิกได้!",
        html: "<span class='text-danger'>เมื่อลบข้อมูลแล้ว จะไม่สามารถทำการยกเลิกได้!</span>",
        icon: 'warning',
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "ยืนยัน",
        confirmButtonClass: "btn btn-primary",
        cancelButtonText: "ยกเลิก",
        cancelButtonClass: "btn btn-danger ml-1",
        buttonsStyling: false,
    }).then(function (t) {
        if (t.value) {

            //Delete
            $.ajax({
                type: 'POST',
                url: '/Receipt/DeleteReceiptByID',
                data: JSON.stringify({ receipttempid: receiptid }),
                contentType: 'application/json',
                success: function (data) {
                    if (data.result) {

                        AlertSuccess('ลบข้อมูลสำเร็จ');
                        HideLoading();

                        //Reload data
                        $('#tbReceipts').DataTable().ajax.reload();
                    }
                    else {
                        //ShowMessageError(data.message);
                        AlertError(data.message);
                        HideLoading();
                    }
                }
            });
        }
    });
}