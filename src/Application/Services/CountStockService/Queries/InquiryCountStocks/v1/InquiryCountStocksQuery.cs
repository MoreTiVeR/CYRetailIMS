using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStocks.v1;
public class InquiryCountStocksQuery : IRequest<BaseResponse<List<InquiryCountStockResponseDTO>>>
{
    public int? branchid { get; set; }
    public DateTime? startdate { get; set; }
    public DateTime? enddate { get; set; }
}
