using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMItemInBranchs;
using CYRetailIMS.Domain.Events.TTItemTransfers;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using static CYRetailIMS.Application.Common.Models.EnumModel;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.UpdateItemTransfer;
public class UpdateItemTransferHandler : BaseService, IRequestHandler<UpdateItemTransferCommand, BaseResponse<CommandResponse>>
{
    public UpdateItemTransferHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(UpdateItemTransferCommand request, CancellationToken cancellationToken)
    {
        #region Validate Qty In Stock
        await _unitOfWork.BeginTransactionAsync();
        #endregion

        #region Check TTItemTransfer
        TTItemTransfer resTTItemTransfer = await _unitOfWork.Repository<TTItemTransfer>().FindAsync(w => w.TransferID == request.transferid);
        if (resTTItemTransfer == null)
        {
            throw new Exception("ขออภัย, ไม่พบรายการโอน กรุลองใหม่อีกครั้ง");
        }

        if(resTTItemTransfer.TransferStatus != (int)TransferStatus.Pending)
        {
            throw new Exception("ขออภัย, ไม่สามารถทำรายการได้ เนื่องรายการโอนได้ถูกตรวจรับ/ยกเลิกไปแล้ว");
        }
        #endregion

        #region Validate Item in Destination Branch
        //IEnumerable<TMItemInBranch> resItemInDestinationBranch = await _unitOfWork.Repository<TMItemInBranch>().QueryAsync(w => w.BranchID == request.destinationid
        //&& w.ItemID == request.itemid
        //&& w.IsActive);
        IEnumerable<TMItemInBranch> resItemInDestinationBranch = await _unitOfWork.Repository<TMItemInBranch>().QueryAsync(w => w.BranchID == request.destinationid
        && w.ItemID == request.itemid);
        if (resItemInDestinationBranch.Any(w => w.IsActive == ((int)EnumModel.ItemInBranchStatus.InActive).ToBool()))
        {
            throw new Exception("ขออภัย, มีรายการสินค้าที่ถูกยกเลิกในสาขาปลายทางแล้ว! กรุณาเปิดใช้งานสินค้าและทำรายการใหม่อีกครั้ง");
        }
        #endregion

        switch (request.transferstatusid)
        {
            case (int)TransferStatus.Received:
                if (request.transferstatusid == (int)TransferStatus.Received)
                {
                    #region ปลายทางโอน ยังไม่มีสินค้าใน ItemInBranch ให้ทำการเพิ่มข้อมูลAdd/ ถ้ามีแล้ว อัพเดท Update stock in destination branch
                    if (resItemInDestinationBranch.Count() == 0)
                    {
                        //List<TMItemInBranch> tmItemInDestinationBranch = new List<TMItemInBranch>();
                        //Add/Create new item in Destination branch
                        TMItem tmItem = await _unitOfWork.Repository<TMItem>().FindAsync(w => w.ItemID == request.itemid);
                        if (tmItem == null)
                        {
                            throw new Exception("ขออภัย, ไม่พบข้อมูลสินค้าในคลังสินค้าต้นทางที่ทำรายการโอน");
                        }

                        TMItemInBranch tmItemBranch = new TMItemInBranch()
                        {
                            BranchID = request.destinationid,
                            ItemID = request.itemid,
                            DiscountPercent = 0,
                            Qty = request.receiveqty, //ใช้จำนวนที่ตรวจรับจริง
                            Price = tmItem.Price
                        };
                        tmItemBranch.SetCreatedDate(request.updateddate);
                        tmItemBranch.SetCreatedBy(request.updatedby);
                        tmItemBranch.ActiveStatus();
                        tmItemBranch.AddDomainEvent(new TMItemInBranchCreateEvent(tmItemBranch));
                        await _unitOfWork.Repository<TMItemInBranch>().AddAsync(tmItemBranch);
                    }
                    else
                    {
                        //Update Destination Stock
                        resItemInDestinationBranch.ToList().ForEach(s =>
                        {
                            s.Qty = s.Qty + request.receiveqty;
                            s.SetUpdatedDate(request.updateddate);
                            s.SetUpdatedBy(request.updatedby);
                            s.AddDomainEvent(new TMItemInBranchUpdateEvent(s));
                        });
                    }
                    #endregion

                    #region Update TTItemTransfer
                    resTTItemTransfer.ReceiveQTY = request.receiveqty;
                    resTTItemTransfer.ReturnQTY = request.returnqty;
                    resTTItemTransfer.Description = request.description;
                    resTTItemTransfer.SetUpdatedBy(request.updatedby);
                    resTTItemTransfer.SetUpdatedDate(request.updateddate);
                    resTTItemTransfer.TransferStatus = request.transferstatusid;
                    resTTItemTransfer.AddDomainEvent(new TTItemTransferUpdateEvent(resTTItemTransfer));
                    #endregion
                }
                break;
            case (int)TransferStatus.Reject:
                //คืน Stock ไปยัง TMItem sourceid ต้นทาง 
                TMItem rejectTmItem = await _unitOfWork.Repository<TMItem>().FindAsync(w => w.ItemID == request.itemid);
                if (rejectTmItem == null)
                {
                    throw new Exception("ขออภัย, ไม่พบข้อมูลในคลังสินค้าต้นทางที่ทำรายการโอน");
                }
                rejectTmItem.Qty += resTTItemTransfer.Qty;
                rejectTmItem.SetUpdatedBy(request.updatedby);
                rejectTmItem.SetUpdatedDate(request.updateddate);

                #region Update TTItemTransfer
                resTTItemTransfer.Description = request.description;
                resTTItemTransfer.SetUpdatedBy(request.updatedby);
                resTTItemTransfer.SetUpdatedDate(request.updateddate);
                resTTItemTransfer.TransferStatus = request.transferstatusid;
                resTTItemTransfer.AddDomainEvent(new TTItemTransferUpdateEvent(resTTItemTransfer));
                #endregion
                break;
            case (int)TransferStatus.Cancel:
                //คืน Stock ไปยัง TMItem sourceid ต้นทาง
                TMItem cancelTmItem = await _unitOfWork.Repository<TMItem>().FindAsync(w => w.ItemID == request.itemid);
                if (cancelTmItem == null)
                {
                    throw new Exception("ขออภัย, ไม่พบข้อมูลในคลังสินค้าต้นทางที่ทำรายการโอน");
                }
                cancelTmItem.Qty += resTTItemTransfer.Qty;
                cancelTmItem.SetUpdatedBy(request.updatedby);
                cancelTmItem.SetUpdatedDate(request.updateddate);

                #region Update TTItemTransfer
                resTTItemTransfer.Description = request.description;
                resTTItemTransfer.SetUpdatedBy(request.updatedby);
                resTTItemTransfer.SetUpdatedDate(request.updateddate);
                resTTItemTransfer.TransferStatus = request.transferstatusid;
                resTTItemTransfer.AddDomainEvent(new TTItemTransferUpdateEvent(resTTItemTransfer));
                #endregion
                break;
            default:
                break;
        }

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
}
