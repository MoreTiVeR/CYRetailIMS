using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.GetDraftItemTransferByBranchID.v1;
public class GetDraftItemTransferDetailResponseDTO
{
    public int transferdetailid { get; set; }
    public int itemid { get; set; }
    public string itemname { get; set; }
    public int qty { get; set; }
}
