using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;

namespace CYRetailIMS.Application.ExternalService.TransactionAPI;
public interface ITransactionAPI
{
    Task<BaseResponse<CommandResponse>> CreateTransactionAsync(CreateTransactionCommand createItemCommand);

}
