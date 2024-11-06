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

namespace CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransfer.v1;
public class CreateMoneyTransferHandler : BaseService, IRequestHandler<CreateMoneyTransferCommand, BaseResponse<CommandResponse>>
{
    public CreateMoneyTransferHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreateMoneyTransferCommand request, CancellationToken cancellationToken)
    {
        TTMoneyTransfer mTransferEnt = _mapper.Map<TTMoneyTransfer>(request);
        mTransferEnt.AddDomainEvent(new TTMoneyTransferCreateEvent(mTransferEnt));
        await _unitOfWork.Repository<TTMoneyTransfer>().AddAsync(mTransferEnt);
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
