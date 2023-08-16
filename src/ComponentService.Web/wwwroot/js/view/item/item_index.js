
$(document).ready(function () {

});

function deleteItem(itemid) {

    Swal.fire({
        title: "ยืนยันการลบข้อมูล?",
        text: "เมื่อลบข้อมูลแล้ว จะไม่สามารถทำการยกเลิกได้!",
        icon: 'warning',
        type: "warning",
        showCancelButton: !0,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "ยืนยัน",
        confirmButtonClass: "btn btn-primary",
        cancelButtonText: "ยกเลิก",
        cancelButtonClass: "btn btn-danger ml-1",
        buttonsStyling: !1,
    }).then(function (t) {
        if (t.value) {

            //Delete

            //Alert
            Swal.fire({
                icon: 'success',
                type: "success",
                title: "ลบข้อมูลสำเร็จ!",
                //text: "Your product has been deleted.",
                confirmButtonClass: "btn btn-success",
            });
        }
    });
}