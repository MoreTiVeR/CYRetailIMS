var dataTable;

$('.select2').select2();


function CreateEmployee(form) {

    $("#global-loader").css('display', '');
    var frmCreateEmployee = $("#frmCreateEmployee");
    frmCreateEmployee.validate();
    var isValid = frmCreateEmployee.valid();
    if (isValid) {
        console.log('Call => AddItem');
        $.validator.unobtrusive.parse(form);
        var data = $(form).serializeJSON();
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/EmployeeManagement/CreateEmployee',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.result) {
                    //popup.dialog('close');

                    AlertSuccess("เพิ่มข้อมูลพนักงานสำเร็จ");
                    $("#frmCreateEmployee")[0].reset();
                    $("#global-loader").css('display', 'none');

                    //To do next?
                    //window.location = data.url;
                }
                else {
                    AlertError(data.message);
                    $("#global-loader").css('display', 'none');
                }
            }
        });
        return false;
    }
    else {
        $("#global-loader").css('display', 'none');
    }
}