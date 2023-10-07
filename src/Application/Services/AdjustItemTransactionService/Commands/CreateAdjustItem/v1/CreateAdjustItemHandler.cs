using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMItemInBranchs;
using CYRetailIMS.Domain.Events.TMItems;
using CYRetailIMS.Domain.Events.TTAdjustItemTransactions;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using static CYRetailIMS.Application.Common.Models.EnumModel;

namespace CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.CreateAdjustItem.v1;
public class CreateAdjustItemHandler : BaseService, IRequestHandler<CreateAdjustItemCommand, BaseResponse<CommandResponse>>
{
    public CreateAdjustItemHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreateAdjustItemCommand request, CancellationToken cancellationToken)
    {
        List<CreateAdjustItemDetailCommand> adjustItemWarehouse = request.items.Where(w => w.branchid == 1).ToList();
        List<CreateAdjustItemDetailCommand> adjustItemBranch = request.items.Where(w => w.branchid != 1).ToList();
        List<TTAdjustItemTransaction> adjustItemTransactionList = new List<TTAdjustItemTransaction>();

        #region Begin Transaction
        await _unitOfWork.BeginTransactionAsync();
        #endregion

        //คลังสำนักงานใหญ่
        if (adjustItemWarehouse.Count > 0)
        {
            #region Re-check item & qty
            List<TMItem> resItem = (from a in await _unitOfWork.Repository<TMItem>().QueryAsync()
                                    where adjustItemWarehouse.Select(s => s.itemid).Contains(a.ItemID)
                                    select a).ToList();
            if (resItem.Count == 0)
            {
                throw new Exception("ไม่พบข้อมูลที่ต้องการปรับสต็อก");
            }
            #endregion

            #region Update qty in Item stock
            adjustItemWarehouse.ForEach(e =>
            {
                TMItem item = resItem.FirstOrDefault(w => w.ItemID == e.itemid);
                if (e.adjusttypeid == (int)AdjustItemType.Delete)
                {
                    int minus = item.Qty - e.qty;
                    if (item.Qty == 0 || item.Qty < e.qty || (item.Qty - e.qty < 0))
                    {
                        throw new Exception("จำนวนสินค้าไม่เพียงพอในการปรับสต็อก");
                    }
                    item.Qty -= e.qty;
                }
                else
                {
                    item.Qty += e.qty;
                }
                item.SetUpdatedBy(request.createdby);
                item.SetUpdatedDate(request.createddate);
                item.AddDomainEvent(new TMItemUpdateEvent(item));

                #region Create Adjust Item
                TTAdjustItemTransaction adjustItemTransaction = new TTAdjustItemTransaction
                {
                    AdjustTypeID = e.adjusttypeid,
                    ItemID = e.itemid,
                    BranchID = e.branchid,
                    Qty = e.qty,
                    Remark = request.remark
                };
                adjustItemTransaction.ActiveStatus();
                adjustItemTransaction.SetCreatedBy(request.createdby);
                adjustItemTransaction.SetCreatedDate(request.createddate);
                adjustItemTransaction.AddDomainEvent(new TTAdjustItemTransactionCreateEvent(adjustItemTransaction));
                adjustItemTransactionList.Add(adjustItemTransaction);
                #endregion
            });
            #endregion
        }

        //Adjust ItemInBranch
        if (adjustItemBranch.Count > 0)
        {
            #region Re-check item & qty
            List<TMItemInBranch> resItemBranch = (from a in await _unitOfWork.Repository<TMItemInBranch>().QueryAsync()
                                                  where adjustItemBranch.Select(s => s.itemid).Contains(a.ItemID)
                                                  && adjustItemBranch.Select(s => s.branchid).Contains(a.BranchID)
                                                  select a).ToList();
            if (resItemBranch.Count == 0)
            {
                throw new Exception("ไม่พบข้อมูลที่ต้องการปรับสต็อก");
            }
            #endregion

            #region Update qty in Item stock
            adjustItemBranch.ForEach(e =>
            {
                TMItemInBranch item = resItemBranch.FirstOrDefault(w => w.ItemID == e.itemid);
                if (e.adjusttypeid == (int)AdjustItemType.Delete)
                {
                    int minus = item.Qty - e.qty;
                    if (item.Qty == 0 || item.Qty < e.qty || (item.Qty - e.qty < 0))
                    {
                        throw new Exception("จำนวนสินค้าไม่เพียงพอในการปรับสต็อก");
                    }
                    item.Qty -= e.qty;
                }
                else
                {
                    item.Qty += e.qty;
                }
                item.SetUpdatedBy(request.createdby);
                item.SetUpdatedDate(request.createddate);
                item.AddDomainEvent(new TMItemInBranchUpdateEvent(item));

                #region Create Adjust Item
                TTAdjustItemTransaction adjustItemTransaction = new TTAdjustItemTransaction
                {
                    AdjustTypeID = e.adjusttypeid,
                    ItemID = e.itemid,
                    BranchID = e.branchid,
                    Qty = e.qty,
                    Remark = request.remark
                };
                adjustItemTransaction.ActiveStatus();
                adjustItemTransaction.SetCreatedBy(request.createdby);
                adjustItemTransaction.SetCreatedDate(request.createddate);
                adjustItemTransaction.AddDomainEvent(new TTAdjustItemTransactionCreateEvent(adjustItemTransaction));
                adjustItemTransactionList.Add(adjustItemTransaction);
                #endregion
            });
            #endregion
        }

        #region Add TTAdjustItemTransactions
        await _unitOfWork.Repository<TTAdjustItemTransaction>().AddRangeAsync(adjustItemTransactionList);
        #endregion

        #region Commit Transaction
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
}
