using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInventoryForTransferByBranchID.v1;

[Serializable]
public class GetItemInventoryTransferResposeDTO
{
    public int branchid { get; set; }
    public string branchname { get; set; }
    public int itemid { get; set; }
    public string itemcode { get; set; }
    public string itemname { get; set; }
    public int brandid { get; set; }
    //public string brandname { get; set; }
    public int qtyinstock { get; set; }
    public int qtyinbranch { get; set; }
    public int notifyminqty { get; set; }

    private int _orderqty { get; set; }
    public int orderqty
    {
        get
        {
            int numQty = this.notifyminqty - this.qtyinbranch;
            return numQty < 0 ? 0 : numQty;
        }
        set
        {
            value = this._orderqty;
        }
    }
    public int refillqty { get; set; }
}
