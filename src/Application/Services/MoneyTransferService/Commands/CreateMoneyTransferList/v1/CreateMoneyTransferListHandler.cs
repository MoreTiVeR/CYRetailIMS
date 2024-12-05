using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TTMoneyTransfers;
using CYRetailIMS.Domain.Events.TTMoneyTransferSlips;
using CYRetailIMS.Domain.Events.TTMoneyTransferSlipsDetails;
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

        TTMoneyTransferSlip slipEnt = null;
        if (request.transferslipdetail != null && request?.transferslipdetail.Count > 0)
        {
            slipEnt = new TTMoneyTransferSlip
            {
                TotalAmountTransfer = request.mtransferdata.Sum(s => s.amounttransfer),
                CreatedBy = request.mtransferdata.FirstOrDefault().createdby,
                CreatedDate = createdDate,
                IsActive = true
            };
            request.transferslipdetail.ForEach(f =>
            {
                TTMoneyTransferSlipsDetail fileDetail = new TTMoneyTransferSlipsDetail
                {
                    SlipImagePath = f.slipimagepath,
                    CreatedBy = request.mtransferdata.FirstOrDefault().createdby,
                    CreatedDate = createdDate,
                    IsActive = true
                };
                fileDetail.AddDomainEvent(new TTMoneyTransferSlipsDetailCreateEvent(fileDetail));
                slipEnt.TTMoneyTransferSlipsDetails.Add(fileDetail);
            });
            
        }

        request.mtransferdata.ForEach(e =>
        {
            TTMoneyTransfer mTransferEnt = _mapper.Map<TTMoneyTransfer>(e);
            mTransferEnt.SetCreatedDate(createdDate);
            mTransferEnt.AddDomainEvent(new TTMoneyTransferCreateEvent(mTransferEnt));
            moneyTransfers.Add(mTransferEnt);
        });

        if (slipEnt != null)
        {
            moneyTransfers.ToList().ForEach(moneyTransferEnt =>
            {
                slipEnt.TTMoneyTransfers.Add(moneyTransferEnt);
            });
            slipEnt.AddDomainEvent(new TTMoneyTransferSlipCreateEvent(slipEnt));
            await _unitOfWork.Repository<TTMoneyTransferSlip>().AddAsync(slipEnt);
        }

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
