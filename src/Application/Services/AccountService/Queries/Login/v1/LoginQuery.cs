using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.AccountService.Queries.Login.v1;
public record LoginQuery : IRequest<BaseResponse<UserProfileResponseDTO>>
{
    [Required(AllowEmptyStrings = false)]
    public string username { get; init; }

	[Required(AllowEmptyStrings = false)]
    public string password { get; init; }
}
