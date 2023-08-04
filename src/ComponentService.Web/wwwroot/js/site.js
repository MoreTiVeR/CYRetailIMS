
function ShowMessage(msg) {
    console.log('Call => ShowMessage success => msg');
    toastr.success(msg);
}

function ShowMessageError(msg) {
    console.log('Call => ShowMessage info => msg');
    toastr.error(msg);
}
