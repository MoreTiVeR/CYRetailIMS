
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMReceiveTemplates;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ReceiveTempService.Commands.CreateReceipt.v1;
public class CreateReceiptHandler : BaseService, IRequestHandler<CreateReceiptCommand, BaseResponse<CommandResponse>>
{
    public CreateReceiptHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreateReceiptCommand request, CancellationToken cancellationToken)
    {
        if(request.transactionid <= 0)
        {
            throw new ValidationException("Transaction ID is required");
        }
        if(string.IsNullOrEmpty(request.receiptno))
        {
            throw new ValidationException("Receipt No is required");
        }

        TTReceipt receiptEnt = new TTReceipt
        {
            TransactionID = request.transactionid,
            ReceiptNo = request.receiptno,
            CreatedBy = request.createdby,
            CreatedDate = DateTime.Now
        };
        receiptEnt.AddDomainEvent(new TTReceiptCreateEvent(receiptEnt));
        await _unitOfWork.Repository<TTReceipt>().AddAsync(receiptEnt);
        await _unitOfWork.SaveChangesAsync();

        var response = new BaseResponse<CommandResponse>
        {
            result = true,
            message = "Create Receipt successfully",
            data = new CommandResponse
            {
                result = true,
            },
            soruce = "DB",
            status = StatusCodes.Status200OK.ToString()
        };
        return response;
    }
}
