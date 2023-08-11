using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.AccountService.Queries.Login.v1;
public record LoginQuery : IRequest<BaseResponse<UserProfileResponseDTO>>
{
    [JsonPropertyName("username")]
    public string UserName { get; init; }

    [JsonPropertyName("password")]
    public string Password { get; init; }
}
