using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.BranchService.Commands.CreateBranch.v1;
using CYRetailIMS.Application.Services.ChartService.Queries.GetMontlySaleSummaryBarchart.v1;
using CYRetailIMS.Application.Services.ChartService.Queries.GetMontlySaleSummaryByYear.v1;
using CYRetailIMS.Application.Services.ChartService.Queries.GetSellingTransactionByMonth.v1;

namespace CYRetailIMS.Application.ExternalService.ChartAPI;
public interface IChartAPI
{
    Task<BaseResponse<List<GetMontlySaleSummaryBarchartResponseDTO>>> GetMontlySameSummaryAsync(GetMontlySaleSummaryBarchartQuery summaryBarchartQuery);
    Task<BaseResponse<List<GetMontlySaleSummaryBarchartResponseDTO>>> GetMontlySameSummaryV2Async(GetMontlySaleSummaryBarchartQuery summaryBarchartQuery);
    Task<BaseResponse<List<GetMontlySaleSummaryByYearResponseDTO>>> GetMontlySaleSummaryByYearAsync(GetMontlySaleSummaryByYearQuery saleSummaryByYearQuery);
    Task<BaseResponse<List<GetSellingTransactionByMonthResponseDTO>>> GetSellingItemPercnetageAsync(GetTransactionByMonthQuery transactionByMonthQuery);
}
