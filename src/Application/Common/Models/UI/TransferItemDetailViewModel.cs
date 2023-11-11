using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CYRetailIMS.Application.Common.Models.UI;

[Serializable]
public class TransferItemDetailViewModel
{
    /// <summary>
    /// Seq
    /// </summary>
    public int nseq { get; set; }

    //[Required(ErrorMessage = "Required field")]
    public int nitemid { get; set; }

    public string? sitemname { get; set; }

    [Required(ErrorMessage = "Required field")]
    public string sbarcode { get; set; }

    private int _nqty { get; set; }

    [Required(ErrorMessage = "Required field")]
    public int nqty
    {
        get
        {
            if (_nqty == 0)
            {
                _nqty = 1;
            }
            return _nqty;
        }
        set
        {
            _nqty = value;
        }
    }

    //[Required(ErrorMessage = "*กรุณาระบุราคาสินค้า")]
    [DisplayName("ราคาสินค้า")]
    public decimal price { get; set; }

    //[Required(ErrorMessage = "*กรุณาระบุจำนวนเงินรวม")]
    [DisplayName("เงินรวม")]
    public decimal totalprice { get; set; }

}
