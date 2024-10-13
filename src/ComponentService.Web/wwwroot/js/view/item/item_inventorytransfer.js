
var datatable;
InitialNumberInput();
$('.select2').select2();

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
        { "data": "itemid" },
        { "data": "itemcode" },
        { "data": "itemname" },
        { "data": "qtyinstock" },
        { "data": "qtyinbranch" },
        { "data": "notifyminqty" },
        { "data": "orderqty" },
        { "data": "refillqty" },
    ],
    //"language": {
    //    "emptyTable": "ไม่พบข้อมูล."
    //},
    "order": [[1, "asc"]],
    "columnDefs": [
        {
            "targets": [1],
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
                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8]
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

//datatable.on('click', 'tbody tr', function () {
//    datatable.row(this).edit();
//});

//datatable.on('click', 'tbody td:not(:first-child)', function (e) {
//    editor.inline(this, {
//        submit: 'allIfChanged'
//    });
//});

/*var table = $('#tbItemInventoryTransfer').DataTable().makeEditable();*/
// Edit row
//$('#tbItemInventoryTransfer tbody').on('click', 'button.edit', function () {
//    var row = $(this).closest('tr');
//    var data = table.row(row).data();

//    // Allow inline editing; the inputs can be either simple input boxes or other types based on your requirement
//    row.find('td:eq(1)').html('<input type="text" value="' + data[1] + '"/>');
//    row.find('td:eq(2)').html('<input type="text" value="' + data[2] + '"/>');
//    $(this).removeClass('edit').addClass('save').text('Save');
//});
//$('#tbItemInventoryTransfer').dataTable().makeEditable({
//    sUpdateURL: "UpdateData.php"
//});
/*$('#tbItemInventoryTransfer').dataTable().makeEditable();*/

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