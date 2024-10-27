using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInventoryForTransferByBranchID.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInventoryForTransferByDraftID.v1;

[Serializable]
public record GetItemInventoryForTransferByDraftIDQuery : IRequest<BaseResponse<List<GetItemInventoryTransferResposeDTO>>>
{
    public int draftid { get; init; }
}
