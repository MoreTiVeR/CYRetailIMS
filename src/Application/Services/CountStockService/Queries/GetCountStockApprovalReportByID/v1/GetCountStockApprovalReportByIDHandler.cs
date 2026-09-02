using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockApprovalReportByID.v1;

public class GetCountStockApprovalReportByIDHandler : BaseService,
    IRequestHandler<GetCountStockApprovalReportByIDQuery, BaseResponse<GetCountStockApprovalReportByIDResponseDTO>>
{
    public GetCountStockApprovalReportByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetCountStockApprovalReportByIDResponseDTO>> Handle(
        GetCountStockApprovalReportByIDQuery request,
        CancellationToken cancellationToken)
    {
        var historyQuery = await _unitOfWork.Repository<TTCountStockApprovalHistory>()
            .QueryAsync(w => w.IsActive && w.CountStockID == request.countstockid);

        if (!historyQuery.Any())
        {
            throw new Exception("ไม่พบประวัติการอนุมัตินับสต๊อก");
        }

        var itemQuery = await _unitOfWork.Repository<TMItem>().QueryAsync(w => w.IsActive);
        var subItemTypeQuery = await _unitOfWork.Repository<TMSubItemType>().QueryAsync();
        var countStockQuery = await _unitOfWork.Repository<TTCountStock>().QueryAsync(w => w.IsActive);
        var branchQuery = await _unitOfWork.Repository<TMBranch>().QueryAsync(w => w.IsActive);

        var detailData = (from h in historyQuery
                          join i in itemQuery on h.ItemID equals i.ItemID into ji
                          from i in ji.DefaultIfEmpty()
                          join s in subItemTypeQuery on h.SubItemTypeID equals s.SubItemTypeID into js
                          from s in js.DefaultIfEmpty()
                          orderby s.SubTypeNameTH, i.ItemCode
                          select new GetCountStockApprovalReportByIDDetailDTO
                          {
                              countstockapprovalhistoryid = h.CountStockApprovalHistoryID,
                              countstockdetailid = h.CountStockDetailID,
                              itemid = h.ItemID,
                              itemcode = i != null ? i.ItemCode : string.Empty,
                              itemname = i != null ? i.Name : string.Empty,
                              subitemtypeid = h.SubItemTypeID,
                              subitemcode = s != null ? s.SubTypeNameTH : string.Empty,
                              qtyinbranchofcountstockday = h.QtyInBranchOfCountStockDay,
                              qtyinbranchbeforeapprove = h.QtyInBranchBeforeApprove,
                              qtyinbranchafterapprove = h.QtyInBranchAfterApprove,
                              countedamountqty = h.CountedAmountQty,
                              pendingrestockqty = h.PendingReStockQty,
                              damagedqty = h.DamagedQty,
                              salebeforecountqty = h.SaleBeforeCountQty,
                              totalcountqty = h.TotalCountQty,
                              shortagesurplusqty = h.ShortageSurplusQty,
                              itemremark = h.ItemRemark,
                              adjustedqty = h.QtyInBranchAfterApprove - h.QtyInBranchBeforeApprove
                          }).ToList();

        var headerData = (from cs in countStockQuery
                          join b in branchQuery on cs.BranchID equals b.BranchID into jb
                          from b in jb.DefaultIfEmpty()
                          where cs.CountStockID == request.countstockid
                          select new
                          {
                              cs.CountStockID,
                              cs.CountDate,
                              cs.BranchID,
                              BranchName = b != null ? b.BranchName : string.Empty,
                              cs.CounterRole,
                              cs.ApprovedBy,
                              cs.ApprovedDate,
                              cs.Remark
                          }).FirstOrDefault();

        var historyHeader = historyQuery
            .OrderByDescending(o => o.ApprovedDate)
            .ThenByDescending(o => o.CountStockApprovalHistoryID)
            .First();

        var response = new GetCountStockApprovalReportByIDResponseDTO
        {
            countstockid = request.countstockid,
            countstockdate = headerData?.CountDate ?? historyHeader.CountStockDate,
            branchid = headerData?.BranchID ?? historyHeader.BranchID,
            branchname = headerData?.BranchName ?? string.Empty,
            counterrole = headerData?.CounterRole ?? historyHeader.CounterRole,
            approvedby = headerData?.ApprovedBy ?? historyHeader.ApprovedBy,
            approveddate = headerData?.ApprovedDate ?? historyHeader.ApprovedDate,
            remark = headerData?.Remark,
            totalqtybefore = detailData.Sum(s => s.qtyinbranchbeforeapprove),
            totalqtyafter = detailData.Sum(s => s.qtyinbranchafterapprove),
            totaladjustedqty = detailData.Sum(s => s.adjustedqty),
            detail = detailData
        };

        return new BaseResponse<GetCountStockApprovalReportByIDResponseDTO>
        {
            result = true,
            data = response,
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
