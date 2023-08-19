using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ItemService.Queries.GetItemByID.v1;

[Serializable]
public class GetItemByIDResponseDTO
{
    public int itemid { get; set; }

    public string itemcode { get; set; }

    public int itemtypeid { get; set; }

    public string itemtypename { get; set; }

    public int unitofmeasureid { get; set; }

    public string unitofmeasurename { get; set; }

    public int brandid { get; set; }

    public string brandname { get; set; }

    public string name { get; set; }

    public string shortname { get; set; }

    public string description { get; set; }

    public string barcode { get; set; }

    /// <summary>
    /// ราคาต้นทุน
    /// </summary>
    public decimal cost { get; set; }

    /// <summary>
    /// ราคาขาย
    /// </summary>
    public decimal price { get; set; }

    public string itemimageurl { get; set; }

    public int qty { get; set; }

    public double discountpercent { get; init; }

    public string createdby { get; set; }

    public DateTime createddate { get; set; }

    public string? updatedby { get; set; }

    public DateTime? updateddate { get; set; }

    public bool isactive { get; set; }

}
