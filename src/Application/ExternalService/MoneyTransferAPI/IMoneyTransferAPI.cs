using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransfer.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransferList.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.DeleteMoneyTransfer.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.UpdateMoneyTransfer.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Queries.GetMoneyTransferByCriteria.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Queries.GetMoneyTransferByID.v1;

namespace CYRetailIMS.Application.ExternalService.MoneyTransferAPI;
public interface IMoneyTransferAPI
{
    Task<BaseResponse<CommandResponse>> CreateAsync(CreateMoneyTransferCommand moneyTransferCommand);
    Task<BaseResponse<CommandResponse>> BulkCreateAsync(CreateMoneyTransferListCommand moneyTransferCommand);
    Task<BaseResponse<CommandResponse>> UpdateAsync(UpdateMoneyTransferCommand moneyTransferCommand);
    Task<BaseResponse<CommandResponse>> DeleteAsync(DeleteMoneyTransferCommand moneyTransferCommand);
    Task<BaseResponse<List<GetMoneyTransferByCriteriaResponseDTO>>> GetMoeytransferByCriteriaAsync(GetMoneyTransferByCriteriaQuery reqData);
    Task<BaseResponse<GetMoneyTransferByCriteriaResponseDTO>> GetMoeytransferByIDAsync(GetMoneyTransferByIDQuery reqData);
}
