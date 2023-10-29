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

namespace CYRetailIMS.Application.Services.ItemService.Commands.UpdateItem;
public class UpdateItemHandler : BaseService, IRequestHandler<UpdateItemCommand, BaseResponse<CommandResponse>>
{
    private readonly ILog4NetLogger _log;

    public UpdateItemHandler(IMapper mapper, IUnitOfWork unitOfWork, ILog4NetLogger log) : base(mapper, unitOfWork)
    {
        _log = log;
    }

    public async Task<BaseResponse<CommandResponse>> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        _log.Info($"Invoke UpdateItemHandler request: {request.ToJson()}");
        TMItem itemEnt = await _unitOfWork.Repository<TMItem>().FirstOrDefaultAsync(w => w.ItemID == request.itemid);
        if (itemEnt == null)
        {
            throw new Exception("ไม่พบข้อมูลสินค้าในระบบ");
        }

        #region Update
        //itemEnt = _mapper.Map<TMItem>(request);
        itemEnt.Name = request.name;
        itemEnt.ShortName = request.shortname;
        itemEnt.BarCode = !string.IsNullOrEmpty(request.barcode) ? request.barcode : null;
        itemEnt.Description = request.description;
        itemEnt.Price = request.price;
        itemEnt.Qty = request.qty;
        itemEnt.NotifyMinQty = request.notifyqty;
        itemEnt.DiscountPercent = request.discountpercent;
        itemEnt.ItemImageUrl = !string.IsNullOrEmpty(request.itemimageurl) ? request.itemimageurl : null;
        itemEnt.SetUpdatedBy(request.updatedby);
        itemEnt.SetUpdatedDate();
        #endregion


        itemEnt.AddDomainEvent(new TMItemUpdateEvent(itemEnt));
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
