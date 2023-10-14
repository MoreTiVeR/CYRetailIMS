using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.PurchaseTypeService.Queries.GetPurchaseTypeList.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.WarehouseService.Queries.GetWarehouseList.v1;
public class GetWarehouseListHandler : BaseService, IRequestHandler<GetWarehouseListCommand, BaseResponse<List<GetWarehouseResponseDTO>>>
{
	public GetWarehouseListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
	{
	}

	public async Task<BaseResponse<List<GetWarehouseResponseDTO>>> Handle(GetWarehouseListCommand request, CancellationToken cancellationToken)
	{
		IEnumerable<TMWarehouse> resWarehouses = await _unitOfWork.Repository<TMWarehouse>().QueryAsync(w => w.IsActive);
		if (!resWarehouses.Any())
		{
			throw new Exception("ไม่พบข้อมูล");
		}
		return new BaseResponse<List<GetWarehouseResponseDTO>>
		{
			result = true,
			data = _mapper.Map<List<GetWarehouseResponseDTO>>(resWarehouses),
			status = StatusCodes.Status200OK.ToString(),
			message = "Success",
			soruce = "db"
		};
	}
}
