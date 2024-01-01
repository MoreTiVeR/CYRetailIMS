using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByDestinationBranchID.v1;

[Serializable]
public record GetItemTransferByDestinationBranchIDQuery : IRequest<BaseResponse<List<GetItemTransferResponseDTO>>>
{
    public int destinationbranchid { get; init; }
    public DateTime? transferdate { get; init; }
    public int? transferstatusid { get; init; }
}
