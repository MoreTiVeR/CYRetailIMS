
var datatable;
InitialNumberInput();
InitialItemsTransferDataTable();
$('.select2').select2();

//datatable = $("#tbItemsTransfer").DataTable({
//    "destroy": true,
//    "bFilter": true,
//    "sDom": 'fBtlpi',
//    "pagingType": 'numbers',
//    "ordering": true,
//    "pageLength": 50,
//    "autoWidth": false,
//    "ajax": {
//        "url": "/Inventory/GetDrafItemTransferOfMonth",
//        "type": "GET",
//        "datatype": "json"
//    },
//    "columns": [
//        {
//            "data": { transferheaderid: "transferheaderid" },
//            "render": function (data) {
//                console.log('render columns : checkbox');
//                return "<label class='checkboxs'><input type='checkbox' id='select-all' name='select_draftid_" + data.transferheaderid +"'><span class='checkmarks'></span></label>";
//            }
//        },
//        {
//            "data": { transferheaderid: "transferheaderid", transferstatus: "transferstatus" },
//            "render": function (data) {
//                //var _transferheaderid = parseInt(data.transferheaderid);
//                console.log(data.transferheaderid);
//                if (data.transferstatus == 1) {
//                    return "<span class='badges bg-lightgreen'>บันทึกแล้ว</span>";
//                }
//                else if (data.transferstatus == 0) {
//                    return "<a href='Draft?draftid=" + data.transferheaderid + "' class='me-3' title='คลิก เพื่อทำรายการโอนสินค้าต่อ'><span class='badges bg-lightyellow'>ฉบับร่าง</span></a>";
//                    //return "<center><a href='../Inventory/InventoryTransfer' class='me-3' title='คลิก เพื่อทำรายการโอนสินค้าต่อ'><span class='badges bg-lightyellow'>ฉบับร่าง</span></a></center>";
//                }
//                else {
//                    return "<span class='badges bg-lightyellow'>N/A</span>";
//                }
//                return "<span class='badges bg-lightyellow'>N/A</span>";

//            }
//        },
//        { "data": "refno" },
//        { "data": "destinationbranchid" },
//        { "data": "destinationbranchname" },
//        { "data": "createdby" },
//        { "data": "createddate" }
//    ],
//    //"language": {
//    //    "emptyTable": "ไม่พบข้อมูล."
//    //},
//    "order": [[3, "asc"]],
//    "columnDefs": [
//        {
//            "targets": [0, 3],
//            "visible": false
//        }
//    ],
//    "language": {
//        search: ' ',
//        sLengthMenu: '_MENU_',
//        searchPlaceholder: "ค้นหาข้อมูล...",
//        info: "_START_ - _END_ of _TOTAL_ items",
//        "emptyTable": "ไม่พบข้อมูล."
//    },
//    initComplete: (settings, json) => {
//        $('.dataTables_filter').appendTo("#tbItemsTransfer");
//        $('.dataTables_filter').appendTo('.search-input');
//    },
//    /*dom: 'Bfrtip',*/
//    buttons: [
//        {
//            extend: 'excelHtml5',
//            title: 'รายงานโอนสินค้า',
//            text: 'ดาวโหลดไฟล์ Excel',
//            class: 'btn-primary',
//            //Columns to export
//            exportOptions: {
//                columns: [1, 2, 3, 4, 5, 6]
//            }
//        },
//        {
//            extend: 'pdfHtml5',
//            title: 'PDF',
//            text: 'Export to PDF'
//            //Columns to export
//            //exportOptions: {
//            //     columns: [0, 1, 2, 3, 4, 5, 6]
//            //  }
//        }
//    ]
//});

$(document).on('change', '.select2', function (e) {

    // Get the selected value
    var selectedValue = $(this).val();
    // Get the data-row attribute to identify the row
    var row = $(this).data('row');
    console.log($(this).data('name'));
    // Log the selected value for the current row (you can replace this with your desired logic)
    console.log("Row " + row + ": " + selectedValue);
    //ShowMessageInfo('Selected value :' + selectedValue);
});

