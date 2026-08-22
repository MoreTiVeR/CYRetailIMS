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

        // Update TMItemInBranch stock quantities per SubItemType
        // Group by subitemtype and sum countedamountqty for each item in branch
        var branchItems = await _unitOfWork.Repository<TMItemInBranch>()
            .QueryAsync(w => w.BranchID == countStock.BranchID && w.IsActive);

        var itemsWithSubType = branchItems
            .Where(i => i.Item != null)
            .ToList();

        foreach (var detail in countStock.TTCountStockDetails)
        {
            // Find items in this branch that belong to this subitemtype
            var itemsInSubType = itemsWithSubType
                .Where(i => i.Item.SubItemTypeID == detail.SubItemTypeID)
                .ToList();

            if (!itemsInSubType.Any()) continue;

            // Distribute the counted qty proportionally, or set total for subitemtype
            // Per spec: approve = update stock to counted qty
            // We update the aggregate qty at subitemtype level by adjusting each item proportionally
            int totalCurrentQty = itemsInSubType.Sum(i => i.Qty);
            int targetQty = detail.CountedAmountQty;

            foreach (var item in itemsInSubType)
            {
                if (totalCurrentQty > 0)
                {
                    // Proportional distribution
                    item.Qty = (int)Math.Round((double)item.Qty / totalCurrentQty * targetQty);
                }
                else
                {
                    // If current qty is 0, distribute evenly
                    item.Qty = targetQty / itemsInSubType.Count;
                }
                item.SetUpdatedBy(request.approvedby);
                item.SetUpdatedDate();
                _unitOfWork.Repository<TMItemInBranch>().Update(item);
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
