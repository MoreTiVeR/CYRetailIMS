using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ReportService.Queries.AuditReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReport.v1;

namespace CYRetailIMS.Application.ExternalService.Report;
public interface IReportAPI
{
	Task<BaseResponse<List<SaleReportResponseDTO>>> GetSaleReport(SaleReportQuery saleReportQuery);

	Task<BaseResponse<List<SaleSummaryReportResponseDTO>>> GetSaleSummaryReport(SaleSummaryReportQuery saleSummaryReportQuery);

	Task<BaseResponse<List<AuditReportResponseDTO>>> GetAuditReport(AuditReportQuery auditReportQuery);
}
