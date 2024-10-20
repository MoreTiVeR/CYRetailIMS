
var datatable;
$('.select2').select2();

datatable = $("#tblInventoryReport").DataTable({
    "destroy": true,
    "bFilter": true,
    "sDom": 'fBtlpi',
    'pagingType': 'numbers',
    "ordering": true,
    "ajax": {
        "url": "/Report/GetInventoryReport",
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
        { "data": "itemcode" },
        { "data": "itemname" },
        { "data": "qtyinstock" },
        { "data": "totalsale" },
        { "data": "notifymin" },
        { "data": "notifymax" },
        { "data": "firstordernum" },
        { "data": "secoundordernum" }
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
        $('.dataTables_filter').appendTo("#tblInventoryReport");
        $('.dataTables_filter').appendTo('.search-input');
    },
    /*dom: 'Bfrtip',*/
    buttons: [
        {
            extend: 'excelHtml5',
            title: 'รายงานสั่งสินค้าเข้าคลัง',
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

//$("#ddlSearchType").on('change', function (event) {

//    alert(event.val()  + '|' + event);
//});

$('.ddl-inventory-search-type').on("change", function () {
    var text = $('option:selected', $(this)).text();
    var selectedMonth = parseInt($('option:selected', $(this)).val());
    if (selectedMonth == 1) {
        $("#divSearchByDate").attr("hidden", false);
        $("#divSearchByMonth").attr("hidden", true);
    }

    if (selectedMonth == 2) {
        $("#divSearchByDate").attr("hidden", true);
        $("#divSearchByMonth").attr("hidden", false);
    }
});

$("#btnSearch").on('click', function (event) {
    ShowLoading();
    event.preventDefault(); // Prevent the default form submission

    var selectedInventorySearchType = $("#ddlInventorySearchType").val();
    var searchtype = parseInt(selectedInventorySearchType);
    var reportinventorydate = null;
    if (searchtype == 1) {
        reportinventorydate = $("#txtSearchDate").val();
    }
    if (searchtype == 2) {
        reportinventorydate = $("#txtSearchMonth").val();
    }

    var reqdata = { "searchtype": searchtype, "reportinventorydate": reportinventorydate };
    var jsonreqdata = JSON.stringify(reqdata);
    console.log(jsonreqdata);

    var request = $.ajax({
        type: 'POST',
        url: '/Report/SearchInventoryReportByCriteria',
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
});