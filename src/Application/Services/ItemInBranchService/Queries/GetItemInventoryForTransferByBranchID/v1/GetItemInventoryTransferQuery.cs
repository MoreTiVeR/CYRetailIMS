using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInventoryForTransferByBranchID.v1;

[Serializable]
public record GetItemInventoryTransferQuery : IRequest<BaseResponse<List<GetItemInventoryTransferResposeDTO>>>
{
    public int branchid { get; init; }
    public int? brandid { get; init; }
}
