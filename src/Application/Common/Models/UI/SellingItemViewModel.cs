using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace CYRetailIMS.Application.Common.Models.UI;
public class AddSaleItemViewModel
{
    public string ddlSearchItem { get; set; }
    public decimal txtItemPrice { get; set; }
    public int txtItemQty { get; set; }
    public decimal txtAmount { get; set; }
}

[Serializable]
[JsonObject]
public class SellingItemViewModel
{
    [Required(ErrorMessage = "* กรุณาระบุสาขา")]
    [Display(Name = "สาขา")]
    public string branch { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุวันที่ขาย")]
    [Display(Name = "วันที่ขาย")]
    public DateTime saledate { get; set; }
    public int qty { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุยอดเงินโอน")]
    [Display(Name = "เงินโอน")]
    public decimal mtransfer { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุยอดเงินฝากธนาคาร")]
    [Display(Name = "เงินฝาก")]
    public decimal mdeposit { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุยอดเงินสดคงเหลือ")]
    [Display(Name = "เงินสด")]
    public decimal mcash { get; set; }

    [Display(Name = "เงินรวม")]
    public decimal amount { get; set; }
}
