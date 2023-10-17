using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class PurchaseOrderItemViewModel
{
    public int nseq { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุสินค้า")]
    [DisplayName("สินค้า")]
    public int nitemid { get; set; }
    public string? sitemname { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุจำนวนสั่งซื่อ")]
    [DisplayName("จำนวนสั่งซื้อ")]
    public int nqty { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุราคาสินค้า")]
    [DisplayName("ราคาสินค้า")]
    public decimal price { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุจำนวนเงินรวม")]
    [DisplayName("เงินรวม")]
    public decimal amount { get; set; }
}
