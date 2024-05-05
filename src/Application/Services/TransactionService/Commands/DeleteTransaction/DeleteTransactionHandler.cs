using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.TransactionService.EventHandlers;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TTTransactionDetails;
using CYRetailIMS.Domain.Events.TTTransactions;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.TransactionService.Commands.DeleteTransaction;
public class DeleteTransactionHandler : BaseService, IRequestHandler<DeleteTransactionCommand, BaseResponse<CommandResponse>>
{
    public DeleteTransactionHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        IQueryable<TTTransaction> resTrans = await _unitOfWork.Repository<TTTransaction>().FindWithInclude(w => w.TransactionID == request.transactionid, i => i.Include(w => w.TTTransactonDetails));
        if (resTrans == null || !resTrans.Any())
        {
            throw new Exception("Not found transaction");
        }

        resTrans.ToList().ForEach(w =>
        {
            w.IsActive = false;
            w.SetUpdatedBy(request.deletedby);
            w.SetUpdatedDate();
            w.TTTransactonDetails.ToList().ForEach(d =>
            {
                d.IsActive = false;
                d.AddDomainEvent(new TTTransactionDetailDeleteEvent(d));
            });
            w.AddDomainEvent(new TTTransactionsDeleteEvent(w));
        });
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<CommandResponse>
        {
            result = true,
            message = "Success",
            soruce = "DB",
            status = StatusCodes.Status200OK.ToString(),
            data = new CommandResponse { result = true }
        };
    }
}
