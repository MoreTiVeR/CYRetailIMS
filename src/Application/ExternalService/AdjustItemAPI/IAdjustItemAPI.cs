using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.CreateAdjustItem.v1;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.UpdateAdjustItem;

namespace CYRetailIMS.Application.ExternalService.AdjustItemAPI;
public interface IAdjustItemAPI
{
    Task<BaseResponse<CommandResponse>> CreateAdjustItemAsync(CreateAdjustItemCommand createAdjustItemCommand);

    Task<BaseResponse<CommandResponse>> UpdateAdjustItemAsync(UpdateAdjustItemCommand updateAdjustItemCommand);
}
