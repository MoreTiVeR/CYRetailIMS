using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.CountStockService.Commands.ApproveCountStock.v1;
using CreateCountStockCommandV1 = CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v1.CreateCountStockCommand;
using CreateCountStockCommandV2 = CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v2.CreateCountStockCommand;
using CYRetailIMS.Application.Services.CountStockService.Commands.DeleteCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Commands.CancelCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Commands.SubmitCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Commands.UpdateCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockComparison.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.GetPendingApprovals.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockApprovalReport.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockApprovalReportByID.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByBranchID.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByID.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStocks.v1;

namespace CYRetailIMS.Application.ExternalService.CountStockAPI;
public interface ICountStockAPI
{
    Task<BaseResponse<CommandResponse>> CreateCountStockListAsync(CreateCountStockCommandV1 createCommand);
    Task<BaseResponse<CommandResponse>> CreateCountStockListV2Async(CreateCountStockCommandV2 createCommand);

    Task<BaseResponse<CommandResponse>> UpdateCountStocAsync(UpdateCountStockCommand updateCommand);
    Task<BaseResponse<CommandResponse>> DeleteCountStockAsync(DeleteCountStockCommand deleteCommand);
    Task<BaseResponse<List<InquiryCountStockResponseDTO>>> GetCountStockListAsync(InquiryCountStocksQuery inquiryObj);
    Task<BaseResponse<List<InquiryCountStockByBranchIDResponseDTO>>> InquiryCountStockByBranchIDAsync(InquiryCountStockByBranchIDQuery inquiryObj);
    Task<BaseResponse<InquiryCountStockByIDResponseDTO>> InquiryCountStockByStockIDAsync(InquiryCountStockByIDQuery inquiryObj);

    /// <summary>
    /// ส่งข้อมูลนับสต๊อกเพื่อรออนุมัติ (Draft → Submitted)
    /// </summary>
    Task<BaseResponse<CommandResponse>> SubmitCountStockAsync(SubmitCountStockCommand submitCommand);

    /// <summary>
    /// ยกเลิกรายการรออนุมัติ (Submitted → Draft)
    /// </summary>
    Task<BaseResponse<CommandResponse>> CancelCountStockAsync(CancelCountStockCommand cancelCommand);

    /// <summary>
    /// อนุมัติการนับสต๊อก (เฉพาะรายการ HeadPC เท่านั้น) และปรับสต๊อกในระบบ
    /// </summary>
    Task<BaseResponse<CommandResponse>> ApproveCountStockAsync(ApproveCountStockCommand approveCommand);

    /// <summary>
    /// ดึงรายการรออนุมัติ (หน้ารออนุมัติ)
    /// </summary>
    Task<BaseResponse<List<GetPendingApprovalsResponseDTO>>> GetPendingApprovalsAsync(GetPendingApprovalsQuery query);

    /// <summary>
    /// ดึงข้อมูลเปรียบเทียบสต๊อก (หน้าเทียบข้อมูล)
    /// </summary>
    Task<BaseResponse<List<GetCountStockComparisonResponseDTO>>> GetCountStockComparisonAsync(GetCountStockComparisonQuery query);

    /// <summary>
    /// รายงานรายการอนุมัตินับสต๊อก (Index)
    /// </summary>
    Task<BaseResponse<GetCountStockApprovalReportResponseDTO>> GetCountStockApprovalReportAsync(GetCountStockApprovalReportQuery query);

    /// <summary>
    /// รายงานรายละเอียดอนุมัตินับสต๊อก (By CountStockID)
    /// </summary>
    Task<BaseResponse<GetCountStockApprovalReportByIDResponseDTO>> GetCountStockApprovalReportByIDAsync(GetCountStockApprovalReportByIDQuery query);
}

