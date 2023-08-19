
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
            $.ajax({
                type: 'POST',
                url: '/Item/DeleteItem',
                data: JSON.stringify({ ItemID: itemid }),
                contentType: 'application/json',
                success: function (data) {
                    if (data.result) {
                        //popup.dialog('close');

                        //Alert
                        Swal.fire({
                            icon: 'success',
                            type: "success",
                            title: "ลบข้อมูลสำเร็จ!",
                            //text: "Your product has been deleted.",
                            confirmButtonClass: "btn btn-success",
                        });

                        $("#global-loader").css('display', 'none');
                        //ShowMessageSuccess(data.message);

                        //To do next?
                        //window.location = data.url;
                    }
                    else {
                        //ShowMessageError(data.message);
                        Swal.fire({
                            title: "ทำรายการไม่สำเร็จ!",
                            text: data.message,
                            type: "success",
                            confirmButtonClass: "btn btn-dander",
                            buttonsStyling: !1,
                        });
                        $("#global-loader").css('display', 'none');
                    }
                }
            });
        }
    });
}