using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.UserService.Queries.GetUser.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.UserService.Queries.GetUserByID.v1;

[Serializable]
public record GetUserByIDQuery : IRequest<BaseResponse<GetUserResponseDTO>>
{
    public int userid { get; set; }
}
