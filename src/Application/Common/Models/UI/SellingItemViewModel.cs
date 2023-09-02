using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class AddSaleItemViewModel
{
    public string ddlSearchItem { get; set; }
    public decimal txtItemPrice { get; set; }
    public int txtItemQty { get; set; }
    public decimal txtAmount { get; set; }
}

public class SellingItemViewModel
{
    public string branch { get; set; }
    public DateTime saledate { get; set; }
    public int qty { get; set; }
    public decimal amount { get; set; }
    public decimal mtransfer { get; set; }
}
