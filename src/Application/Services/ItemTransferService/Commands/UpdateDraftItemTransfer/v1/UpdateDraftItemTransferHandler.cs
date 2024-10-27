using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TTDraftItemTransferDetails;
using CYRetailIMS.Domain.Events.TTDraftItemTransfers;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.UpdateDraftItemTransfer.v1;
public class UpdateDraftItemTransferHandler : BaseService, IRequestHandler<UpdateDraftItemTransferCommand, BaseResponse<CommandResponse>>
{
    public UpdateDraftItemTransferHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(UpdateDraftItemTransferCommand request, CancellationToken cancellationToken)
    {
        #region Validate Draft id
        IEnumerable<TTDraftItemTransfer> resDraftItemTransfer = await _unitOfWork.Repository<TTDraftItemTransfer>()
            .FindWithInclude(w => w.TransferHeaderID == request.draftid && w.IsActive, i => i.Include(s => s.TTDraftItemTransferDetails));
        if (!resDraftItemTransfer.Any())
        {
            throw new Exception("ไม่พบข้อมูลร่างโอนสินค้า");
        }
        #endregion

        DateTime updateDate = DateTime.Now;
        TTDraftItemTransfer itemTransferEntities = resDraftItemTransfer.FirstOrDefault();
        List<int> itemList = itemTransferEntities.TTDraftItemTransferDetails.Select(s => s.ItemID).ToList();
        itemTransferEntities.TTDraftItemTransferDetails = PrepareDrafItemDetail(request, itemTransferEntities.CreatedBy, itemTransferEntities.CreatedDate, updateDate, itemList);
        itemTransferEntities.SetUpdatedBy(request.createdby);
        itemTransferEntities.SetUpdatedDate(updateDate);
        itemTransferEntities.AddDomainEvent(new TTDraftItemTransferUpdateEvent(itemTransferEntities));
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse { result = true },
            status = StatusCodes.Status200OK.ToString(),
            message = "Success",
            soruce = "db"
        };
    }

    private List<TTDraftItemTransferDetail> PrepareDrafItemDetail(UpdateDraftItemTransferCommand reqObj,
        string createdBy, DateTime createdDate, DateTime updateDate, List<int> currentDraftItemIDList)
    {
        List<TTDraftItemTransferDetail> draftItemTransferDetails = new List<TTDraftItemTransferDetail>();
        reqObj.items.ForEach(e =>
        {
            bool isUpdate = currentDraftItemIDList.Contains(e.itemid);
            TTDraftItemTransferDetail detail = new TTDraftItemTransferDetail
            {
                TransferHeaderID = reqObj.draftid,
                ItemID = e.itemid,
                Qty = e.qty,
                CreatedDate = reqObj.createddate,
                CreatedBy = reqObj.createdby,
                IsActive = true
                //UpdatedBy = isUpdate == true ? reqObj.createdby : null,
                //UpdatedDate = isUpdate == true ? reqObj.createddate : null,
            };
            if (isUpdate)
            {
                detail.CreatedDate = createdDate;
                detail.CreatedBy = createdBy;
                detail.UpdatedDate = updateDate;
                detail.UpdatedBy = createdBy;
                detail.AddDomainEvent(new TTDraftItemTransferDetailUpdateEvent(detail));
            }
            else
            {
                detail.AddDomainEvent(new TTDraftItemTransferDetailCreateEvent(detail));
            }            
            draftItemTransferDetails.Add(detail);
        });
        return draftItemTransferDetails;
    }
}
