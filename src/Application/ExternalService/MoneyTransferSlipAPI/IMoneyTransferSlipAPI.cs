using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.MoneyTransferSlipService.Queries.GetSlipByMoneyTransferID.v1;

namespace CYRetailIMS.Application.ExternalService.MoneyTransferSlipAPI;
public interface IMoneyTransferSlipAPI
{
    Task<BaseResponse<GetSlipByMoneyTransferIDResponseDTO>> GetMoneyTransferSlipByMoneyTransferIDAsync(GetSlipByMoneyTransferIDQuery moneyTransferIDQuery);
}
