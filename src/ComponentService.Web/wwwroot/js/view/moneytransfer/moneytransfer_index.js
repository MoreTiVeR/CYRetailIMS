
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
            "data": { moneytransferid: "moneytransferid", branchid: "branchid" },
            "render": function (data) {
                var dict = {
                    "moneytransferid": data.moneytransferid,
                };
                console.log('data dic:' + dict);
                return "<a id='rowid" + data.moneytransferid + "' onclick=deleteMoneyTransfer(" + data.moneytransferid + ") class='me-3'><img src='../assets/img/icons/delete.svg' alt='img'></a>";
                //if (data.isiteminbranch) {
                //    //Branch
                //    return "<a id='rowid" + data.itemid + "' onclick=deleteItemInBranch(" + data.itemid + ',' + data.searchbranchid + ") class='me-3'><img src='../assets/img/icons/delete.svg' alt='img'></a>";
                //}
                //else {
                //    //Warehouse
                //    return "<a id='rowid" + data.itemid + "' onclick=deleteItem(" + data.itemid + ") class='me-3'><img src='../assets/img/icons/delete.svg' alt='img'></a>";
                //}
                //return "<a href='Edit?itemid=" + data.itemid + "'  class='me-3' title='แก้ไขข้อมูลสินค้า'><img src='../assets/img/icons/edit.svg' alt='img'></a><a id='rowid" + data.itemid + "' onclick=deleteItem(" + data.itemid + ") class='me-3'><img src='../assets/img/icons/delete.svg' alt='img'></a>";
            }
        }
    ],
    //"language": {
    //    "emptyTable": "ไม่พบข้อมูล."
    //},
    "order": [[1, "desc"]],
    "columnDefs": [
        {
            "targets": [0, 1, 3],
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
        $('.dataTables_filter').appendTo("#tbMoneyTransfer");
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
                columns: [3, 4, 5, 6, 7]
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

    //var transferdate = $("#txtTransferDate").val();

    var selectedBranch = $("#ddlBranch").val();
    var branchid = parseInt(selectedBranch);

    //var selectedTransferStatus = $("#ddlTransferStatus").val();
    //var transferstatusid = parseInt(selectedTransferStatus);

    var reqdata = { "branchid": branchid };
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
        confirmButtonClass: "btn btn-primary",
        cancelButtonText: "ยกเลิก",
        cancelButtonClass: "btn btn-danger ml-1",
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

                        console.log("#rowid" + itemid);
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

