using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.ReportAPI;
using CYRetailIMS.Application.Services.ReportService.Commands.CreateAuditReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.AuditReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.AvailableStockByBrachReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.AvailableStockReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.CountStockReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.InventoryReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.InventoryTransferByDraftID.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.InventoryTransferReportByDraftID.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.ItemStockReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.ItemTransactionLogReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.ItemTransferShortageReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReportGroupByBranch.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReportByBranch.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleBarcodeReport.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.ReportAPI;
public class ReportAPI : HttpClientService, IReportAPI
{
    public ReportAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<CommandResponse>> CreateAuditTransactionReportAsync(CreateAuditReportCommand createAuditReportCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse,
            CreateAuditReportCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/createaudittransaction"), createAuditReportCommand);
    }

    public async Task<BaseResponse<SaleReportResponseDTO>> GetSaleReportByCriteriaAsync(SaleReportQuery saleReportQuery)
    {
		return await _httpClientRequest.HttpRequestToObject<SaleReportResponseDTO, 
            SaleReportQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/salereport"), saleReportQuery);
	}

    public async Task<BaseResponse<List<SaleSummaryReportResponseDTO>>> GetSaleSummaryReportAsync(SaleSummaryReportQuery saleSummaryReportQuery)
    {
		return await _httpClientRequest.HttpRequestToObject<List<SaleSummaryReportResponseDTO>,
			SaleSummaryReportQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/salesummaryreport"), saleSummaryReportQuery);
	}

    public async Task<BaseResponse<List<AuditReportResponseDTO>>> GetAuditReportAsync(AuditReportQuery auditReportQuery)
    {
		return await _httpClientRequest.HttpRequestToObject<List<AuditReportResponseDTO>,
			AuditReportQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/auditreport"), auditReportQuery);
	}

    public async Task<BaseResponse<SaleSummaryReportResponseDTO>> GetSaleSummaryReportByTransIDAsync(int transactionid)
    {
        return await _httpClientRequest.HttpRequestToObject<SaleSummaryReportResponseDTO,
            object>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/salesummaryreportbytransid/{transactionid}"), null);
    }

    public async Task<BaseResponse<SaleSummaryReportResponseDTO>> GetSaleSummaryReportByBranchAsync(SaleSummaryReportByBranchQuery summaryReportByBranchQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<SaleSummaryReportResponseDTO,
            SaleSummaryReportByBranchQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/salesummaryreportbybranch"), summaryReportByBranchQuery);
    }

    /// <summary>
    /// รายงานการปรับราคาขายสินค้า
    /// </summary>
    /// <param name="logReportQuery"></param>
    /// <returns></returns>
    public async Task<BaseResponse<List<ItemTransactionLogReportResponseDTO>>> GetItemTransactionLogReportAsync(ItemTransactionLogReportQuery logReportQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<List<ItemTransactionLogReportResponseDTO>,
                    ItemTransactionLogReportQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/itemtransactionlog"), logReportQuery);
    }


    public async Task<BaseResponse<List<AvailableStockReportResponseDTO>>> GetAvailableItemStockReportAsync(AvailableStockReportQuery availableStockReportQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<List<AvailableStockReportResponseDTO>,
                    AvailableStockReportQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/availableitemstock"), availableStockReportQuery);
    }

    public async Task<BaseResponse<List<AvailableStockReportResponseDTO>>> GetAvailableItemStockByBranchReportAsync(AvailableStockByBrachReportQuery availableStockByBrachReportQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<List<AvailableStockReportResponseDTO>,
                    AvailableStockByBrachReportQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/availableitemstockinbranch"), availableStockByBrachReportQuery);
    }

    public async Task<BaseResponse<List<InventoryReportResponseDTO>>> GetInventoryReportAsync(InventoryReportQuery inventoryReportQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<List<InventoryReportResponseDTO>,
                    InventoryReportQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/inventoryreport"), inventoryReportQuery);
    }

    public async Task<BaseResponse<InventoryTransferReportByDraftIDResponseDTO>> GetInventoryTransferByDraftReportAsync(InventoryTransferReportByDraftIDQuery inventoryTransferQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<InventoryTransferReportByDraftIDResponseDTO,
                    InventoryTransferReportByDraftIDQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/inventorytransferreport"), inventoryTransferQuery);
    }

    public async Task<BaseResponse<List<CountStockReportResponseDTO>>> GetCountStockReportAsync(CountStockReportQuery countStockReportQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<List<CountStockReportResponseDTO>,
                            CountStockReportQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/countstockreport"), countStockReportQuery);
    }

    public async Task<BaseResponse<List<ItemTransferShortageReportResponseDTO>>> GetItemTransferShortageReportAsync(ItemTransferShortageReportQuery transferShortageReportQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<List<ItemTransferShortageReportResponseDTO>,
            ItemTransferShortageReportQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/itemtransfershortagereport"), transferShortageReportQuery);
    }

    public async Task<BaseResponse<ItemStockReportResponseDTO>> GetItemStockReportAsync(ItemStockReportQuery itemStockReport)
    {
        return await _httpClientRequest.HttpRequestToObject<ItemStockReportResponseDTO,
            ItemStockReportQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/itemstock"), itemStockReport);
    }

    public async Task<BaseResponse<SaleReportGroupByBranchResposneDTO>> GetSaleReportByGroupAsync(SaleReportGroupByBranchQuery saleReportGroupByBranchQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<SaleReportGroupByBranchResposneDTO,
            SaleReportGroupByBranchQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/salereportbygroup"), saleReportGroupByBranchQuery);
    }

    public async Task<BaseResponse<List<SaleBarcodeReportResponseDTO>>> GetSaleBarcodeReportAsync(SaleBarcodeReportQuery saleBarcodeReportQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<List<SaleBarcodeReportResponseDTO>,
            SaleBarcodeReportQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/salebarcodereport"), saleBarcodeReportQuery);
    }
}
