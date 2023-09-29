using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CYRetailIMS.Application.Common.Models.UI;

[Serializable]
[JsonObject]
public class EditItemViewModel
{
    [ReadOnly(true)]
    [Required(ErrorMessage = "*กรุณาระบุไอดีสินค้า")]
    [DisplayName("ระบุไอดีสินค้า")]
    public int ItemID { get; set; }

    [ReadOnly(true)]
    [Required(ErrorMessage = "*กรุณาระบุรหัสสินค้า")]
    //[RegularExpression("^[A-Za-z0-9].{4,20}$", ErrorMessage = "รูปแบบชื่อผู้ใช้งานไม่ถูกต้อง, ตัวอักษรภาษาอังกฤาและตัวเลข ความยาวไม่เกิน20ตัวอักษร")]
    [DisplayName("ระบุรหัสสินค้า")]
    public string ItemCode { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุประเภทสินค้า")]
    //[RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "รุปแบบรหัสผ่านไม่ถูกต้อง, รหัสผ่านประกอบด้วยตัวพิมพ์ใหญ่ ตัวพิมพ์เล็ก ตัวเลข และ อัขระพิเศษ #?!@$%^&*- ความยาวขั้นต่ำ 8 ตัวอักษร")]
    //[RegularExpression("^[A-Za-z0-9#?!@$%^&*-].{8,20}$", ErrorMessage = "รุปแบบรหัสผ่านไม่ถูกต้อง, รหัสผ่านประกอบด้วยตัวพิมพ์ใหญ่ ตัวพิมพ์เล็ก ตัวเลข และ อัขระพิเศษ #?!@$%^&*- ความยาวขั้นต่ำ 8 ตัวอักษร")]
    [DisplayName("ระบุประเภทสินค้า")]
    public int ItemTypeID { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุหน่วยนับสินค้า")]
    [DisplayName("ระบุหน่วยนับสินค้า")]
    public int UnitOfMeasureID => 1;

    [ReadOnly(true)]
    [Required(ErrorMessage = "*กรุณาระบุแบรนด์สินค้า")]
    [DisplayName("ระบุแบรนด์สินค้า")]
    public int BrandID { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุชื่อสินค้า")]
    [DisplayName("ระบุชื่อสินค้า")]
    public string Name { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุราคาสินค้า")]
    [DisplayName("ระบุชื่อราคาสินค้า")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุต้นทุนสินค้า")]
    [DisplayName("ระบุต้นทุนสินค้า")]
    public decimal Cost { get; set; }

    //[Required(ErrorMessage = "*กรุณาระบุชื่อย่อสินค้า")]
    [DisplayName("ระบุชื่อย่อสินค้า")]
    public string ShortName { get; set; }

    //[Required(ErrorMessage = "*กรุณาระบุชื่อย่อสินค้า")]
    [DisplayName("ระบุรายละเอียดสินค้า")]
    public string Description { get; set; }

    //[Required(ErrorMessage = "*กรุณาระบุชื่อย่อสินค้า")]
    [DisplayName("ระบุบาร์โค้ดสินค้า")]
    public string BarCode { get; set; }

    //[Required(ErrorMessage = "*กรุณาระบุชื่อย่อสินค้า")]
    public string ItemImageUrl { get; set; }

    //[Required(ErrorMessage = "*กรุณาระบุจำนวนหน่วยสินค้า")]
    [DisplayName("ระบุจำนวนหน่วยสินค้า")]
    public int Qty { get; set; }

    [DisplayName("ระบุจำนวนสินค้าขั้นต่ำ")]
    public int NotifyMinQty { get; set; }

    //[Required(ErrorMessage = "*กรุณาระบุจำนวนหน่วยสินค้า")]
    [DisplayName("ระบุส่วนลดสินค้า")]
    public double DiscountPercent { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุสถานะสินค้า")]
    [DisplayName("ระบุสถานะสินค้า")]
    public string IsActive { get; set; }

}
