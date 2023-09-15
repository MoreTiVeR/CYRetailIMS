using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByCriteria.v1;

public record GetItemInBranchByCriteriaQuery : IRequest<BaseResponse<GetItemInBranchByCriteriaResponseDTO>>
{
    public int itemid { get; init; }

    public int branchid { get; init; }
}
