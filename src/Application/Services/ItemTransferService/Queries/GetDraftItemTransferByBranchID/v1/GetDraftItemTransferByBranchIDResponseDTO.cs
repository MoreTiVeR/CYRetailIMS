using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.GetDraftItemTransferByBranchID.v1;
public class GetDraftItemTransferByBranchIDResponseDTO
{
    public int transferheaderid { get; set; }
    public string refno { get; set; }
    public int destinationbranchid { get; set; }
    public string destinationbranchname { get; set; }
    public string createdby { get; set; }
    public DateTime createddate { get; set; }
    public bool isactive { get; set; }
    public int transferstatus { get; set; }

    public List<GetDraftItemTransferDetailResponseDTO> detail { get; set; }
}
