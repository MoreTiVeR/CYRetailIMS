using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStocks.v1;

namespace CYRetailIMS.Application.ExternalService.CountStockAPI;
public interface ICountStockAPI
{
    Task<BaseResponse<List<InquiryCountStockResponseDTO>>> GetCountStockListAsync(InquiryCountStocksQuery inquiryObj);
}
