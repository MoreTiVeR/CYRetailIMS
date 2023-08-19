using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemService.EventHandlers;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMItems;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;
public class CreateItemHandler : BaseService, IRequestHandler<CreateItemCommand, BaseResponse<CommandResponse>>
{
    private readonly ILog4NetLogger _log;
    public CreateItemHandler(IMapper mapper, IUnitOfWork unitOfWork, ILog4NetLogger log4NetLogger) : base(mapper, unitOfWork)
    {
        _log = log4NetLogger;
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        _log.Info($"Invoke CreateItemHandler request: {request.ToJson()}");

        TMItem isExistItem = await _unitOfWork.Repository<TMItem>().FirstOrDefaultAsync(w => w.ItemCode.Trim().Equals(request.itemcode));
        if(isExistItem != null)
        {
            throw new Exception("มีข้อมูลสินค้านี้แล้วในระบบ");
        }

        TMItem itemEnt = _mapper.Map<TMItem>(request);
        itemEnt.SetCreatedDate();
        itemEnt.AddDomainEvent(new TMItemCreateEvent(itemEnt));
        _unitOfWork.Repository<TMItem>().Add(itemEnt);

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
