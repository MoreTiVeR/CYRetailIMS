using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;

[Serializable]
public record GetItemInBranchByBranchIDQuery : IRequest<BaseResponse<GetItemInBranchByBranchIDResponseDTO>>
{
    [Required(ErrorMessage = "Branch id is required")]
    public int branchid { get; init; }
}
