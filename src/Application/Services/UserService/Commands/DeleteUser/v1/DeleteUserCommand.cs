using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.UserService.Commands.DeleteUser.v1;
public record DeleteUserCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int userid { get; init; }
    public string updatedby { get; init; }
}
