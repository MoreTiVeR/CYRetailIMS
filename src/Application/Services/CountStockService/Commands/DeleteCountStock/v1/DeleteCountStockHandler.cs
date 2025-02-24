using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TTCountStockDetails;
using CYRetailIMS.Domain.Events.TTCountStocks;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.DeleteCountStock.v1;
public class DeleteCountStockHandler : BaseService, IRequestHandler<DeleteCountStockCommand, BaseResponse<CommandResponse>>
{
    public DeleteCountStockHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(DeleteCountStockCommand request, CancellationToken cancellationToken)
    {
        var resCountStock = await _unitOfWork.Repository<TTCountStock>().FindWithInclude(w => w.CountStockID == request.countstockid,
            i => i.Include(x => x.TTCountStockDetails));

        if(resCountStock == null || !resCountStock.Any())
        {
            throw new Exception("ไม่พบข้อมูลนับสต๊อก");
        }

        DateTime deletedDate = DateTime.Now;

        TTCountStock countStockEnt = resCountStock.FirstOrDefault();
        countStockEnt.DeActiveStatus();
        countStockEnt.UpdatedDate = deletedDate;
        countStockEnt.UpdatedBy = request.deletedby;
        countStockEnt.TTCountStockDetails.ToList().ForEach(x =>
        {
            x.DeActiveStatus();
            x.UpdatedDate = deletedDate;
            x.UpdatedBy = request.deletedby;
            x.AddDomainEvent(new TTCountStockDetailUpdateEvent(x));
        });
        countStockEnt.AddDomainEvent(new TTCountStockUpdateEvent(countStockEnt));
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
