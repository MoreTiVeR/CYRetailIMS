var dataTable;

function deleteEmp(id) {

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
                url: '/EmployeeManagement/DeleteEmployee',
                data: JSON.stringify({ empid: id }),
                contentType: 'application/json',
                success: function (data) {
                    if (data.result) {

                        AlertSuccess(data.message);
                        $("#global-loader").css('display', 'none');

                        console.log("#rowid" + id);
                        $("#rowid" + id).closest("tr").remove();
                        /*$('#tbEmployees').DataTable().ajax.reload();*/
                    }
                    else {
                        //ShowMessageError(data.message);
                        AlertError(data.message);
                        $("#global-loader").css('display', 'none');
                    }
                }
            });
        }
    });
}