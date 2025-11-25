var datatable;
$('.select2').select2();

datatable = $("#tbSaleTransaction").DataTable({
    "processing": true,         // Show processing indicator
    "serverSide": true,        // Enable server-side processing
    "destroy": true,
    "bFilter": true,
    "stateSave": true,
    "sDom": '<"top"f>rt<"bottom"lpi><"clear">',
    "pagingType": 'numbers',
    "ordering": true,
    "ajax": {
        "url": "/Sale/SearchSaleTransaction",
        "type": "POST",
        "contentType": "application/json", // Add this line
        "data": function (data) {
            data.startdate = $("#txtStartDate").val();
            data.enddate = $("#txtEndDate").val();
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
        {
            "data": { transactionid: "transactionid" },
            "render": function (data) {
                return "<div class='text-center'><a class='action-set' href='javascript:void(0);' data-bs-toggle='dropdown' aria-expanded='true'><i class='fa fa-ellipsis-v' aria-hidden='true'></i></a>"
                    + "<ul class='dropdown-menu'><li><a asp-action='Edit' asp-controller='Transactions' title='แก้ไขข้อมูลขาย' asp-all-route-data='aTransactionID' class='dropdown-item' href='/Transactions/Edit?tranid=" + data.transactionid + "'><img src='../assets/img/icons/edit.svg' class='me-2' alt='img'>แก้ไขข้อมูล</a></li>"
                    + "<li><a href='#' id='rowid" + data.transactionid + "' class='dropdown-item' onclick='deleteTransaction(" + data.transactionid + ")'><img src='../assets/img/icons/delete1.svg' class='me-2' alt='img'>ลบข้อมูล</a></li></div>";
            }
            //"data": { transactionid: "transactionid" },
            //"render": function (data) {
            //    return "<a href='/Transactions/Edit?tranid=" + data.transactionid + "' asp-all-route-data='aTransactionID' class='me-3' title='แก้ไขข้อมูลใบเสร็จ'><img src='../assets/img/icons/edit.svg' alt='img'></a>";

            //}
        },
        {
            "data": { transactiondate: "transactiondate" },
            "render": function (data) {
                if (data.transactiondate === null || data.transactiondate == null) {
                    return data.transactiondate;
                }
                return formatDateTime(new Date(data.transactiondate));
            }
        },
        {
            "data": { branchname: "branchname" },
            "render": function (data) {
                return "<span class='badges bg-lightgreen'>" + data.branchname + "</span>"
            }
        },
        {
            "data": { transactiontypeid: "transactiontypeid" },
            "render": function (data) {
                if (data.transactiontypeid === 1 || data.transactiontypeid == 1) {
                    return "<span class='badges bg-orange'>" + data.transactiontypedesc +"</span>";
                }
                else if (data.transactiontypeid === 3 || data.transactiontypeid == 3) {
                    return "<span class='badges bg-lightpurple'>" + data.transactiontypedesc + "</span>";
                }
                else if (data.transactiontypeid === 4 || data.transactiontypeid == 4) {
                    return "<span class='badges bg-lightbule'>" + data.transactiontypedesc + "</span>";
                }
                else {
                    return "<span class='badges bg-lightyellow'>N/A</span>";
                }
            }
        },
        { "data": "totalamount" },
        { "data": "amounttransfer" },
        { "data": "amountdeposit" },
        { "data": "amountcash" },
        { "data": "remark" },
        { "data": "createdbystaff" }
    ],
    "order": [[0, "desc"]],
    "columnDefs": [
        {
            "targets": [0],
            "visible": false
        },
        {
            "targets": 1, // index of receivetempid column
            "className": "text-center"
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
        $('.dataTables_filter').appendTo("#tbSaleTransaction");
        $('.dataTables_filter').appendTo('.search-input');

        const totalcash = json.totalcash ?? 0;
        const totaltransfer = json.totaltransfer ?? 0;

        // อัปเดต attribute data-count ด้วย (สำหรับใช้ animate counter)
        //$('#hTotalCash').attr('data-count', totalcash);
        //$('#hTotalTransfer').attr('data-count', totaltransfer);

        //$('#hTotalCash').text(totalcash.toLocaleString());
        //$('#hTotalTransfer').text(totaltransfer.toLocaleString());
        
    }
});

// ✅ เมื่อ DataTable ดึงข้อมูลเสร็จ (ทุกครั้งที่ search / paging / reload)
$('#tbSaleTransaction').on('xhr.dt', function (e, settings, json, xhr) {

    // ป้องกัน null หรือ summary หาย
    const totalcash = json?.totalcash ?? 0;
    const totaltransfer = json?.totaltransfer ?? 0;
    const totaldepositfee = json?.totaldepositfee ?? 0;

    // อัปเดต attribute และ text
    $('#hTotalCash').attr('data-count', totalcash);
    $('#hTotalMoneyTransfer').attr('data-count', totaltransfer);
    $('#hTotalDepositFee').attr('data-count', totaldepositfee);

    // เริ่ม counter animation
    $('.counters').each(function () {
        const $this = $(this);
        const target = parseFloat($this.attr('data-count')) || 0;
        const current = parseFloat($this.text().replace(/,/g, '')) || 0;

        // ถ้ามี animation เดิมอยู่ให้หยุดก่อน
        $this.stop(true, true);

        $({ value: current }).animate(
            { value: target },
            {
                duration: 1000,
                easing: 'swing',
                step: function (now) {
                    $this.text(Math.ceil(now).toLocaleString());
                },
                complete: function () {
                    $this.text(target.toLocaleString());
                }
            }
        );
    });
});


$("#btnSearch").on('click', function (event) {
    ShowLoading();
    event.preventDefault(); // Prevent the default form submission
    datatable.ajax.reload(); // This will use the updated parameters automatically
    HideLoading();
});

function deleteTransaction(tranid) {
    //AlertError("ไม่สามารถลบรายการได้ชั่วคราว! <br> กรุณาติดต่อผู้ดูแลระบบ.");
    //return;
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
                url: '/Transactions/DeleteTransaction',
                data: JSON.stringify({ transactionid: tranid }),
                contentType: 'application/json',
                success: function (data) {
                    if (data.result) {

                        AlertSuccess(data.message);
                        HideLoading();
                        //ShowMessageSuccess(data.message);

                        //To do next?
                        //window.location = data.url;
                        //itemDataTable.row('.selected').remove().draw(false);
                        //dataTable.ajax.reload();
                        /*$("#tbItems").DataTable().ajax.reload();*/
                        /* $('#tbItems').DataTable().ajax.reload();*/
                        //$('#tbItems').DataTable().ajax.reload();

                        console.log("#rowid" + tranid);
                        $("#rowid" + tranid).closest("tr").remove();
                        $('#tbItems').DataTable().ajax.reload();

                        //$("#rowid" + tranid).closest("tr").remove().draw(false);
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