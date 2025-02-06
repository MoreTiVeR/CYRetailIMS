
var datatable;

$('.select2').select2();

datatable = $("#tbCountStock").DataTable({
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
        "url": "/Stock/GetCountStocks", // URL to your controller method
        "type": "POST",         // Use GET or POST based on your implementation
        "contentType": "application/json", // Add this line
        "data": function (data) {
            data.startdate = $("#txtStartDate").val();
            data.enddate = $("#txtEndDate").val();

            var selectedBranch = $('.ddl-branch').val();
            var branchid = isNaN(parseInt(selectedBranch, 10)) ? 999 : parseInt(selectedBranch, 10); // Parse and if NaN, set to -1

            //var selectedTransferStatus = $('.ddl-transferstatus').val();
            //var transferstatusid = isNaN(parseInt(selectedTransferStatus, 10)) ? 999 : parseInt(selectedTransferStatus, 10); // Parse and if NaN, set to -1

            data.branchid = branchid;
            //data.transferstatusid = transferstatusid;
            data.draw = data.draw;
            data.start = data.start;
            data.length = data.length;
            data.searchValue = data.search.value;
            // Return the serialized JSON string
            return JSON.stringify(data); // Ensure data is being serialized to JSON
        }
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
        { "data": "remark" },
        {
            "data": { countstockdetailid: "countstockdetailid" },
            "render": function (data) {
                //var _countstockdetailid = parseInt(data.countstockdetailid);
                return "<a href='ReceiveItemTransfer?transferid=" + data.countstockdetailid + "' class='me-3' title='คลิก เพื่อตรวจรับสินค้า'><span class='badges bg-lightred'>" + data.countstockdetailid + "</span></a>";

            }
        }
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
            title: 'รายงานประวัติการนับสต๊อก',
            text: 'ดาวโหลดไฟล์ Excel',
            class: 'btn-primary',
            exportOptions: {
                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]
            }
        },
        {
            extend: 'pdfHtml5',
            title: 'PDF',
            text: 'Export to PDF'
        }
    ],
    initComplete: (settings, json) => {
        $('.dataTables_filter').appendTo("#tbCountStock");
        $('.dataTables_filter').appendTo('.search-input');
    },
});

$("#btnSearch").on('click', function (event) {
    ShowLoading();
    event.preventDefault(); // Prevent the default form submission
    datatable.ajax.reload(); // This will use the updated parameters automatically
    HideLoading();    
});

function deleteItem(itemid) {

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
                url: '/Stock/DeleteItem',
                data: JSON.stringify({ itemid: itemid }),
                contentType: 'application/json',
                success: function (data) {
                    if (data.result) {

                        AlertSuccess('ลบข้อมูลสำเร็จ');
                        $("#global-loader").css('display', 'none');
                        //ShowMessageSuccess(data.message);

                        //To do next?
                        //window.location = data.url;
                        //itemDataTable.row('.selected').remove().draw(false);
                        //dataTable.ajax.reload();
                        /*$("#tbItems").DataTable().ajax.reload();*/
                        /* $('#tbItems').DataTable().ajax.reload();*/
                        //$('#tbItems').DataTable().ajax.reload();

                        console.log("#rowid" + itemid);
                        //$("#rowid" + itemid).closest("tr").remove();

                        //Reload data
                        $('#tbItems').DataTable().ajax.reload();

                        //$("#rowid" + itemid).closest("tr").remove().draw(false);
                        //console.log(row);
                        //$('#tbItems').DataTable().row(row).remove().draw(false);

                        //var row = $('#dataTable').DataTable().rows('.remove-row').closest('tr');
                        //alert('test -> ' + row);
                        //var rowdata = $('#tbItems').DataTable().row(row).data();
                        //alert('data -> ' + rowdata)
                        //AlertSuccess('ลบแถวสำเร็จ');
                    }
                    else {
                        //ShowMessageError(data.message);
                        AlertError(data.message);
                        $("#global-loader").css('display', 'none');
                    }
                }
            });
        }
    });
}

function deleteItemInBranch(itemid, searchbranchid) {
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
                url: '/Stock/DeleteItemInBranch',
                data: JSON.stringify({ itemid: itemid, searchbranchid: searchbranchid }),
                contentType: 'application/json',
                success: function (data) {
                    if (data.result) {

                        AlertSuccess('ลบข้อมูลสำเร็จ');
                        $("#global-loader").css('display', 'none');
                        //ShowMessageSuccess(data.message);

                        //To do next?
                        //window.location = data.url;
                        //itemDataTable.row('.selected').remove().draw(false);
                        //dataTable.ajax.reload();
                        /*$("#tbItems").DataTable().ajax.reload();*/
                        /* $('#tbItems').DataTable().ajax.reload();*/
                        //$('#tbItems').DataTable().ajax.reload();

                        console.log("#rowid" + itemid);
                        $("#rowid" + itemid).closest("tr").remove();

                        //Reload data
                        //$('#tbItems').DataTable().ajax.reload();

                        //$("#rowid" + itemid).closest("tr").remove().draw(false);
                        //console.log(row);
                        //$('#tbItems').DataTable().row(row).remove().draw(false);

                        //var row = $('#dataTable').DataTable().rows('.remove-row').closest('tr');
                        //alert('test -> ' + row);
                        //var rowdata = $('#tbItems').DataTable().row(row).data();
                        //alert('data -> ' + rowdata)
                        //AlertSuccess('ลบแถวสำเร็จ');
                    }
                    else {
                        //ShowMessageError(data.message);
                        AlertError(data.message);
                        $("#global-loader").css('display', 'none');
                    }
                }
            });
        }
    });
}