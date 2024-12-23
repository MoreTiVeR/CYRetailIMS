using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchForImportByBranchID.v1;
public record GetItemInBranchForImportByBranchIDQuery : IRequest<BaseResponse<List<GetItemInBranchForImportByBranchIDResponseDTO>>>
{
    public int branchid { get; init; }
}
