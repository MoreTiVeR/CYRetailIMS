
$('.ddl-ddlBranch').select2();
InitialDatePicker();
InitialNumberInput();
InitialCharacterRemaining();

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

$("#btnSave").on("click", function (e) {
    e.preventDefault();

    var frmSelling = $("#frmSelling");
    frmSelling.validate();
    var isValid = frmSelling.valid();
    if (isValid) {
        $.validator.unobtrusive.parse(frmSelling);
        var data = $(frmSelling).serializeJSON();

        // Access the role ID from the userProfile object in JavaScript
        var userRoleId = document.getElementById("uRoleID").value;

        // Now you can use userRoleId in your JavaScript code
        if (userRoleId == 2) {
            //Role Sale
            CreateSellingTransactionDataBySale(data);
        }
        else {
            //Admin or Other
            CreateSellingTrasnactionDataByAdmin(data);
        }
        
    }
    
    
});
function OnSuccess(data) {
    if (data.result) {
        ShowMessageSuccess(data.msg);
        AlertSuccess(data.msg);
        $("#txtSummaryTHB").val(0);
        ResetForm();
    }
    else {
        ShowMessageError(data.msg);
    }
}

function ResetForm() {
    $('#frmSelling')[0].reset(); // [0] gets the DOM element from the jQuery object
    dataTable.clear().draw(); // Clear the table and redraw
    InitialDatePicker();
}

function ValidateSellingBranchSelection() {

    //Validate tranfer type, from-branch, to-branch
    var sellingBranch = $('.ddl-ddlBranch').val();

    // Validate the selected value
    if (sellingBranch === undefined || sellingBranch === null || sellingBranch <= 0) {
        return false;
    }
    return true; // Return true if all validations pass
}

function CreateSellingTrasnactionDataByAdmin(data) {
    Swal.fire({
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
        focusConfirm: true,
        didOpen: function () {
            //Initial
        },
        reverseButtons: true // this will put Cancel on left, Confirm on right
    }).then(function (result) {

        if (result.isConfirmed) {

            //Prepare Request Data and Direct create transaction if is admin
            data.qty = 1;
            data = JSON.stringify(data);
            CreateData(data);
        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            //Code
            ShowMessageInfo('ยกเลิก');
        }
    });
}

function CreateSellingTransactionDataBySale(data) {
    Swal.fire({
        title: '<u><strong>เลือกประเภทการจ่ายเงิน</strong></u>',
        showCancelButton: true,
        confirmButtonColor: '#04B431',
        confirmButtonText: '<i class="fa fa-money-bill" aria-hidden="true"></i> เงินสด',
        cancelButtonColor: '#D33',
        cancelButtonText: '<i class="fa fa-university" aria-hidden="true"></i> เงินโอน',
        customClass: {
            confirmButton: 'btn-lg btn-icon btn-primary',
            denyButton: 'btn btn-warning ml-1',
            cancelButton: 'btn-lg btn-icon btn-success'
        },
        buttonsStyling: false,
        focusConfirm: true,
        didOpen: function () {
            //Initial other
        }
    }).then(function (result) {

        if (result.isConfirmed) {
            //เงินสด
            data.qty = 1;
            data.transactiontype = 3;
            data.iscash = true;
            data.version = "2";
            data = JSON.stringify(data);
            //CreateData(data);
            //ShowPrintReceipt(data);

            (async () => {
                try {
                    const result = await ShowPrintReceipt(data);
                    console.log("✅ Printing finished:", result);
                } catch (err) {
                    console.error("❌ Printing failed:", err.message);
                }
            })();
            // Remove the event listener after printing
            //document.getElementById('btnPrintReceipt').removeEventListener('click', ShowPrintReceipt);
        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            //เงินโอน
            data.qty = 1;
            data.transactiontype = 3;
            data.iscash = false;
            data.version = "2";
            data = JSON.stringify(data);
            //CreateData(data);
            //ShowPrintReceipt(data);
            (async () => {
                try {
                    const result = await ShowPrintReceipt(data);
                    console.log("✅ Printing finished:", result);
                } catch (err) {
                    console.error("❌ Printing failed:", err.message);
                }
            })();
            // Remove the event listener after printing
            //document.getElementById('btnPrintReceipt').removeEventListener('click', ShowPrintReceipt);
        }
    });
}

function CreateData(objData) {
    
    $.ajax({
        type: 'POST',
        url: '/Sale/SaveSellingItemByBarcode',
        data: objData,
        contentType: 'application/json',
        success: function (data) {
            if (data.result) {

                ShowMessageSuccess(data.msg);
                dataTable.ajax.reload();

                AlertSuccess(data.msg);
                $("#txtSummaryTHB").val(0);
                ResetForm();
            }
            else {
                AlertError(data.msg);
            }
        }
    });
}


async function CreateDataAsync(objData) {
    try
    {
        const response = await $.ajax({
            type: 'POST',
            url: '/Sale/SaveSellingItemByBarcode',
            data: objData, // stringify ถ้าเป็น object
            contentType: 'application/json'
        });

        if (response.result) {
            ShowMessageSuccess(response.msg);
            dataTable.ajax.reload();

            AlertSuccess(response.msg);
            $("#txtSummaryTHB").val(0);
            ResetForm();
        } else {
            ShowMessageError(response.msg);
        }
        return response; // important: return response ไปให้ await ใช้งาน
    } catch (error) {
        ShowMessageError(error.statusText || 'เกิดข้อผิดพลาด');
        throw error; // ส่ง error กลับไปเพื่อให้ .catch ใช้ได้
    }
}
