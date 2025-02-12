using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class CountStockUpdateModel
{
    public string ItemTypeCode { get; set; }
    public string SubItemCode { get; set; }
    public int ItemId { get; set; }
    public int StoreStock { get; set; }
    public int CountedQty { get; set; }
    public int WaitingToRestock { get; set; }
    public int Damaged { get; set; }
    public int SoldBeforeCount { get; set; }
    public int TotalCounted { get; set; }
    public int Difference { get; set; }
}