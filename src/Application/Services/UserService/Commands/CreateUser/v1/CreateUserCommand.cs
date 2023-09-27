using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.UserService.Commands.CreateUser.v1;
public record CreateUserCommand : IRequest<BaseResponse<CommandResponse>>
{
    [Required(ErrorMessage = "Required field")]
    public int empid { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(20, ErrorMessage = "Maximum length 20")]
    public string username { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(100, ErrorMessage = "Maximum length 50")]
    public string password { get; init; }

    public string profilepicture { get; set; }

    [Required(ErrorMessage = "Required field")]
    public int roleid { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    public string createdby { get; init; }

    [Required(ErrorMessage = "Required field")]
    public DateTime creadeddate { get; init; }

    [Required(ErrorMessage = "Required field")]
    public int userinbranchid { get; set; }

    [Required(ErrorMessage = "Required field")]
    public int approvestatus { get; set; }
}
