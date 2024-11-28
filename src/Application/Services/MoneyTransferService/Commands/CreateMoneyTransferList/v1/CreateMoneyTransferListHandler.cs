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

namespace CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransferList.v1;
public class CreateMoneyTransferListHandler : BaseService, IRequestHandler<CreateMoneyTransferListCommand, BaseResponse<CommandResponse>>
{
    public CreateMoneyTransferListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreateMoneyTransferListCommand request, CancellationToken cancellationToken)
    {
        HashSet<TTMoneyTransfer> moneyTransfers = new HashSet<TTMoneyTransfer>();
        DateTime createdDate = DateTime.Now;
        request.mtransferdata.ForEach(e =>
        {
            TTMoneyTransfer mTransferEnt = _mapper.Map<TTMoneyTransfer>(e);
            mTransferEnt.SetCreatedDate(createdDate);
            mTransferEnt.AddDomainEvent(new TTMoneyTransferCreateEvent(mTransferEnt));
            moneyTransfers.Add(mTransferEnt);
        });
        await _unitOfWork.Repository<TTMoneyTransfer>().AddRangeAsync(moneyTransfers);
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
