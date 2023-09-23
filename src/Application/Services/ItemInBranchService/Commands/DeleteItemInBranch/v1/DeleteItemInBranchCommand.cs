using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Commands.DeleteItemInBranch.v1;
public record DeleteItemInBranchCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int itemid { get; init; }
    public int branchid { get; init; }
    public string updatedby { get; init; }
    public DateTime updateddate { get; init; }
}
