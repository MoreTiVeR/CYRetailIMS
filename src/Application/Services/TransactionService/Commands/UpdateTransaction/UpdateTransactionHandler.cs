using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TTTransactions;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.TransactionService.Commands.UpdateTransaction;
public class UpdateTransactionHandler : BaseService, IRequestHandler<UpdateTransactionCommand, BaseResponse<CommandResponse>>
{
    public UpdateTransactionHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var resTran = await _unitOfWork.Repository<TTTransaction>().FirstOrDefaultAsync(w => w.TransactionID == request.transactionid);
        if (resTran == null)
        {
            throw new Exception("ไม่พบข้อมูลในการทำรายการ!");
        }
        resTran.TransactionDate = request.transactiondate;
        resTran.SetUpdatedBy(request.updatedby);
        resTran.SetUpdatedDate();
        resTran.AddDomainEvent(new TTTransactionsUpdateEvent(resTran));
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<CommandResponse>
        {
            result = true,
            message = "ปรับปรุงวันที่ทำรายการสำเร็จ",
            soruce = "DB",
            status = StatusCodes.Status200OK.ToString(),
            data = new CommandResponse { result = true }
        };
    }
}
