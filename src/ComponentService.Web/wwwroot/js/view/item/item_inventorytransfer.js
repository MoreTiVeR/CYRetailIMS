
var datatable;
InitialNumberInput();
$('.select2').select2();

//var editor = new DataTable.Editor({
//    ajax: '/Item/GetItemInventoryTransfer',
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
    'pagingType': 'numbers',
    "ordering": true,
    "ajax": {
        "url": "/Item/GetItemInventoryTransfer",
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
    ],
    //"language": {
    //    "emptyTable": "ไม่พบข้อมูล."
    //},
    "order": [[2, "asc"]],
    "columnDefs": [
        {
            "targets": [1, 2],
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
            title: 'รายงานโอนสินค้าขั้น',
            text: 'ดาวโหลดไฟล์ Excel',
            class: 'btn-primary',
            //Columns to export
            exportOptions: {
                columns: [0, 3, 4, 5, 6, 7, 8, 9]
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
        url: '/Item/CreateItemInvenrotyTransferValidation',
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
                        ShowMessageSuccess('Post: SaveInvenrotyTransfer');

                        var request = $.ajax({
                            type: 'POST',
                            url: '/Item/CreateItemInvenrotyTransfer',
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
                                $("#tblInventoryReport").DataTable().clear().rows.add(response.data).draw();
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
    //                        url: '/Item/SaveInvenrotyTransfer',
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
        url: '/Item/SearchInvenrotyTransfer',
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

function SearchTransferData(branchid, brandid) {

    var reqdata = { "branchid": branchid, "brandid": brandid };
    var jsonData = JSON.stringify(reqdata);
    console.log(jsonData);
    var request = $.ajax({
        type: 'POST',
        url: '/Item/SearchInvenrotyTransfer',
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

/*NoUse*/
$("#btnSave").on('click', function () {
    var isValid = $('#frmTransferItem').valid();
    if (!isValid) {
        ShowMessageError('กรุณาตรวจสอบข้อมูลก่อนบันทึกข้อมูล!');
    }
    else {
        var data = $($("#frmTransferItem")).serializeJSON();
        $.post("ItemTransferDataValidation", { data }).then(
            function (results) {

                if (results.result) {
                    console.log(results.msg);
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
                            $("#frmTransferItem").submit();
                        }
                        else if (result.dismiss === Swal.DismissReason.cancel) {
                            //Code
                            ShowMessageInfo('ยกเลิก');
                        }
                    });
                }
                else {
                    ShowMessageError(results.msg);
                    return;
                }

            }, function (results) {
                //Failed
                console.log('Failed');
                ShowMessageError(results.message);

            }, function () {
                ShowMessageError('Unknow error => Create Sale data.');
                console.log('this will run if the deferred generates a progress update.');
            }
        );
    }
});

$("#btnAdd").on('click', function () {
    var trows = parseInt($("#totalrow").val()) + parseInt(1);
    $("#totalrow").val(trows);
});

/*dropdown event*/
$("#transfertypeid").on("change", function () {
    var text = $('option:selected', $(this)).text();
    var transfertypeid = parseInt($('option:selected', $(this)).val());
    var request = $.ajax({
        url: '/Item/FillSourceDestinationBranch',
        async: true,
        type: 'POST',
        dataType: 'JSON',
        data: { "transferTypeID": transfertypeid },
        success: function (response) {

            if (response.result) {
                //สาขาต้นทาง
                var selectList_source_branchid = $('#source_branchid');
                selectList_source_branchid.html(""); // clear before appending new list
                selectList_source_branchid.append($('<option></option>').val("").html("--เลือกสาขาต้นทาง--")); //Add first itemList
                $.each(response.data_source, function () {
                    $("<option />").val(this.value).text(this.text).appendTo(selectList_source_branchid);
                    //prevGroupName = this.group.name;
                });

                //สาขาปลายทาง
                var selectList_destination_branchid = $('#destination_branchid');
                selectList_destination_branchid.html(""); // clear before appending new list
                selectList_destination_branchid.append($('<option></option>').val("").html("--เลือกสาขาปลายทาง--")); //Add first itemList
                $.each(response.data_destination, function () {
                    $("<option />").val(this.value).text(this.text).appendTo(selectList_destination_branchid);
                    //prevGroupName = this.group.name;
                });

                //ItemList
                //var selectList_itembranchtransfer = $('#itembranchtransfer');
                //selectList_itembranchtransfer.html(""); // clear before appending new list
                //selectList_itembranchtransfer.append($('<option></option>').val("").html("--เลือกสินค้าที่ต้องการโอน--")); //Add first itemList
                //$.each(response.data_itemlist, function () {
                //    alert(this.text);
                //    $("<option />").val(this.value).text(this.text).appendTo(selectList_itembranchtransfer);
                //    //prevGroupName = this.group.name;
                //});

                $('.item-transfer-repeater').find('select').html("");
                $("<option />").val("").text("--เลือกสินค้าที่ต้องการโอน--").appendTo($('.item-transfer-repeater').find('select'));
                $('.item-transfer-repeater').find('select').each(function (e) {
                    if (this.type == 'select-one') {
                        if (this.value == '') {
                            $.each(response.data_itemlist, function () {
                                $("<option />").val(this.value).text(this.text).appendTo($('.item-transfer-repeater').find('select'));
                                //prevGroupName = this.group.name;
                            });
                        }
                    }
                });
            }

        },
        failure: function (response) {
            ShowMessageError(response);
        },
        error: function (response) {
            ShowMessageWarning(response);
        }
    });

});

$("#source_branchid").on("change", function () {
    var text = $('option:selected', $(this)).text();
    var sbranchid = parseInt($('option:selected', $(this)).val());
    var transferTypeid = parseInt($("#transfertypeid").val());
    //ShowMessageWarning('source_branchid - change' + 'val -> ' + sbranchid + 'text -> ' + text);
    var request = $.ajax({
        url: '/Item/FillItemTransferByBranchID',
        async: true,
        type: 'POST',
        dataType: 'JSON',
        data: { "transferTypeID": transferTypeid, "branchID": sbranchid },
        success: function (response) {

            if (response.result) {

                //Fill destination branchid selection สาขาปลายทาง
                var selectList_destination_branchid = $('#destination_branchid');
                selectList_destination_branchid.html(""); // clear before appending new list
                selectList_destination_branchid.append($('<option></option>').val("").html("--เลือกสาขาปลายทาง--")); //Add first itemList
                $.each(response.data_destination, function () {
                    $("<option />").val(this.value).text(this.text).appendTo(selectList_destination_branchid);
                    //prevGroupName = this.group.name;
                });

                //Fill item transfer selection in repeater
                $('.item-transfer-repeater').find('select').html("");
                $("<option />").val("").text("--เลือกสินค้าที่ต้องการโอน--").appendTo($('.item-transfer-repeater').find('select'));
                $('.item-transfer-repeater').find('select').each(function (e) {
                    if (this.type == 'select-one') {
                        if (this.value == '') {
                            $.each(response.data_itemlist, function () {
                                $("<option />").val(this.value).text(this.text).appendTo($('.item-transfer-repeater').find('select'));
                                //prevGroupName = this.group.name;
                            });
                        }
                    }
                });
            }
            else {
                ShowMessageError(response.msg);
            }

        },
        failure: function (response) {
            ShowMessageError(response);
        },
        error: function (response) {
            ShowMessageWarning(response);
        }
    });
});

$("#ddlSearchItem").on("change", function () {
    ShowMessageWarning('ddlSearchItem - change');
});

$("#itembranchtransfer").on("change", function () {
    ShowMessageWarning('itembranchtransfer - change');
});

function ResetForm() {
    //Reset Repeater
    $('.outer-item-group').empty();

    //Reset form
    $('#frmTransferItem')[0].reset(); // [0] gets the DOM element from the jQuery object

    //Reset select2
    $("#transfertypeid").val('').trigger('change.select2');

    $("#source_branchid").empty();
    var source_option = new Option("--เลือกสาขาต้นทาง--", "", true, true);
    $("#source_branchid").append(source_option).trigger('change');

    $("#destination_branchid").empty();
    var destination_option = new Option("--เลือกสาขาปลายทาง--", "", true, true);
    $("#destination_branchid").append(destination_option).trigger('change');
}