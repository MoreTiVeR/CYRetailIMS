function ShowMessageInfo(msg) {
    console.log('Call => ShowMessage info => msg');
    toastr.info(msg);
}

function ShowMessageWarning(msg) {
    console.log('Call => ShowMessage warning => msg');
    toastr.warning(msg);
}

function ShowMessageSuccess(msg) {
    console.log('Call => ShowMessage success => msg');
    toastr.success(msg);
}

function ShowMessageError(msg) {
    console.log('Call => ShowMessage info => msg');
    toastr.error(msg);
}

function currencyFormat(num) {
    return num.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
}

function currencyFormatWithDigi(num, digi) {
    return num.toFixed(digi).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
}

function AlertSuccess(msg) {
    Swal.fire({
        title: "สำเร็จ!",
        html: "<span class='text-success'>" + msg + "</span>",
        icon: "success",
        type: "success",
        confirmButtonText: 'ปิดหน้าต่าง',
        confirmButtonClass: 'btn btn-primary',
        buttonsStyling: false,
    });
}

function AlertInfo(msg) {
    Swal.fire({
        title: "ข้อมูล!",
        html: "<span class='text-info'>" + msg + "</span>",
        icon: "info",
        type: "info",
        confirmButtonText: 'ปิดหน้าต่าง',
        confirmButtonClass: 'btn btn-primary',
        buttonsStyling: false,
    });
}

function AlertWarn(msg) {
    Swal.fire({
        title: "คำเตือน!",
        html: "<span class='text-warning'>" + msg + "</span>",
        icon: "warning",
        type: "warning",
        confirmButtonText: 'ปิดหน้าต่าง',
        confirmButtonClass: 'btn btn-primary',
        buttonsStyling: false,
    });
}

function AlertError(msg) {
    Swal.fire({
        title: "เกิดข้อผิดพลาด!",
        html: "<span class='text-danger'>" + msg + "</span>",
        icon: "error",
        type: "error",
        confirmButtonText: 'ปิดหน้าต่าง',
        confirmButtonClass: 'btn btn-primary',
        buttonsStyling: false,
    });
}
