using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.WarehouseService.Queries.GetWarehouseList.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.WarehouseService.Queries.GetWarehouseByID.v1;
public class GetWarehouseByIDHandler : BaseService, IRequestHandler<GetWarehouseByIDCommand, BaseResponse<GetWarehouseResponseDTO>>
{
    public GetWarehouseByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetWarehouseResponseDTO>> Handle(GetWarehouseByIDCommand request, CancellationToken cancellationToken)
    {
		IEnumerable<TMWarehouse> resWarehouses = await _unitOfWork.Repository<TMWarehouse>().QueryAsync(w => w.WarehouseID == request.warehouseid &&  w.IsActive);
		if (!resWarehouses.Any())
		{
			throw new Exception("ไม่พบข้อมูล");
		}
		return new BaseResponse<GetWarehouseResponseDTO>
		{
			result = true,
			data = _mapper.Map<GetWarehouseResponseDTO>(resWarehouses.FirstOrDefault()),
			status = StatusCodes.Status200OK.ToString(),
			message = "Success",
			soruce = "db"
		};
	}
}
