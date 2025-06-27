using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.BranchService.Commands.UpdateBranch.v1;

[Serializable]
public record UpdateBranchCommand : IRequest<BaseResponse<CommandResponse>>
{
    [Required(ErrorMessage = "* กรุณาระบุสาขา")]
    public int branhid { get; init; }

    [Required(ErrorMessage = "* กรุณาระบุเชื่อสาขา")]
    public string branchcode { get; init; }

    [Required(ErrorMessage = "* กรุณาระบุเชื่อสาขา")]
    public string branchname { get; init; }

    [Required(ErrorMessage = "* กรุณาระบุที่อยู่สาขา")]
    public string address { get; init; }

    [Required]
    public string updatedby { get; init; }

    public bool isactive { get; set; }
}
