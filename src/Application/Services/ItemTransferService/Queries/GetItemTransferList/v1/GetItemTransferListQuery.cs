using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferList.v1;

[Serializable]
public record GetItemTransferListQuery : IRequest<BaseResponse<List<GetItemTransferResponseDTO>>> 
{
    public DateTime? transferdate { get; init; }
    public int? transferstatusid { get; init; }
    public int? branchid { get; init; }
}
