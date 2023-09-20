using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.BranchService.Commands.DeleteBranch.v1;

[Serializable]
public record DeleteBranchCommand : IRequest<BaseResponse<CommandResponse>>
{
    [Required]
    public int branhid { get; init; }

    [Required]
    public string updatedby { get; init; }
}
