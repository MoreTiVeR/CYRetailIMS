using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.AccountService.Queries.Logout.v1;
public record LogoutQuery : IRequest<BaseResponse<CommandResponse>>
{
    public string username { get; init; }
}
