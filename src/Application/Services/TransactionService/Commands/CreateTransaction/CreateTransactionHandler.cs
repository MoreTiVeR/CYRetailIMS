using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TTTransactionDetails;
using CYRetailIMS.Domain.Events.TTTransactions;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
public class CreateTransactionHandler : BaseService, IRequestHandler<CreateTransactionCommand, BaseResponse<CommandResponse>>
{
    public CreateTransactionHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        #region Begin Tran
        await _unitOfWork.BeginTransactionAsync();
        #endregion

        #region Check Item in branch stock
        List<int> itemList = request.transactiondetail.Select(s => s.itemid).ToList();
        IEnumerable<TMItemInBranch> resItemInBranch = await _unitOfWork.Repository<TMItemInBranch>().QueryAsync(w => w.BranchID == request.branchid && itemList.Any(s => s.Equals(w.ItemID)));
        if (!resItemInBranch.Any())
        {
            throw new Exception("ขออภัย, ไม่พบสินค้าในสต๊อก");
        }
        #endregion

        #region Check Qty in branch stock
        bool isAvailableStock = ValidateQtyInBranchStock(request, resItemInBranch);
        if (!isAvailableStock)
        {
            throw new Exception("ขออภัย, จำนวนสต๊อกสินค้าไม่เพียงพอ!");
        }
        #endregion

        #region Create Transaction & Transaction Detail
        TTTransaction tranEntity = MappingTransaction(request);
        tranEntity.SetCreatedBy(request.createdby);
        tranEntity.SetCreatedDate();
        //Mapping CreateTransactionCommand -> TTTransaction
        //Code here
        tranEntity.TTTransactonDetails = MappingTransactionDetail(request);
        tranEntity.TTTransactonDetails.ToList().ForEach(ent =>
        {
            ent.AddDomainEvent(new TTTransactionDetailCreateEvent(ent));
        });
        //tranEntity.AddDomainEvent(new TTTransactionDetailCreateEvent(tranEntity.TTTransactonDetails));
        tranEntity.AddDomainEvent(new TTTransactionsCreateEvent(tranEntity));

        _unitOfWork.Repository<TTTransaction>().Add(tranEntity);

        #endregion

        #region Create Transaction Detail

        #endregion

        #region Update Stock Item In Branch
        resItemInBranch = resItemInBranch.Select(s =>
        {
            int minusQty = request.transactiondetail.Where(w => w.itemid == s.ItemID).FirstOrDefault() != null
            ? request.transactiondetail.Where(w => w.itemid == s.ItemID).FirstOrDefault().qty : 0;
            s.Qty = s.Qty - minusQty;
            return s;
        }).ToList();

        //request.transactiondetail.ForEach(i =>
        //{
        //    TMItemInBranch itemBranch = _unitOfWork.Repository<TMItemInBranch>().Find(w => w.ItemID == i.itemid);
        //    if(ValidateQtyInBranchStock(i, itemBranch))
        //    {
        //        itemBranch.Qty = itemBranch.Qty - i.qty;
        //    }
        //});
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

    private async Task ValidateQtyBeforeUpdateStock(TMItemInBranch s, int minusQty)
    {
        var dsds = await _unitOfWork.Repository<TMItemInBranch>().AnyAsync(w => w.ItemID == s.ItemID && s.Qty < minusQty);
    }

    private TTTransaction MappingTransaction(CreateTransactionCommand createTransactionCommand)
    {
        return new TTTransaction
        {
            TransactionTypeID = createTransactionCommand.transactiontypeid,
            TransactionDate = createTransactionCommand.transactiondate,
            BranchID = createTransactionCommand.branchid,
            AmountTransfer = createTransactionCommand.amounttransfer,
            AmountDeposit = createTransactionCommand.amountdeposit,
            AmountCash = createTransactionCommand.amountcash,
            TotalAmount = createTransactionCommand.totalamount,
            IsExcludeVAT = createTransactionCommand.isexcludevat
        };
    }

    private ICollection<TTTransactonDetail> MappingTransactionDetail(CreateTransactionCommand createTransactionCommand)
    {
        var resTranDetail = (from a in createTransactionCommand.transactiondetail
                             select new TTTransactonDetail
                             {
                                 ItemID = a.itemid,
                                 Price = a.price,
                                 Qty = a.qty,
                                 Amount = a.amount,
                                 IsActive = a.isactive
                             }).ToList();
        return resTranDetail;
    }

    private bool ValidateQtyInBranchStock(CreateTransactionCommand request, IEnumerable<TMItemInBranch> itemInBranches)
    {
        request.transactiondetail.ForEach(item =>
        {
            if(!itemInBranches.Any(stock => stock.Qty > 0 && item.qty <= stock.Qty))
            {
                throw new Exception($"ขออภัย, จำนวนสินค้าในสต๊อกไม่เพียงพอ!");
            }
        });
        return true;
    }
    private bool ValidateQtyInBranchStock(CreateTransactionDetailCommand request, TMItemInBranch itemInBranches)
    {
        if (itemInBranches.Qty <= 0 || itemInBranches.Qty < request.qty)
        {
            throw new Exception($"ขออภัย, จำนวนสินค้าในสต๊อกไม่เพียงพอ!");
        }
        return true;
    }
}
