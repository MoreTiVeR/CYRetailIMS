using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TTDraftItemTransfers;
using CYRetailIMS.Domain.Events.TTItemTransfers;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateDraftItemTransfer;
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
        #region Craete TTItemTransfer
        ICollection<TTDraftItemTransfer> itemTransferEntities = PrepreTTItemTransfer(request);
        itemTransferEntities.ToList().ForEach(e =>
        {
            e.AddDomainEvent(new TTDraftItemTransferCreateEvent(e));
        });
        await _unitOfWork.Repository<TTDraftItemTransfer>().AddRangeAsync(itemTransferEntities);
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

    private ICollection<TTDraftItemTransfer> PrepreTTItemTransfer(CreateDraftItemTransferCommand draftItemTransfer)
    {
        return (from a in draftItemTransfer.items
                let t = draftItemTransfer
                select new TTDraftItemTransfer
                {
                    TransferTypeID = t.transfertypeid,
                    SourceID = t.sourceid,
                    DestinationID = t.destinationid,
                    ItemID = a.itemid,
                    Qty = a.qty,
                    Description = t.description,
                    CreatedBy = t.createdby,
                    CreatedDate = draftItemTransfer.createddate,
                    IsActive = t.isactive,
                    TransferStatus = t.transferstatus
                }).ToList();
    }
}
