using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;

public class CreateAdjustItemViewModel
{
    //[Required(ErrorMessage = "*กรุณาระบุประเภทการปรับสต๊อก")]
    //[DisplayName("ประเภทการปรับสต๊อก")]
    //public int AdjustTypeID { get; init; }

    //[Required(ErrorMessage = "*กรุณาระบุสินค้า")]
    //[DisplayName("สินค้า")]
    //public int ItemID { get; init; }

    //[Required(ErrorMessage = "*กรุณาระบุจำนวนที่ต้องการปรับเพิ่ม/ลดสต๊อก")]
    //[DisplayName("จำนวนปรับเพิ่ม/ลดสต๊อก")]
    //public int Qty { get; init; }

    [Required(ErrorMessage = "*กรุณาระบุหมายเหตุที่ปรับสต๊อก")]
    [DisplayName("หมายเหตุ")]
    public string Remark { get; init; }
}


public class AdjustItemViewModel
{
    public int nseq { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุประเภทการปรับสต๊อก")]
    [DisplayName("ประเภทการปรับสต๊อก")]
    public int nadjusttypeid { get; set; }

    public string? sadjusttypename { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุสินค้า")]
    [DisplayName("สินค้า")]
    public int nitemid { get; set; }

    public string? sitemname { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุจำนวนที่ต้องการปรับเพิ่ม/ลดสต๊อก")]
    [DisplayName("จำนวนปรับเพิ่ม/ลดสต๊อก")]
    public int nqty { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุสาขา")]
    [DisplayName("สาขา")]
    public int nbranchid { get; set; }

    public string? sbranchname { get; set; }
}
