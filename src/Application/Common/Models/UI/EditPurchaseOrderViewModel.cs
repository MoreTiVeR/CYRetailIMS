using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class EditPurchaseOrderViewModel
{
    [Required(ErrorMessage = "กรุณาระบุหมายเลขสั่งสินค้า")]
    [DisplayName("หมายเลขสั่งสินค้า")]
    public int purchaseorderid { get; set; }


    //[Required(ErrorMessage = "กรุณาระบุหมายเหตุ(ถ้ามี)")]
    [DisplayName("หมายเหตุ")]
    public string? Remark { get; init; }

    [Required(ErrorMessage = "กรุณาระบุหมายเหตุ(ถ้ามี)")]
    [DisplayName("หมายเหตุ")]
    public int npurchasetypeid { get; set; }


    [Required(ErrorMessage = "กรุณาระบุซัฟพลายเออร์")]
    [DisplayName("ซัฟพลายเออร์")]
    public int nsupplierid { get; set; }

    [Required(ErrorMessage = "กรุณาระบุสกุลเงินที่จ่าย")]
    [DisplayName("สกุลเงินที่จ่าย")]
    public int ncurrencyid { get; set; }

    [Required(ErrorMessage = "กรุณาระบุประเภทการจ่ายเงิน")]
    [DisplayName("ประเภทการจ่ายเงิน")]
    public int npaymenttypeid { get; set; }

    [Required(ErrorMessage = "กรุณาระบุจำนวนเงิน")]
    [DisplayName("จำนวนเงิน")]
    public decimal amount { get; set; }

    [DisplayName("ส่วนลด")]
    public decimal discount { get; set; }

    [Required(ErrorMessage = "กรุณาระบุเงินรวมทั้งหมด")]
    [DisplayName("เงินรวมทั้งหมด")]
    public decimal total { get; set; }

    public string? trackingno { get; set; }

    public string? createdby { get; set; }
    public DateTime? createddate { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุสถานะส่งสินค้า")]
    [Display(Name = "สถานะส่งสินค้า")]
    public int approvestatus { get; set; }
}
