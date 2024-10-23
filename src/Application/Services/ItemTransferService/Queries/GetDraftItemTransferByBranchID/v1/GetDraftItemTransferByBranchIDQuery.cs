using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.GetDraftItemTransferByBranchID.v1;
public record GetDraftItemTransferByBranchIDQuery: IRequest<BaseResponse<GetDraftItemTransferByBranchIDResponseDTO>>
{
    public int branchid { get; init; }
}