$('#btnConfirmTransfer').on('click', function (e) {

    ShowLoading();
    e.preventDefault();
    var data = datatable.$('input, select').serialize();
    var object_update = {
        InventoryTransferDataList: datatable.rows()
            .data()
            .toArray()
            .map((el) => {
                //console.log(el.itemid);
                var txtRefillQty = datatable.$('input[name=itemid_' + el.itemid + '], select');
                var isCheck = datatable.$('input[name=select_itemid_' + el.itemid + '], select');
                el.ischeck = isCheck.is(":checked");
                el.refillqty = parseInt(txtRefillQty.val());
                return el;
            })
    }
    console.log(object_update);

    var reqData = { "detail": object_update.InventoryTransferDataList };
    var jsonData = JSON.stringify(reqData);
    console.log(jsonData);

    var request = $.ajax({
        type: 'POST',
        url: '/Inventory/ItemInvenrotyTransferValidation',
        data: jsonData,
        contentType: 'application/json',
        success: function (response) {

            if (response.result) {

                Swal.fire({
                    //title: 'ยืนยันการบันทึกข้อมูล?',
                    //text: 'กรุณาตรวจสอบข้อมูลก่อนทำการบันทึก!',
                    //type: 'warning',
                    title: '<strong>ยืนยันการบันทึกข้อมูล?</strong>',
                    icon: 'warning',
                    html: '<u><span style="color:red">กรุณาตรวจสอบข้อมูลก่อนทำการบันทึก!</span></u>',
                    showCancelButton: true,
                    //showDenyButton: true,
                    confirmButtonColor: '#04B431',
                    confirmButtonText: 'บันทึก',
                    cancelButtonColor: '#D33',
                    cancelButtonText: "ยกเลิก",
                    //denyButtonText: 'ยืนยัน-ไม่ออกใบเสร็จ',
                    //denyButtonColor: '#D33',
                    customClass: {
                        confirmButton: 'btn btn-success',
                        denyButton: 'btn btn-warning ml-1',
                        cancelButton: 'btn btn-danger ml-1'
                    },
                    buttonsStyling: false,
                    focusConfirm: true
                }).then(function (result) {
                    if (result.value) {
                        //Post: SaveInvenrotyTransfer
                        //ShowMessageSuccess(result.message);

                        var request = $.ajax({
                            type: 'POST',
                            url: '/Inventory/CreateItemInvenrotyTransfer',
                            data: jsonData,
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

                                //console.log(response.data);
                                //$("#tblInventoryReport").DataTable().clear().rows.add(response.data).draw();
                                //HideLoading();
                            },
                            failure: function (response) {
                                AlertError(response.message);
                            },
                            error: function (response) {
                                AlertError(response.message);
                            }
                        });
                    }
                    else if (result.dismiss === Swal.DismissReason.cancel) {
                        //Code
                        ShowMessageInfo('ยกเลิก');
                    }
                });
            }
            else {
                AlertErrorNoTitle(response.message);
            }

            console.log(response.data);
            //$("#tblInventoryReport").DataTable().clear().rows.add(response.data).draw();
            HideLoading();
        },
        failure: function (response) {
            AlertError(response.message);
        },
        error: function (response) {
            AlertError(response.message);
        }
    });

    //$.post("InvenrotyTransferDataValidation", { jsonData }).then(
    //    function (results) {

    //        if (results.result) {
    //            console.log(results.msg);
    //            Swal.fire({
    //                //title: 'ยืนยันการบันทึกข้อมูล?',
    //                //text: 'กรุณาตรวจสอบข้อมูลก่อนทำการบันทึก!',
    //                //type: 'warning',
    //                title: '<strong>ยืนยันการบันทึกข้อมูล?</strong>',
    //                icon: 'warning',
    //                html: '<u><span style="color:red">กรุณาตรวจสอบข้อมูลก่อนทำการบันทึก!</span></u>',
    //                showCancelButton: true,
    //                //showDenyButton: true,
    //                confirmButtonColor: '#04B431',
    //                confirmButtonText: 'บันทึก',
    //                cancelButtonColor: '#D33',
    //                cancelButtonText: "ยกเลิก",
    //                //denyButtonText: 'ยืนยัน-ไม่ออกใบเสร็จ',
    //                //denyButtonColor: '#D33',
    //                customClass: {
    //                    confirmButton: 'btn btn-success',
    //                    denyButton: 'btn btn-warning ml-1',
    //                    cancelButton: 'btn btn-danger ml-1'
    //                },
    //                buttonsStyling: false,
    //                focusConfirm: true
    //            }).then(function (result) {
    //                if (result.value) {
    //                    //Post: SaveInvenrotyTransfer
    //                    ShowMessageSuccess('Post: SaveInvenrotyTransfer');

    //                    var request = $.ajax({
    //                        type: 'POST',
    //                        url: '/Inventory/SaveInvenrotyTransfer',
    //                        data: jsonData,
    //                        contentType: 'application/json',
    //                        success: function (response) {

    //                            if (response.result) {
    //                                ShowMessageSuccess(response.message);

    //                                //Update the DataTable with the filtered data from the server
    //                                /*console.log(response.data);*/
    //                                /*$("#tbItemTransferHistory").DataTable().clear().rows.add(response.data).draw();*/
    //                            }
    //                            else {
    //                                AlertErrorNoTitle(response.message);
    //                            }

    //                            console.log(response.data);
    //                            $("#tblInventoryReport").DataTable().clear().rows.add(response.data).draw();
    //                            HideLoading();
    //                        },
    //                        failure: function (response) {
    //                            AlertError(response.message);
    //                        },
    //                        error: function (response) {
    //                            AlertError(response.message);
    //                        }
    //                    });
    //                }
    //                else if (result.dismiss === Swal.DismissReason.cancel) {
    //                    //Code
    //                    ShowMessageInfo('ยกเลิก');
    //                }
    //            });
    //        }
    //        else {
    //            ShowMessageError(results.msg);
    //            return;
    //        }

    //    }, function (results) {
    //        //Failed
    //        console.log('Failed');
    //        ShowMessageError(results.message);

    //    }, function () {
    //        ShowMessageError('Unknow error => Create Sale data.');
    //        console.log('this will run if the deferred generates a progress update.');
    //    }
    //);

});

