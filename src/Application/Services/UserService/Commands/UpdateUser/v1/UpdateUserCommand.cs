using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.UserService.Commands.UpdateUser.v1;

[Serializable]
public record UpdateUserCommand : IRequest<BaseResponse<CommandResponse>>
{
    [Required(ErrorMessage = "Required field")]
    public int userid { get; set; }

    public string profilepicture { get; set; }

    [Required(ErrorMessage = "Required field")]
    public int roleid { get; init; }

    [Required(ErrorMessage = "Required field")]
    public int userinbranchid { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    public string updatedby { get; init; }

    [Required(ErrorMessage = "Required field")]
    public DateTime updateddate { get; init; }

    public bool isactive { get; set; }


}
