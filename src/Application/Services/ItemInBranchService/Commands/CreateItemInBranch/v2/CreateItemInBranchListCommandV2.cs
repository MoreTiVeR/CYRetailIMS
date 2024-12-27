using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemInBranchService.Commands.CreateItemInBranch.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Commands.CreateItemInBranch.v2;

public record CreateItemInBranchListCommandV2 : IRequest<BaseResponse<CommandResponse>>
{
    [Required]
    public int branchid { get; init; }

    [Required]
    public List<CreateItemInBranchDetailCommandV2> items { get; init; }
}