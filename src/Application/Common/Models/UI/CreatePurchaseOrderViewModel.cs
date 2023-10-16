using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class CreatePurchaseOrderViewModel
{
    [Required(ErrorMessage = "กรุณาระบุหมายเหตุ(ถ้ามี)")]
    [DisplayName("หมายเหตุ")]
    public string? Remark { get; init; }

    public int npurchasetypeid { get; set; }

    public int nsupplierid { get; set; }

    public int ncurrencyid { get; set; }

    public int npaymenttypeid { get; set; }

    public decimal amount { get; set; }
    public decimal discount { get; set; }
    public decimal total { get; set; }
}
