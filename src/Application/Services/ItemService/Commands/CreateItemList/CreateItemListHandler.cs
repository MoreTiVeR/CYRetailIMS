using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemService.Commands.UpdateItem;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMItems;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemService.Commands.CreateItemList;
public class CreateItemListHandler : BaseService, IRequestHandler<CreateItemListCommand, BaseResponse<CommandResponse>>
{
	private readonly ILog4NetLogger _log;
	public CreateItemListHandler(IMapper mapper, IUnitOfWork unitOfWork, ILog4NetLogger log4NetLogger) : base(mapper, unitOfWork)
	{
		_log = log4NetLogger;
	}

	public async Task<BaseResponse<CommandResponse>> Handle(CreateItemListCommand request, CancellationToken cancellationToken)
	{
		DateTime createdDate = DateTime.Now;
		string createdBy = request.items.FirstOrDefault().createdby;
		//List<string> itemCodeList = request.items.Select(w => w.itemcode).ToList();
		//List<string> itemNameList = request.items.Select(w => w.name).ToList();

		#region Check duplicate ItemCode

		#endregion

		#region Check duplicate ItemName

		#endregion


		//Update
		var updateItemEntity = request.items.Where(w => w.isupdate).ToList();
		if(updateItemEntity.Count > 0)
		{
			List<string> itemCodeList = updateItemEntity.Select(s => s.itemcode).ToList();
			IEnumerable<TMItem> resUpdateItemEnt = await _unitOfWork.Repository<TMItem>().FindListAsync(w => itemCodeList.Contains(w.ItemCode));
			//List<TMItem> updateItemEntities = _mapper.Map<List<TMItem>>(updateItemEntity);
			resUpdateItemEnt.ToList().ForEach(e =>
			{
				CreateItemDetailCommand reqItem = updateItemEntity.FirstOrDefault(w => w.itemcode == e.ItemCode);
				e.ItemTypeID = reqItem.itemtypeid;
				e.BrandID = reqItem.brandid;
				e.Qty = e.Qty + reqItem.qty;
				e.Cost = reqItem.cost;
				e.Price = reqItem.price;
				e.NotifyMinQty = reqItem.notifyminqty;
				e.Description = reqItem.description;
				e.SetUpdatedBy(createdBy);
				e.SetUpdatedDate(createdDate);
				e.AddDomainEvent(new TMItemUpdateEvent(e));
			});
		}

		//Create new
		var newItemEntity = request.items.Where(w => !w.isupdate).ToList();
		if(newItemEntity.Count > 0)
		{
			List<TMItem> newItemEntities = _mapper.Map<List<TMItem>>(newItemEntity);
			newItemEntities.ForEach(e =>
			{
				e.SetCreatedBy(createdBy);
				e.SetCreatedDate(createdDate);
				e.AddDomainEvent(new TMItemCreateEvent(e));
			});
			await _unitOfWork.Repository<TMItem>().AddRangeAsync(newItemEntities);
		}

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
