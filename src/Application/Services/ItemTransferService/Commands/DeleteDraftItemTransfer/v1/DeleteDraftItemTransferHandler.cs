using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TTDraftItemTransferDetails;
using CYRetailIMS.Domain.Events.TTDraftItemTransfers;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.DeleteDraftItemTransfer.v1;
public class DeleteDraftItemTransferHandler : BaseService, IRequestHandler<DeleteDraftItemTransferCommand, BaseResponse<CommandResponse>>
{
    public DeleteDraftItemTransferHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(DeleteDraftItemTransferCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<TTDraftItemTransfer> resDraftItem = await _unitOfWork.Repository<TTDraftItemTransfer>().FindWithInclude(w => w.TransferHeaderID == request.draftid,
            i => i.Include(s => s.TTDraftItemTransferDetails));
        if(!resDraftItem.Any())
        {
            throw new Exception("Data not found");
        }
        TTDraftItemTransfer draftItemTransfer = resDraftItem.FirstOrDefault();
        draftItemTransfer.DeActiveStatus();
        draftItemTransfer.TransferStatus = (int)EnumModel.TransferStatus.Cancel;
        draftItemTransfer.SetUpdatedBy(request.updatedby);
        draftItemTransfer.SetUpdatedDate();
        draftItemTransfer.TTDraftItemTransferDetails.ToList().ForEach(e =>
        {
            e.DeActiveStatus();
            e.SetUpdatedBy(request.updatedby);
            e.SetUpdatedDate();
            e.AddDomainEvent(new TTDraftItemTransferDetailUpdateEvent(e));
        });
        draftItemTransfer.AddDomainEvent(new TTDraftItemTransferDeleteEvent(draftItemTransfer));
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
}
