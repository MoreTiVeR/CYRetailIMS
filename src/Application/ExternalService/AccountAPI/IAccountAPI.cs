using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.AccountService.Queries.Login.v1;

namespace CYRetailIMS.Application.ExternalService.AccountAPI;
public interface IAccountAPI
{
    Task<BaseResponse<UserProfileResponseDTO>> LoginAsync(LoginQuery loginQuery);
}
