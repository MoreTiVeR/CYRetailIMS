using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.ApproveCountStock.v1;

/// <summary>
/// Handler: อนุมัติการนับสต๊อก (เฉพาะรายการที่หัวหน้า PC ส่งมา)
/// เปลี่ยนสถานะเป็น Approved(2) และปรับ Qty ใน TMItemInBranch ให้เท่ากับยอดที่นับได้
/// </summary>
public class ApproveCountStockHandler : BaseService, IRequestHandler<ApproveCountStockCommand, BaseResponse<CommandResponse>>
{
    public ApproveCountStockHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(ApproveCountStockCommand request, CancellationToken cancellationToken)
    {
        IQueryable<TTCountStock> countStockQuery = await _unitOfWork.Repository<TTCountStock>()
            .FindWithInclude(
                w => w.CountStockID == request.countstockid && w.IsActive,
                i => i.Include(s => s.TTCountStockDetails));
        TTCountStock countStock = countStockQuery.FirstOrDefault();

        if (countStock is null)
        {
            throw new Exception("ไม่พบข้อมูลนับสต๊อก กรุณาลองใหม่อีกครั้ง");
        }

        if (countStock.CounterRole != "HeadPC")
        {
            throw new Exception("ไม่สามารถอนุมัติได้ เนื่องจากรายการนี้ไม่ได้ส่งโดยหัวหน้า PC");
        }

        if (countStock.CountStockStatusID != 1)
        {
            throw new Exception("ไม่สามารถอนุมัติได้ เนื่องจากสถานะรายการไม่ถูกต้อง");
        }

        await _unitOfWork.BeginTransactionAsync();

        // Update TTCountStock status to Approved
        countStock.CountStockStatusID = 2; // Approved
        countStock.ApprovedBy = request.approvedby;
        countStock.ApprovedDate = DateTime.Now;
        countStock.SetUpdatedBy(request.approvedby);
        countStock.SetUpdatedDate();
        _unitOfWork.Repository<TTCountStock>().Update(countStock);

        // Load branch items with Item navigation to enable SubItemType matching for V1 fallback
        var branchItemsQuery = await _unitOfWork.Repository<TMItemInBranch>()
            .FindWithInclude(
                w => w.BranchID == countStock.BranchID && w.IsActive,
                i => i.Include(s => s.Item));
        var branchItems = branchItemsQuery.ToList();

        foreach (var detail in countStock.TTCountStockDetails)
        {
            int targetQty = detail.TotalCountQty > 0 ? detail.TotalCountQty : detail.CountedAmountQty;

            if (detail.ItemID.HasValue && detail.ItemID.Value > 0)
            {
                // V2 per-item: update the specific item directly by ItemID
                var item = branchItems.FirstOrDefault(i => i.ItemID == detail.ItemID.Value);
                if (item == null) continue;
                item.Qty = targetQty;
                item.SetUpdatedBy(request.approvedby);
                item.SetUpdatedDate();
                _unitOfWork.Repository<TMItemInBranch>().Update(item);
            }
            else
            {
                // V1 legacy: distribute proportionally across items in the same SubItemType
                var itemsInSubType = branchItems
                    .Where(i => i.Item?.SubItemTypeID == detail.SubItemTypeID)
                    .ToList();
                if (!itemsInSubType.Any()) continue;

                int totalCurrentQty = itemsInSubType.Sum(i => i.Qty);
                foreach (var item in itemsInSubType)
                {
                    item.Qty = totalCurrentQty > 0
                        ? (int)Math.Round((double)item.Qty / totalCurrentQty * targetQty)
                        : targetQty / itemsInSubType.Count;
                    item.SetUpdatedBy(request.approvedby);
                    item.SetUpdatedDate();
                    _unitOfWork.Repository<TMItemInBranch>().Update(item);
                }
            }
        }

        await _unitOfWork.SaveChangesAsync();
        await _unitOfWork.CommitTransactionAsync();

        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse { result = true },
            message = "อนุมัติและปรับสต๊อกสำเร็จ",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
