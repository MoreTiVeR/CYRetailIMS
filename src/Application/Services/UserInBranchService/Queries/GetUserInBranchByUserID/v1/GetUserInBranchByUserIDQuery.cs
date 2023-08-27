using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.UserInBranchService.Queries.GetUserInBranchByUserID.v1;

[Serializable]
public record GetUserInBranchByUserIDQuery : IRequest<BaseResponse<GetUserInBranchByUserIDResponseDTO>>
{
    [Required(ErrorMessage = "User id is required")]
    public int userid { get; init; }
}
