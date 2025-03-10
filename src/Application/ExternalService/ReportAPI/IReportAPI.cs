using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ReportService.Commands.CreateAuditReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.AuditReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.AvailableStockByBrachReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.AvailableStockReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.CountStockReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.InventoryReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.InventoryTransferByDraftID.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.InventoryTransferReportByDraftID.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.ItemTransactionLogReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.ItemTransferShortageReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReportByBranch.v1;

namespace CYRetailIMS.Application.ExternalService.ReportAPI;
public interface IReportAPI
{
	Task<BaseResponse<CommandResponse>> CreateAuditTransactionReportAsync(CreateAuditReportCommand createAuditReportCommand);

    Task<BaseResponse<SaleReportResponseDTO>> GetSaleReportByCriteriaAsync(SaleReportQuery saleReportQuery);

	Task<BaseResponse<List<SaleSummaryReportResponseDTO>>> GetSaleSummaryReportAsync(SaleSummaryReportQuery saleSummaryReportQuery);

	Task<BaseResponse<List<AuditReportResponseDTO>>> GetAuditReportAsync(AuditReportQuery auditReportQuery);

    Task<BaseResponse<SaleSummaryReportResponseDTO>> GetSaleSummaryReportByTransIDAsync(int transactionid);

    Task<BaseResponse<SaleSummaryReportResponseDTO>> GetSaleSummaryReportByBranchAsync(SaleSummaryReportByBranchQuery summaryReportByBranchQuery);

    Task<BaseResponse<List<ItemTransactionLogReportResponseDTO>>> GetItemTransactionLogReportAsync(ItemTransactionLogReportQuery logReportQuery);

    Task<BaseResponse<List<AvailableStockReportResponseDTO>>> GetAvailableItemStockReportAsync(AvailableStockReportQuery availableStockReportQuery);

    Task<BaseResponse<List<AvailableStockReportResponseDTO>>> GetAvailableItemStockByBranchReportAsync(AvailableStockByBrachReportQuery availableStockByBrachReportQuery);

    Task<BaseResponse<List<InventoryReportResponseDTO>>> GetInventoryReportAsync(InventoryReportQuery inventoryReportQuery);

    Task<BaseResponse<InventoryTransferReportByDraftIDResponseDTO>> GetInventoryTransferByDraftReportAsync(InventoryTransferReportByDraftIDQuery inventoryTransferQuery);

    Task<BaseResponse<List<CountStockReportResponseDTO>>> GetCountStockReportAsync(CountStockReportQuery countStockReportQuery);

    Task<BaseResponse<List<ItemTransferShortageReportResponseDTO>>> GetItemTransferShortageReportAsync(ItemTransferShortageReportQuery transferShortageReportQuery);
}
