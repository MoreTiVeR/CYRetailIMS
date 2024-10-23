using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetDraftItemTransferByBranchID.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.GetDraftItemTransferByCriteria.v1;

public record GetDraftItemTransferByCriteriaQuery : IRequest<BaseResponse<List<GetDraftItemTransferByBranchIDResponseDTO>>>
{
    public DateTime transferdate { get; set; }
    public int? branchid { get; set; }
}
