using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TTItemTransfers;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer;
public class CreateItemTransferHandler : BaseService, IRequestHandler<CreateItemTransferCommand, BaseResponse<CommandResponse>>
{
    public CreateItemTransferHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreateItemTransferCommand request, CancellationToken cancellationToken)
    {
        #region Validate Qty In Stock
        await _unitOfWork.BeginTransactionAsync();
        #endregion

        #region Check Item in Warehouse stock
        List<int> itemList = request.items.Select(s => s.itemid).ToList();
        IEnumerable<TMItem> resItemInWarehouse = await _unitOfWork.Repository<TMItem>().QueryAsync(w => itemList.Any(s => s.Equals(w.ItemID)) && w.IsActive);
        if (!resItemInWarehouse.Any())
        {
            throw new Exception("ขออภัย, ไม่พบสินค้าในคลังสินค้า!");
        }
        #endregion

        #region Check Qty in Warehouse
        bool isAvailableStock = ValidateQtyInBranchStock(request, resItemInWarehouse);
        if (!isAvailableStock)
        {
            throw new Exception("ขออภัย, จำนวนสต๊อกสินค้าไม่เพียงพอ!");
        }
        #endregion

        #region Validate Item in Branch
        IEnumerable<TMItemInBranch> resItemInBranch = await _unitOfWork.Repository<TMItemInBranch>().QueryAsync(w => w.BranchID == request.destinationid && w.IsActive);
        if (!resItemInBranch.Any())
        {
            throw new Exception("ขออภัย, ไม่พบสาขาปลายทาง!");
        }
        #endregion

        #region Craete TTItemTransfer
        ICollection<TTItemTransfer> itemTransferEntities = PrepreTTItemTransfer(request);
        itemTransferEntities.ToList().ForEach(e =>
        {
            e.AddDomainEvent(new TTItemTransferCreateEvent(e));
        });
        await _unitOfWork.Repository<TTItemTransfer>().AddRangeAsync(itemTransferEntities);
        #endregion

        #region Update Stock
        resItemInWarehouse = resItemInWarehouse.Select(s =>
        {
            int minusQty = request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault() != null
            ? request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault().qty : 0;
            s.Qty = s.Qty - minusQty;
            return s;
        }).ToList();
        #endregion

        #region Update Branch Stock
        resItemInBranch = resItemInBranch.Select(s =>
        {
            int plusQty = request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault() != null
            ? request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault().qty : 0;
            s.Qty = s.Qty + plusQty;
            return s;
        }).ToList();
        #endregion

        #region Commit Tran
        await _unitOfWork.SaveChangesAsync();
        await _unitOfWork.CommitTransactionAsync();
        #endregion

        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse { result = true },
            status = StatusCodes.Status200OK.ToString(),
            message = "Success",
            soruce = "db"
        };
    }

    #region Private Method
    private ICollection<TTItemTransfer> PrepreTTItemTransfer(CreateItemTransferCommand itemTransferCommand)
    {
        return (from a in itemTransferCommand.items
                let t = itemTransferCommand
                select new TTItemTransfer
                {
                    TransferTypeID = t.transfertypeid,
                    SourceID = t.sourceid,
                    DestinationID = t.destinationid,
                    ItemID = a.itemid,
                    Qty = a.qty,
                    Description = t.description,
                    CreatedBy = t.createdby,
                    CreadedDate = itemTransferCommand.creadeddate,
                    IsActive = t.isactive,
                    ApproveStatus = t.approvestatus
                }).ToList();
    }

    private bool ValidateQtyInBranchStock(CreateItemTransferCommand request, IEnumerable<TMItem> itemInWarehouse)
    {
        request.items.ForEach(item =>
        {
            if (!itemInWarehouse.Any(stock => stock.Qty > 0 && item.qty <= stock.Qty))
            {
                throw new Exception($"ขออภัย, จำนวนสินค้าในสต๊อกไม่เพียงพอ!");
            }
        });
        return true;
    }
    #endregion
}
