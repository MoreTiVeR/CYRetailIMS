
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ReportService.Queries.InventoryTransferReportByDraftID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ReportService.Queries.InventoryTransferByDraftID.v1;
public class InventoryTransferReportByDraftIDHandler : BaseService, IRequestHandler<InventoryTransferReportByDraftIDQuery, BaseResponse<InventoryTransferReportByDraftIDResponseDTO>>
{
    public InventoryTransferReportByDraftIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<InventoryTransferReportByDraftIDResponseDTO>> Handle(InventoryTransferReportByDraftIDQuery request, CancellationToken cancellationToken)
    {
        //IEnumerable<TTDraftItemTransfer> draftList = await _unitOfWork.Repository<TTDraftItemTransfer>().FindWithInclude(w => w.TransferHeaderID == request.transferid,
        //    i => i.Include(s => s.TTDraftItemTransferDetails));

        var resReport = (from h_draft in await _unitOfWork.Repository<TTDraftItemTransfer>().QueryAsync()
                         join d_draft in await _unitOfWork.Repository<TTDraftItemTransferDetail>().QueryAsync() on h_draft.TransferHeaderID equals d_draft.TransferHeaderID
                         join branch in await _unitOfWork.Repository<TMBranch>().QueryAsync() on h_draft.DestinationBranchID equals branch.BranchID
                         join item in await _unitOfWork.Repository<TMItem>().QueryAsync() on d_draft.ItemID equals item.ItemID
                         join subitem in await _unitOfWork.Repository<TMSubItemType>().QueryAsync() on item.SubItemTypeID equals subitem.SubItemTypeID into iSubType
                         from jSubItem in iSubType.DefaultIfEmpty()
                         where h_draft.IsActive == true 
                         && h_draft.TransferHeaderID == request.transferid 
                         && h_draft.TransferStatus == (int)EnumModel.TransferStatus.Received
                         select new InventoryTransferDataDTO
                         {
                             TransferHeaderID = h_draft.TransferHeaderID,
                             TransferRefNo = h_draft.TransferRefNo,
                             TransferDetailID = d_draft.TransferDetailID,
                             SourceBranchID = h_draft.SourceBranchID,
                             SourceBranchName = "คลังสำนักงานใหญ่",
                             Description = h_draft.Description,
                             BranchID = branch.BranchID,
                             BranchName = branch.BranchName,
                             ItemID = item.ItemID,
                             ItemCode = item.ItemCode,
                             ItemName = item.Name,
                             SubItemTypeID = jSubItem != null ? jSubItem.SubItemTypeID : 1,
                             SubItemTypeName = jSubItem != null ? jSubItem.SubTypeNameTH : "N/A",
                             Qty = d_draft.Qty,
                             CreatedBy = h_draft.CreatedBy,
                             CreatedDate = h_draft.CreatedDate,

                         }).ToList();

        if (!resReport.Any())
        {
            throw new Exception("Data not found");
        }

        int runningno = 1;
        int runningno_subitem = 1;
        InventoryTransferReportByDraftIDResponseDTO res = new InventoryTransferReportByDraftIDResponseDTO();
        res.transferheaderid = resReport.FirstOrDefault().TransferHeaderID;
        res.refno = resReport.FirstOrDefault().TransferRefNo;
        res.sourcebranchid = 1;
        res.sourcebranchname = "คลัง";
        res.destinationbranchid = resReport.FirstOrDefault().BranchID;
        res.destinationbranchname = resReport.FirstOrDefault().BranchName;
        res.description = resReport.FirstOrDefault().Description;
        res.createdby = resReport.FirstOrDefault().CreatedBy;
        res.createddate = resReport.FirstOrDefault().CreatedDate;
        res.detail = (from a in resReport.OrderBy(s => s.TransferDetailID)
                      select new InventoryTransferReportByDraftIDDetailDTO
                      {
                          seq = runningno++,
                          transferdetailid = a.TransferDetailID,
                          itemid = a.ItemID,
                          itemcode = a.ItemCode,
                          itemname = a.ItemName,
                          subitemtypeid = a.SubItemTypeID,
                          subitemtypename = a.SubItemTypeName,
                          transferqty = a.Qty
                      }).ToList();
        res.totaltransferqty = res.detail.Sum(s => s.transferqty);
        var groupdetail = resReport.OrderBy(s => s.TransferDetailID).Where(w => w.SubItemTypeID > 0).GroupBy(g => g.SubItemTypeID).Select(s => new InventoryTransferReportByDraftIDSubItemTypeDTO
        {
            seq = runningno_subitem++,
            subitemtypeid = s.Key,
            subitemtypename = s.FirstOrDefault(w => w.SubItemTypeID == s.Key).SubItemTypeName,
            transferqty = s.Sum(q => q.Qty)
        }).ToList();
        res.subitemdetail = groupdetail;
        res.totalsubitemtransferqty = groupdetail.Sum(s => s.transferqty);

        #region Update updatedby data from emp name
        IEnumerable<TMUsers> userList = await _unitOfWork.Repository<TMUsers>().FindWithInclude(w => w.UserName == res.createdby, i => i.Include(w => w.TMEmployees));
        var empDataList = userList.Select(s => new { s.UserName, s.TMEmployees.FirstOrDefault().FirstName }).ToList();
        res.createdbyname = empDataList.FirstOrDefault(w => w.UserName == res.createdby) != null 
            ? empDataList.FirstOrDefault(w => w.UserName == res.createdby).FirstName : res.createdby;
        #endregion

        return new BaseResponse<InventoryTransferReportByDraftIDResponseDTO>
        {
            result = true,
            data = res,
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
