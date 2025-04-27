using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class SellingBarcodeItemViewModel
{
    public int seq { get; set; }
    public int branchid { get; set; }
    public string? barcode { get; set; }
    public int itemid { get; set; }
    public string? itemname { get; set; }
    public decimal itemprice { get; set; }

    private int _qty { get; set; }
    public int qty
    {
        get
        {
            return _qty > 0 ? _qty : 1;
        }
        set
        {
            _qty = value;
        }
    }


    private decimal _price { get; set; }
    [DisplayName("เงินรวม")]
    public decimal totalprice
    {
        get
        {
            _price = qty * itemprice;
            return _price;
        }
        set
        {
            value = _price;
        }
    }
}
