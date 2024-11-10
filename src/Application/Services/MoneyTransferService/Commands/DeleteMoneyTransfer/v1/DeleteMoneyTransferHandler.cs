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

namespace CYRetailIMS.Application.Services.MoneyTransferService.Commands.DeleteMoneyTransfer.v1;
public class DeleteMoneyTransferHandler : BaseService, IRequestHandler<DeleteMoneyTransferCommand, BaseResponse<CommandResponse>>
{
    public DeleteMoneyTransferHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(DeleteMoneyTransferCommand request, CancellationToken cancellationToken)
    {
        TTMoneyTransfer resMTransfer = await _unitOfWork.Repository<TTMoneyTransfer>().FirstOrDefaultAsync(w => w.MoneyTransferID == request.moeytransferid);
        if(resMTransfer == null)
        {
            throw new Exception("ไม่พบข้อมูล");
        }

        resMTransfer.SetUpdatedDate();
        resMTransfer.SetUpdatedBy(request.updatedby);
        resMTransfer.DeActiveStatus();
        resMTransfer.AddDomainEvent(new TTMoneyTransferDeleteEvent(resMTransfer));
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
