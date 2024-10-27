
var datatable;
InitialNumberInput();
$('.select2').select2();

//var editor = new DataTable.Editor({
//    ajax: '/Inventory/GetItemInventoryTransfer',
//    fields: [
//        {
//            label: 'จำนวน/แก้ไขได้:',
//            name: 'orderqty'
//        }
//    ],
//    table: '#tbItemInventoryTransfer'
//});

datatable = $("#tbItemInventoryTransfer").DataTable({
    "destroy": true,
    "bFilter": true,
    "sDom": 'fBtlpi',
    "pagingType": 'numbers',
    "ordering": true,
    "pageLength": 50,
    "autoWidth": false,
    "ajax": {
        "url": "/Inventory/GetItemInventoryTransfer",
        "type": "GET",
        "datatype": "json"
    },
    "columns": [
        {
            "data": { itemid: "itemid", "itemcode": "itemcode" },
            "render": function (data) {
                console.log('render columns : checkbox');
                return "<label class='checkboxs'><input type='checkbox' id='select-all' name='select_itemid_" + data.itemid +"'><span class='checkmarks'></span></label>";
            }
        },
        //{
        //    "data": { itemimageurl: "itemimageurl", name: "name" },
        //    "render": function (data) {
        //        console.log('columns : render => ' + data);
        //        return "<a asp-action='Detail' asp-controller='Item' asp-all-route-data='aItemID'>" + data.name + "</a>";
        //    }
        //},
        { "data": "branchid" },
        { "data": "itemid" },
        { "data": "itemcode" },
        { "data": "itemname" },
        { "data": "brandname" },
        { "data": "qtyinstock" },
        { "data": "qtyinbranch" },
        { "data": "notifyminqty" },
        { "data": "orderqty" },
        //{ "data": "refillqty" },
        {
            "data": { itemid: "itemid", refillqty: "refillqty", "itemcode": "itemcode" },
            "render": function (data) {
                console.log('columns : render => ' + data);
                return "<input type='number' id='itemid_" + data.itemid + "' name='itemid_" + data.itemid +"' value='" + data.refillqty +"'>";
            }
        },
        { "data": "refillqty" }
    ],
    //"language": {
    //    "emptyTable": "ไม่พบข้อมูล."
    //},
    "order": [[2, "asc"]],
    "columnDefs": [
        {
            "targets": [1, 2, 11],
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
        $('.dataTables_filter').appendTo("#tbItemInventoryTransfer");
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
                columns: [0, 3, 4, 5, 6, 7, 8, 9, 11]
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
                        HideLoading();

                        //Update the DataTable with the filtered data from the server
                        /*console.log(response.data);*/
                        /*$("#tbItemTransferHistory").DataTable().clear().rows.add(response.data).draw();*/

                        window.location.href = '/Inventory/Index';
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
    var sbrandid = $("#ddlBrand :selected").val();

    var branchid = parseInt(sbranchid);
    var brandid = parseInt(sbrandid);

    //SearchTransferData(branchid, brandid);
    var reqdata = { "branchid": branchid, "brandid": brandid };
    var jsonData = JSON.stringify(reqdata);
    console.log(jsonData);
    var request = $.ajax({
        type: 'POST',
        url: '/Inventory/SearchInvenrotyTransferForTransfer',
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

$("#btnCancel").on('click', function (e) {
    window.location.href = '/Inventory/Index';
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