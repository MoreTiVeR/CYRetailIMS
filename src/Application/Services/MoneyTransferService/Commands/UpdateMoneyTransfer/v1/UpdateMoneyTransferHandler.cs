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
        resMoneyTransfer = _mapper.Map<TTMoneyTransfer>(request);
        resMoneyTransfer.SetUpdatedDate();
        resMoneyTransfer.SetUpdatedBy(request.updatedby);
        resMoneyTransfer.AddDomainEvent(new TTMoneyTransferUpdateEvent(resMoneyTransfer));
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse { result = true },
            message = "สำเร็จ",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
