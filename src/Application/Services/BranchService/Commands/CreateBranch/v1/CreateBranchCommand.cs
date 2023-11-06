using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.BranchService.Commands.CreateBranch.v1;
public record CreateBranchCommand : IRequest<BaseResponse<CommandResponse>>
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    public string branchcode { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    public string branchname { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    public string address { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    public string createdby { get; init; }

    [Required(ErrorMessage = "Required field")]
    public DateTime createddate { get; init; }

    [Required(ErrorMessage = "Required field")]
    public bool isactive { get; init; }
}
