using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMItems;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemService.Commands.DeleteItem;
public class DeleteItemHandler : BaseService, IRequestHandler<DeleteItemCommand, BaseResponse<CommandResponse>>
{
    private readonly ILog4NetLogger _log;
    public DeleteItemHandler(IMapper mapper, IUnitOfWork unitOfWork, ILog4NetLogger log) : base(mapper, unitOfWork)
    {
        _log = log;
    }

    public async Task<BaseResponse<CommandResponse>> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        _log.Info($"Invoke DeleteItemHandler request: {request.ToJson()}");
        TMItem itemEnt = await _unitOfWork.Repository<TMItem>().FirstOrDefaultAsync(w => w.ItemID == request.itemid);
        if (itemEnt == null)
        {
            throw new Exception("ไม่พบข้อมูลสินค้าในระบบ");
        }

        #region Update
        //itemEnt = _mapper.Map<TMItem>(request);
        itemEnt.IsActive = false;
        itemEnt.SetUpdatedBy();
        itemEnt.SetUpdatedDate();
        #endregion

        itemEnt.AddDomainEvent(new TMItemDeleteEvent(itemEnt));
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
