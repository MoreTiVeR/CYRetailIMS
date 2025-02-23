using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByBranchID.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByID.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStocks.v1;

namespace CYRetailIMS.Application.ExternalService.CountStockAPI;
public interface ICountStockAPI
{
    Task<BaseResponse<CommandResponse>> CreateCountStockListAsync(CreateCountStockCommand createCommand);
    Task<BaseResponse<List<InquiryCountStockResponseDTO>>> GetCountStockListAsync(InquiryCountStocksQuery inquiryObj);
    Task<BaseResponse<List<InquiryCountStockByBranchIDResponseDTO>>> InquiryCountStockByBranchIDAsync(InquiryCountStockByBranchIDQuery inquiryObj);
    Task<BaseResponse<InquiryCountStockByIDResponseDTO>> InquiryCountStockByStockIDAsync(InquiryCountStockByIDQuery inquiryObj);

}
