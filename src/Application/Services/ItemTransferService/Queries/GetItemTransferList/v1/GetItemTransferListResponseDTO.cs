using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferList.v1;
public class GetItemTransferListResponseDTO
{
    public int totalrow { get; set; }
    public List<GetItemTransferResponseDTO> transactiondata { get; set; }
}
