
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
                         select new
                         {
                             h_draft.TransferHeaderID,
                             h_draft.TransferRefNo,
                             d_draft.TransferDetailID,
                             branch.BranchID,
                             branch.BranchName,
                             item.ItemID,
                             item.ItemCode,
                             ItemName = item.Name,
                             SubItemTypeID = jSubItem != null ? jSubItem.SubItemTypeID : 1,
                             SubTypeName = jSubItem != null ? jSubItem.SubTypeNameTH : "N/A",
                             qty = d_draft.Qty,
                             h_draft.CreatedBy,
                             h_draft.CreatedDate,

                         }).ToList();

        if (!resReport.Any())
        {
            throw new Exception("Data not found");
        }

        var _resReport = resReport;
        return new BaseResponse<InventoryTransferReportByDraftIDResponseDTO>
        {
            result = true,
            data = new InventoryTransferReportByDraftIDResponseDTO(),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
