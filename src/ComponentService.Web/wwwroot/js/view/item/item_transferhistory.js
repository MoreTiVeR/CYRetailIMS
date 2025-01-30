
var datatable;

$('.select2').select2();
//InitialData();

datatable = $("#tbItemTransferHistory").DataTable({
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
        "url": "/Item/GetItemTransferHistoryV2", // URL to your controller method
        "type": "POST",         // Use GET or POST based on your implementation
        "contentType": "application/json", // Add this line
        "data": function (data) {
            data.transferstartdate = $("#txtTransferDate").val();
            data.transferenddate = $("#txtTransferEndDate").val();

            var selectedBranch = $('.ddl-branch').val();
            var branchid = isNaN(parseInt(selectedBranch, 10)) ? 999 : parseInt(selectedBranch, 10); // Parse and if NaN, set to -1

            var selectedTransferStatus = $('.ddl-transferstatus').val();
            var transferstatusid = isNaN(parseInt(selectedTransferStatus, 10)) ? 999 : parseInt(selectedTransferStatus, 10); // Parse and if NaN, set to -1

            data.branchid = branchid;
            data.transferstatusid = transferstatusid;
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
        {
            "data": { createddate: "createddate" },
            "render": function (data) {
                if (data.createddate === null || data.createddate == null) {
                    return data.createddate;
                }
                return formatDateTime(new Date(data.createddate));
            }
        },
        { "data": "sourcename" },
        { "data": "destinationname" },
        { "data": "itemname" },
        { "data": "qty" },
        { "data": "description" },
        {
            "data": { transferid: "transferid", transferstatusid: "transferstatusid", transferstatusname_th: "transferstatusname_th" },
            "render": function (data) {
                var _transferstatusid = parseInt(data.transferstatusid);
                if (_transferstatusid == 1) {
                    return "<span class='badges bg-lightgreen'>" + data.transferstatusname_th + "</span>";
                }
                else if (_transferstatusid == 2 || _transferstatusid == 99) {
                    return "<span class='badges bg-lightred'>" + data.transferstatusname_th + "</span>";
                }
                else {
                    return "<a href='ReceiveItemTransfer?transferid=" + data.transferid + "' class='me-3' title='คลิก เพื่อตรวจรับสินค้า'><span class='badges bg-lightred'>" + data.transferstatusname_th + "</span></a>";
                }
                return "<span class='badges bg-lightyellow'>N/A</span>";

            }
        },
        { "data": "receiveqty" },
        { "data": "returnqty" },
        { "data": "createdby" },
        {
            "data": { updateddate: "updateddate" },
            "render": function (data) {
                if (data.updateddate === null || data.updateddate == null) {
                    return data.updateddate;
                }
                return formatDateTime(new Date(data.updateddate));
            }
        },
        { "data": "updatedby" }
    ],
    "order": [[0, "desc"]],
    "columnDefs": [
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
    buttons: [
        {
            extend: 'excelHtml5',
            title: 'รายงานประวัติการโอนสินค้า',
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
        $('.dataTables_filter').appendTo("#tbItemTransferHistory");
        $('.dataTables_filter').appendTo('.search-input');
    },
});

$("#btnSearch").on('click', function (event) {
    ShowLoading();
    event.preventDefault(); // Prevent the default form submission
    datatable.ajax.reload(); // This will use the updated parameters automatically
    HideLoading();
    //var transferStartdate = $("#txtTransferDate").val();
    //var transferEndDate = $("#txtTransferEndDate").val();

    //var selectedBranch = $("#ddlTransferBranch").val();
    //var branchid = parseInt(selectedBranch);

    //var selectedTransferStatus = $("#ddlTransferStatus").val();
    //var transferstatusid = parseInt(selectedTransferStatus);

    //var reqdata = { "transferstartdate": transferStartdate, "transferenddate": transferEndDate, "branchid": branchid, "transferstatusid": transferstatusid };
    //var jsonreqdata = JSON.stringify(reqdata);
    //console.log(jsonreqdata);
    //var request = $.ajax({
    //    type: 'POST',
    //    url: '/Item/SearchItemTransferHistory',
    //    data: jsonreqdata,
    //    contentType: 'application/json',
    //    success: function (response) {

    //        if (response.result) {
    //            ShowMessageSuccess(response.message);
    //        }
    //        else {
    //            AlertErrorNoTitle(response.message);
    //        }

    //        //console.log(response.data);
    //        $("#tbItemTransferHistory").DataTable().clear().rows.add(response.data).draw();

    //        HideLoading();
    //    },
    //    failure: function (response) {
    //        AlertError(response.message);
    //    },
    //    error: function (response) {
    //        AlertError(response.message);
    //    }
    //});
});

//$(document).on('change', '.select2', function (e) {

//    // Get the selected value
//    var selectedValue = $(this).val();
//    // Get the data-row attribute to identify the row
//    var row = $(this).data('row');
//    console.log($(this).data('name'));
//    // Log the selected value for the current row (you can replace this with your desired logic)
//    console.log("Row " + row + ": " + selectedValue);
//    //ShowMessageInfo('Selected value :' + selectedValue);
//});

//$('.ddl-transferstatus').on('change', function () {
//    var selectedTransferStatus = $(this).val();
//    console.log("Selected Transfer Status:", selectedTransferStatus);
//    var transferstatusid = parseInt(selectedTransferStatus, 10) || -1;
//    console.log("Transfer Status ID:", transferstatusid);
//});

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
                url: '/Item/DeleteItem',
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
                url: '/Item/DeleteItemInBranch',
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

function InitialData() {
    datatable = $('#tbItemTransferHistory').DataTable({
        "sDom": 'fBtlpi',
        "destroy": true,
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,
        "ajax": {
            "url": "/Item/GetItemTransferHistoryV3",
            "type": "POST"
        },
        "columns": [
            {
                "render": function () {
                    console.log('render columns : checkbox');
                    return "<label class='checkboxs'><input type='checkbox' id='select-all'><span class='checkmarks'></span></label>";
                }
            },
            //{
            //    "data": { itemimageurl: "itemimageurl", name: "name" },
            //    "render": function (data) {
            //        console.log('columns : render => ' + data);
            //        return "<a asp-action='Detail' asp-controller='Item' asp-all-route-data='aItemID'>" + data.name + "</a>";
            //    }
            //},
            {
                "data": { createddate: "createddate" },
                "render": function (data) {
                    if (data.createddate === null || data.createddate == null) {
                        return data.createddate;
                    }
                    return formatDateTime(new Date(data.createddate));
                }
            },
            { "data": "sourcename" },
            { "data": "destinationname" },
            { "data": "itemname" },
            { "data": "qty" },
            { "data": "description" },
            {
                "data": { transferid: "transferid", transferstatusid: "transferstatusid", transferstatusname_th: "transferstatusname_th" },
                "render": function (data) {
                    var _transferstatusid = parseInt(data.transferstatusid);
                    if (_transferstatusid == 1) {
                        return "<span class='badges bg-lightgreen'>" + data.transferstatusname_th + "</span>";
                    }
                    else if (_transferstatusid == 2 || _transferstatusid == 99) {
                        return "<span class='badges bg-lightred'>" + data.transferstatusname_th + "</span>";
                    }
                    else {
                        return "<a href='ReceiveItemTransfer?transferid=" + data.transferid + "' class='me-3' title='คลิก เพื่อตรวจรับสินค้า'><span class='badges bg-lightred'>" + data.transferstatusname_th + "</span></a>";
                    }
                    return "<span class='badges bg-lightyellow'>N/A</span>";

                }
            },
            { "data": "receiveqty" },
            { "data": "returnqty" },
            { "data": "createdby" },
            {
                "data": { updateddate: "updateddate" },
                "render": function (data) {
                    if (data.updateddate === null || data.updateddate == null) {
                        return data.updateddate;
                    }
                    return formatDateTime(new Date(data.updateddate));
                }
            },
            { "data": "updatedby" }
        ],
        //"language": {
        //    "emptyTable": "ไม่พบข้อมูล."
        //},
        "order": [[0, "desc"]],
        "columnDefs": [
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
            "emptyTable": "ไม่พบข้อมูล."
        },
        initComplete: (settings, json) => {
            $('.dataTables_filter').appendTo("#tbItemTransferHistory");
            $('.dataTables_filter').appendTo('.search-input');
        },
        /*dom: 'Bfrtip',*/
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'รายงานประวัติการโอนสินค้า',
                text: 'ดาวโหลดไฟล์ Excel',
                class: 'btn-primary',
                //Columns to export
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]
                }
            },
            {
                extend: 'pdfHtml5',
                title: 'PDF',
                text: 'Export to PDF'
                //Columns to export
                //exportOptions: {
                //     columns: [0, 1, 2, 3, 4, 5, 6]
                //  }
            }
        ]
    });

}