$('#btnSaveDraft').on('click', function (e) {
    
    e.preventDefault();
    var data = datatable.$('input, select').serialize();
    var object_update = {
        InventoryTransferDataList: datatable.rows()
            .data()
            .toArray()
            .map((el) => {
                //console.log(el.itemid);
                var txtRefillQty = datatable.$('input[name=itemid_' + el.itemid + '], select');
                var isCheck = datatable.$('input[name=select_itemid_' + el.itemid + '], select');
                el.ischeck = isCheck.is(":checked");
                el.refillqty = parseInt(txtRefillQty.val());
                return el;
            })
    }
    console.log(object_update);

    var reqData = { "detail": object_update.InventoryTransferDataList };
    var jsonData = JSON.stringify(reqData);
    console.log(jsonData);

    Swal.fire({
        //title: 'ยืนยันการบันทึกข้อมูล?',
        //text: 'กรุณาตรวจสอบข้อมูลก่อนทำการบันทึก!',
        //type: 'warning',
        title: '<strong>ยืนยันการบันทึกข้อมูลฉบับร่าง?</strong>',
        icon: 'warning',
        html: '<u><span style="color:red">กรุณาตรวจสอบข้อมูลก่อนทำการบันทึกฉบับร่าง!</span></u>',
        showCancelButton: true,
        //showDenyButton: true,
        confirmButtonColor: '#04B431',
        confirmButtonText: 'บันทึก',
        cancelButtonColor: '#D33',
        cancelButtonText: "ยกเลิก",
        //denyButtonText: 'ยืนยัน-ไม่ออกใบเสร็จ',
        //denyButtonColor: '#D33',
        customClass: {
            confirmButton: 'btn btn-success',
            denyButton: 'btn btn-warning ml-1',
            cancelButton: 'btn btn-danger ml-1'
        },
        buttonsStyling: false,
        focusConfirm: true
    }).then(function (result) {
        if (result.value) {
            ShowLoading();
            var request = $.ajax({
                type: 'POST',
                url: '/Inventory/CreateDraftItemInvenrotyTransfer',
                data: jsonData,
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

                    //console.log(response.data);
                    //$("#tblInventoryReport").DataTable().clear().rows.add(response.data).draw();
                    HideLoading();
                },
                failure: function (response) {
                    AlertError(response.message);
                },
                error: function (response) {
                    AlertError(response.message);
                }
            });
        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            //Code
            ShowMessageInfo('ยกเลิก');
            HideLoading();
        }

    });

});

