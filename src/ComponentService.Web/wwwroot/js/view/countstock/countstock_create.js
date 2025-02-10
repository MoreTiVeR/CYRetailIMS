
var datatable;

$('.select2').select2();


datatable = $("#tbMoneyTransfer").DataTable({
    "destroy": true,
    "bFilter": true,
    "sDom": 'fBtlpi',
    'pagingType': 'numbers',
    "ordering": true,
    "ajax": {
        "url": "/MoneyTransfer/GetMoneyTransfer",
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
        { "data": "moneytransferid" },
        {
            "data": { transferdate: "transferdate" },
            "render": function (data) {
                if (data.transferdate === null || data.transferdate == null) {
                    return data.transferdate;
                }
                return formatDate(new Date(data.transferdate));
            }
        },
        {
            "data": { transferdate: "transferdate" },
            "render": function (data) {
                if (data.transferdate === null || data.transferdate == null) {
                    return data.transferdate;
                }
                return formatTimeHHMM(new Date(data.transferdate));
            }
        },
        { "data": "branchid" },
        { "data": "branchname" },
        { "data": "amounttransfer" },
        { "data": "description" },
        { "data": "createdby" },
        {
            "data": { createddate: "createddate" },
            "render": function (data) {
                if (data.createddate === null || data.createddate == null) {
                    return data.createddate;
                }
                return formatDateTime(new Date(data.createddate));
                //return formatDate(new Date(data.createddate));
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
            "data": { moneytransferid: "moneytransferid", branchid: "branchid", imgpath: "imgpath" },
            "render": function (data) {
                var dict = {
                    "moneytransferid": data.moneytransferid,
                };
                /*console.log('data dic:' + dict);*/
                //return "<a class='me-3' href='" + data.imgpath + "' title='แก้ไขรายการโอน'><img src='../assets/img/icons/eye.svg' alt='img'></a><a class='me-3' href='Edit?mTransferID=" + data.moneytransferid + "' title='แก้ไขรายการโอน'><img src='../assets/img/icons/edit.svg' alt='img'></a><a id='rowid" + data.moneytransferid + "' onclick=deleteMoneyTransfer(" + data.moneytransferid + ") class='me-3'><img src='../assets/img/icons/delete.svg' alt='ลบรายการโอน' title='ลบรายการโอน'></a>";
                return "<a id='rowid" + data.moneytransferid + "' onclick=showGallery(" + data.moneytransferid + ") class='me-3' title='ดูรูปสลิปเงินโอน'><img src='../assets/img/icons/eye.svg' alt='ดูรูปสลิป'></a><a class='me-3' href='Edit?mTransferID=" + data.moneytransferid + "' title='แก้ไขรายการโอน'><img src='../assets/img/icons/edit.svg' alt='img'></a><a id='rowid" + data.moneytransferid + "' onclick=deleteMoneyTransfer(" + data.moneytransferid + ") class='me-3' title='ลบรายการเงินโอน'><img src='../assets/img/icons/delete.svg' alt='ลบรายการเงินโอน'></a>";
            }
        }
        //{
        //    "data": { moneytransferid: "moneytransferid", imgpath: "imgpath" },
        //    "render": function (data) {
        //        return "<button onclick='showGallery(1, " + data.moneytransferid + ")'>Show</button>";
        //        //return "<a class='me-2' href='" + data.imgpath + "' target='_blank' title='คลิกเพื่อดูสลิป'><img src='" + data.imgpath + "' class='avatar' width='25' height='25' alt='img'></a>";
        //    }
        //}
    ],
    "order": [[1, "desc"]],
    "columnDefs": [
        {
            "targets": [0, 1, 4],
            "visible": false
        },
        {
            "targets": [10],
            "className": "text-center"
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
        $('.dataTables_filter').appendTo("#tbMoneyTransfer");
        $('.dataTables_filter').appendTo('.search-input');
    },
    /*dom: 'Bfrtip',*/
    buttons: [
        {
            extend: 'excelHtml5',
            title: 'รายงานบันทึกการโอนเงิน',
            text: 'ดาวโหลดไฟล์ Excel',
            class: 'btn-primary',
            //Columns to export
            exportOptions: {
                columns: [2, 4, 5, 6, 7, 8]
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

$("#btnSearch").on('click', function (event) {

    ShowLoading();
    event.preventDefault(); // Prevent the default form submission

    var selectedBranch = $("#ddlBranch").val();
    var branchid = parseInt(selectedBranch);

    var startdate = $("#txtStartDate").val();
    var enddate = $("#txtEndDate").val();
    //console.log('branchid:' + startdate);
    //console.log('startdate:' + branchid);
    //console.log('enddate:' + enddate);
    //if (branchid === null || branchid === undefined || branchid === '') {
    //    ShowMessageError('กรุณาระบุเงื่อนไขก่อนทำการค้นหา');
    //    event.preventDefault();
    //    HideLoading();
    //    return;
    //}
    //if (startdate === null || startdate === undefined || startdate === '') {
    //    ShowMessageError('กรุณาระบุเงื่อนไขก่อนทำการค้นหา');
    //    event.preventDefault();
    //    HideLoading();
    //    return;
    //}
    //if (enddate === null || enddate === undefined || enddate === '') {
    //    ShowMessageError('กรุณาระบุเงื่อนไขก่อนทำการค้นหา');
    //    event.preventDefault();
    //    HideLoading();
    //    return;
    //}
    var reqdata = { "branchid": branchid, "startdate": startdate, "enddate": enddate };
    var jsonreqdata = JSON.stringify(reqdata);
    console.log(jsonreqdata);
    var request = $.ajax({
        type: 'POST',
        url: '/MoneyTransfer/SearchMoneyTransfer',
        data: jsonreqdata,
        contentType: 'application/json',
        success: function (response) {

            if (response.result) {
                ShowMessageSuccess(response.message);

                //Update the DataTable with the filtered data from the server
                /*console.log(response.data);*/
                /*$("#tbItemTransferHistory").DataTable().clear().rows.add(response.data).draw();*/
            }
            else {
                AlertErrorNoTitle(response.message);
            }

            console.log(response.data);
            $("#tbMoneyTransfer").DataTable().clear().rows.add(response.data).draw();
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
function deleteMoneyTransfer(moneytransferid) {

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
        cancelButtonText: "ยกเลิก",
        //confirmButtonClass: "btn btn-primary",
        //cancelButtonClass: "btn btn-danger ml-1",
        customClass: {
            confirmButton: "btn btn-primary",
            cancelButton: "btn btn-danger ml-1"
        },
        buttonsStyling: false,
    }).then(function (t) {
        if (t.value) {

            ShowLoading();

            //Delete
            $.ajax({
                type: 'POST',
                url: '/MoneyTransfer/DeleteTransaction',
                data: JSON.stringify({ moneytransferid: moneytransferid }),
                contentType: 'application/json',
                success: function (data) {
                    if (data.result) {

                        AlertSuccess('ลบข้อมูลสำเร็จ');
                        HideLoading();
                        //ShowMessageSuccess(data.message);

                        //To do next?
                        //window.location = data.url;
                        //itemDataTable.row('.selected').remove().draw(false);
                        //dataTable.ajax.reload();
                        /*$("#tbItems").DataTable().ajax.reload();*/
                        /* $('#tbItems').DataTable().ajax.reload();*/
                        //$('#tbItems').DataTable().ajax.reload();

                        console.log("#rowid" + moneytransferid);
                        //$("#rowid" + itemid).closest("tr").remove();

                        //Reload data
                        $('#tbMoneyTransfer').DataTable().ajax.reload();

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
                        HideLoading();
                    }
                }
            });
        }
    });
}

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
    "stateSave": true
});

$("#btnSaveCountStock").on('click', function (event) {
    ShowLoading();
    event.preventDefault(); // Prevent the default form submission

    let updatedItems = [];

    // Loop through each row to collect data
    $('#countStockTable tbody tr').each(function () {
        let row = $(this);

        updatedItems.push({
            ItemTypeCode: row.find('td:eq(0)').text(),
            SubItemCode: row.find('td:eq(1)').text(),
            ItemId: row.find('td:eq(2)').text(),
            StoreStock: row.find('td:eq(3)').text(),
            CountedQty: row.find('td:eq(4)').text(),
            WaitingToRestock: row.find('td:eq(5)').text(),
            Damaged: row.find('td:eq(6)').text(),
            SoldBeforeCount: row.find('td:eq(7)').text(),
            TotalCounted: row.find('td:eq(8)').text(),
            Difference: row.find('td:eq(9)').text()
        });
    });

    // Send data to the server via AJAX
    $.ajax({
        url: '/Stock/Save',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(updatedItems),
        success: function (response) {
            ShowMessageSuccess('Stock counts updated successfully!');
            HideLoading();
        },
        error: function (xhr, status, error) {
            ShowMessageError('An error occurred while saving stock counts.');
            HideLoading();
        }
    });
});

// Handle dropdown selection change
$('#ddlItemType').on('change', function () {
    let selectedValue = $(this).val(); // Get the selected value from the dropdown
    ShowMessageInfo(selectedValue);
    // Apply search filter to the DataTable
    table.column(0) // Assuming the first column (index 0) corresponds to the branch/type
        .search(selectedValue)
        .draw(); // Redraw the table with the filtered data
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