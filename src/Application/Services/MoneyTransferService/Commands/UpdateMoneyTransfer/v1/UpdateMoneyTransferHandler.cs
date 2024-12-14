using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TTMoneyTransfers;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.MoneyTransferService.Commands.UpdateMoneyTransfer.v1;
public class UpdateMoneyTransferHandler : BaseService, IRequestHandler<UpdateMoneyTransferCommand, BaseResponse<CommandResponse>>
{
    public UpdateMoneyTransferHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(UpdateMoneyTransferCommand request, CancellationToken cancellationToken)
    {
        TTMoneyTransfer resMoneyTransfer = await _unitOfWork.Repository<TTMoneyTransfer>().FirstOrDefaultAsync(w => w.MoneyTransferID == request.moneytransferid);
        if(resMoneyTransfer == null)
        {
            throw new Exception("ไม่พบข้อมูล");
        }

        resMoneyTransfer.BranchID = request.branchid;
        //transfer date not same
        if(DateTime.Compare(request.transferdate, resMoneyTransfer.TransferDate) != 0)
        {
            resMoneyTransfer.TransferDate = request.transferdate;
        }
        resMoneyTransfer.AmountTransfer = request.amounttransfer;
        resMoneyTransfer.Description = request.description;
        if (!string.IsNullOrEmpty(request.slipimagepath))
        {
            resMoneyTransfer.SlipImagePath = request.slipimagepath;
        }
        resMoneyTransfer.IsActive = request.isactive;
        resMoneyTransfer.SetUpdatedDate();
        resMoneyTransfer.SetUpdatedBy(request.updatedby);
        resMoneyTransfer.AddDomainEvent(new TTMoneyTransferUpdateEvent(resMoneyTransfer));
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse { result = true },
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
