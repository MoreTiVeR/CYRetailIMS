using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TTDraftItemTransferDetails;
using CYRetailIMS.Domain.Events.TTDraftItemTransfers;
using CYRetailIMS.Domain.Events.TTItemTransfers;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateDraftItemTransfer.v1;
internal class CreateDraftItemTransferHandler : BaseService, IRequestHandler<CreateDraftItemTransferCommand, BaseResponse<CommandResponse>>
{
    public CreateDraftItemTransferHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    /// <summary>
    /// Save to TTDraftItemTransfer
    /// No need validate
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<BaseResponse<CommandResponse>> Handle(CreateDraftItemTransferCommand request, CancellationToken cancellationToken)
    {
        #region Check Draft Item if exist
        var isExist = await _unitOfWork.Repository<TTDraftItemTransfer>().AnyAsync(w => w.DestinationBranchID == request.destinationid 
        && w.TransferStatus == (int)EnumModel.TransferStatus.Pending);
        if (isExist)
        {
            throw new Exception("ไม่สามารถทำรายการได้ เนื่องจากสาขาดังกล่าวมีการบันทึกฉบับร่างในระบบแล้ว");
        }
        #endregion

        #region Craete TTItemTransfer
        TTDraftItemTransfer itemTransferEntities = PrepreTTItemTransfer(request);
        itemTransferEntities.TTDraftItemTransferDetails = PrepreTTItemTransferDetail(request);
        itemTransferEntities.TTDraftItemTransferDetails.ToList().ForEach(e =>
        {
            e.AddDomainEvent(new TTDraftItemTransferDetailCreateEvent(e));
        });
        itemTransferEntities.AddDomainEvent(new TTDraftItemTransferCreateEvent(itemTransferEntities));
        await _unitOfWork.Repository<TTDraftItemTransfer>().AddAsync(itemTransferEntities);
        #endregion

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

    private TTDraftItemTransfer PrepreTTItemTransfer(CreateDraftItemTransferCommand createDraftItemTransfer)
    {
        return new TTDraftItemTransfer
        {
            TransferRefNo = $"{createDraftItemTransfer.createddate:ddMMyyyyHHMM}",
            TransferTypeID = createDraftItemTransfer.transfertypeid,
            Description = createDraftItemTransfer.description,
            SourceBranchID = createDraftItemTransfer.sourceid,
            DestinationBranchID = createDraftItemTransfer.destinationid,
            CreatedBy = createDraftItemTransfer.createdby,
            CreatedDate = createDraftItemTransfer.createddate,
            IsActive = createDraftItemTransfer.isactive,
            TransferStatus = createDraftItemTransfer.transferstatus
        };
    }

    private ICollection<TTDraftItemTransferDetail> PrepreTTItemTransferDetail(CreateDraftItemTransferCommand createDraftItemTransfer)
    {
        return (from a in createDraftItemTransfer.items
                let t = createDraftItemTransfer
                select new TTDraftItemTransferDetail
                {
                    ItemID = a.itemid,
                    Qty = a.qty,
                    CreatedBy = t.createdby,
                    CreatedDate = createDraftItemTransfer.createddate,
                    IsActive = t.isactive
                }).ToList();
    }


}
