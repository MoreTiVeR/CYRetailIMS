using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.CreateReceipt.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.CreateReceiveTemplate.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.GenerateReceiptNo.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.UpdateReceiveTemplate.v1;

namespace CYRetailIMS.Application.ExternalService.ReceiptAPI;
public interface IReceiptAPI
{
    Task<BaseResponse<GenerateReceiptNoResponseDTO>> GenerateReceiptNoByBranchAsync(GenerateReceiptNoCommand generateReceiptNoCommand);
    Task<BaseResponse<CommandResponse>> CreateReceiptAsync(CreateReceiptCommand createReceiptCommand);
}
