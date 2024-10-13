var datepicker;
//$(document).ready(function () {


//});

function formatDateTime(date) {
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Months are zero-based
    const year = date.getFullYear();
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');

    return `${day}/${month}/${year} ${hours}:${minutes}:${seconds}`;
}

function formatDate(date) {
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Months are zero-based
    const year = date.getFullYear();

    return `${day}/${month}/${year}`;
}

function formatDateDDMMYYYY(date) {
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Months are zero-based
    const year = date.getFullYear();

    return `${day}${month}${year}`;
}

function InitialDatePicker() {
    var now = new Date();
    var $input = $('.pickadate-saledate').pickadate({
        selectYears: true,
        selectMonths: true,
        language: 'th-th',
        format: 'dd/mm/yyyy',
        formatSubmit: 'dd/mm/yyyy',
        monthsFull: ['มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม'],
        monthsShort: ['ม.ค.', 'ก.พ.', 'มี.ค.', 'เม.ย.', 'พ.ค.', 'มิ.ย.', 'ก.ค.', 'ส.ค.', 'ก.ย.', 'พ.ย.', 'พ.ย.', 'ธ.ค.'],
        weekdaysShort: ['อา', 'จ', 'ค', 'พ', 'พฤ', 'ศ', 'ส'],
        today: 'วันนี้',
        clear: 'ล้างค่า',
        close: 'ปิด',
        onSet: function (event) {
            var $input = $('#date-fin').pickadate();
            var picker = $input.pickadate('picker');
            var tempDate = new Date(event.select);
            //picker.set('select', tempDate.setDate(tempDate.getDate() + 7));
            //picker.set('min', new Date(event.select));
        }
    });
    datepicker = $input.pickadate('picker');
    datepicker.set('select', new Date())
}

function InitialDatePickerWithoutSetCurrentData() {
    var now = new Date();
    var $input = $('.pickadate-saledate').pickadate({
        selectYears: true,
        selectMonths: true,
        language: 'th-th',
        format: 'dd/mm/yyyy',
        formatSubmit: 'dd/mm/yyyy',
        monthsFull: ['มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม'],
        monthsShort: ['ม.ค.', 'ก.พ.', 'มี.ค.', 'เม.ย.', 'พ.ค.', 'มิ.ย.', 'ก.ค.', 'ส.ค.', 'ก.ย.', 'พ.ย.', 'พ.ย.', 'ธ.ค.'],
        weekdaysShort: ['อา', 'จ', 'ค', 'พ', 'พฤ', 'ศ', 'ส'],
        today: 'วันนี้',
        clear: 'ล้างค่า',
        close: 'ปิด',
        onSet: function (event) {
            var $input = $('#date-fin').pickadate();
            var picker = $input.pickadate('picker');
            var tempDate = new Date(event.select);
            //picker.set('select', tempDate.setDate(tempDate.getDate() + 7));
            //picker.set('min', new Date(event.select));
        }
    });
    datepicker = $input.pickadate('picker');
    //datepicker.set('select', new Date())
}

function InitialEditDatePicker() {
    var now = new Date();
    var $input = $('.pickadate-saledate').pickadate({
        selectYears: true,
        selectMonths: true,
        language: 'th-th',
        format: 'dd/mm/yyyy',
        formatSubmit: 'dd/mm/yyyy',
        monthsFull: ['มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม'],
        monthsShort: ['ม.ค.', 'ก.พ.', 'มี.ค.', 'เม.ย.', 'พ.ค.', 'มิ.ย.', 'ก.ค.', 'ส.ค.', 'ก.ย.', 'พ.ย.', 'พ.ย.', 'ธ.ค.'],
        weekdaysShort: ['อา', 'จ', 'ค', 'พ', 'พฤ', 'ศ', 'ส'],
        today: 'วันนี้',
        clear: 'ล้างค่า',
        close: 'ปิด',
        onSet: function (event) {
            var $input = $('#date-fin').pickadate();
            var picker = $input.pickadate('picker');
            var tempDate = new Date(event.select);
            //picker.set('select', tempDate.setDate(tempDate.getDate() + 7));
            //picker.set('min', new Date(event.select));
        }
    });
    datepicker = $input.pickadate('picker');
    /*datepicker.set('select', new Date())*/
}

function InitialTransactionDate() {
    $("#txtTransactionDate").val(formatDateTime(new Date($("#txtTransactionDate").val())));
}

function InitialNumberInput() {
    $(".allownumericwithdecimal").on("keypress keyup blur", function (event) {
        //this.value = this.value.replace(/[^0-9\.]/g,'');
        $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });

    $(".allownumericwithoutdecimal").on("keypress keyup blur", function (event) {
        $(this).val($(this).val().replace(/[^\d].+/, ""));
        if ((event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });
}

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
    return num.toFixed(2).replace(/(\d)(?=(\d{10})+(?!\d))/g, '$1,');
}

function currencyFormatWithDigi(num, digi) {
    return num.toFixed(digi).replace(/(\d)(?=(\d{10})+(?!\d))/g, '$1,');
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

function AlertErrorNoTitle(msg) {
    Swal.fire({
        html: "<span class='text-danger'>" + msg + "</span>",
        icon: "error",
        type: "error",
        confirmButtonText: 'ปิดหน้าต่าง',
        confirmButtonClass: 'btn btn-primary',
        buttonsStyling: false,
    });
}

function InitialCharacterRemaining() {
    $('textarea').charactersRemaining();
    $('textarea').charactersRemaining({
        singleCharacterText: '## จำนวนตัวอักษรที่พิมพ์ได้',
        multipleCharacterText: '## จำนวนตัวอักษรที่พิมพ์ได้'
    });
}

function ShowLoading() {
    $("#global-loader").css('display', '');
}

function HideLoading() {
    $("#global-loader").css('display', 'none');
}