using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItemList;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Commands.CreateItemInBranch.v1;

[Serializable]
public record CreateItemInBranchListCommand : IRequest<BaseResponse<CommandResponse>>
{
    [Required]
    public int branchid { get; init; }

    [Required]
    public List<CreateItemInBranchDetailCommand> items { get; init; }
}