$("#btnSearch").on('click', function (event) {
    ShowLoading();

    event.preventDefault(); // Prevent the default form submission

    var text = $("#ddlBranch :selected").text();
    var sbranchid = $("#ddlBranch :selected").val();
    var branchid = parseInt(sbranchid);

    //SearchTransferData(branchid, brandid);
    var reqdata = { "branchid": branchid };
    var jsonData = JSON.stringify(reqdata);
    console.log(jsonData);
    var request = $.ajax({
        type: 'POST',
        url: '/Inventory/SearchInvenrotyTransferForIndex',
        data: jsonData,
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
            $("#tbItemsTransfer").DataTable().clear().rows.add(response.data).draw();
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

function SearchTransferData(branchid, brandid) {

    var reqdata = { "branchid": branchid, "brandid": brandid };
    var jsonData = JSON.stringify(reqdata);
    console.log(jsonData);
    var request = $.ajax({
        type: 'POST',
        url: '/Inventory/SearchInvenrotyTransfer',
        data: jsonData,
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
            $("#tbItemInventoryTransfer").DataTable().clear().rows.add(response.data).draw();
            
        },
        failure: function (response) {
            AlertError(response.message);
        },
        error: function (response) {
            AlertError(response.message);
        }
    });
}

async function InitialItemsTransferDataTable() {
    datatable = $("#tbItemsTransfer").DataTable({
        "destroy": true,
        "bFilter": true,
        "sDom": 'fBtlpi',
        "pagingType": 'numbers',
        "ordering": true,
        "pageLength": 10,
        "autoWidth": false,
        "ajax": {
            url: "/Inventory/GetDrafItemTransferOfMonth",
            type: "GET",
            datatype: "json",
            async: true
        },
        "columns": [
            {
                "data": { transferheaderid: "transferheaderid" },
                "render": function (data) {
                    console.log('render columns : checkbox');
                    return "<label class='checkboxs'><input type='checkbox' id='select-all' name='select_draftid_" + data.transferheaderid + "'><span class='checkmarks'></span></label>";
                }
            },
            {
                "data": { transferheaderid: "transferheaderid", transferstatus: "transferstatus" },
                "render": function (data) {
                    //var _transferheaderid = parseInt(data.transferheaderid);
                    console.log(data.transferheaderid);
                    if (data.transferstatus == 1) {
                        return "<span class='badges bg-lightgreen'>บันทึกแล้ว</span>";
                    }
                    else if (data.transferstatus == 0) {
                        return "<a href='Draft?draftid=" + data.transferheaderid + "' class='me-3' title='คลิก เพื่อทำรายการโอนสินค้าต่อ'><span class='badges bg-lightyellow'>ฉบับร่าง</span></a>";
                        //return "<center><a href='../Inventory/InventoryTransfer' class='me-3' title='คลิก เพื่อทำรายการโอนสินค้าต่อ'><span class='badges bg-lightyellow'>ฉบับร่าง</span></a></center>";
                    }
                    else {
                        return "<span class='badges bg-lightyellow'>N/A</span>";
                    }
                    return "<span class='badges bg-lightyellow'>N/A</span>";

                }
            },
            { "data": "refno" },
            { "data": "destinationbranchid" },
            { "data": "destinationbranchname" },
            { "data": "createdby" },
            {
                "data": { createddate: "createddate" },
                "render": function (data) {
                    if (data.createddate === null || data.createddate == null) {
                        return data.createddate;
                    }
                    return formatDate(new Date(data.createddate));
                    //var _createddate = new Date(data.createddate).toLocaleDateString("en-US");
                    //return _createddate;
                }
            }
        ],
        //"language": {
        //    "emptyTable": "ไม่พบข้อมูล."
        //},
        "order": [[3, "asc"]],
        "columnDefs": [
            {
                "targets": [0, 3],
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
            $('.dataTables_filter').appendTo("#tbItemsTransfer");
            $('.dataTables_filter').appendTo('.search-input');
        },
        /*dom: 'Bfrtip',*/
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'รายงานโอนสินค้า',
                text: 'ดาวโหลดไฟล์ Excel',
                class: 'btn-primary',
                //Columns to export
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6]
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
