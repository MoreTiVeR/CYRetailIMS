using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.PurchaseTypeService.Queries.GetPurchaseTypeList.v1;
using CYRetailIMS.Application.Services.ShipmentTypeService.Queries.GetShipmentTypeByID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ShipmentTypeService.Queries.GetShipmentTypeList.v1;
public class GetShipmentTypeByIDHandler : BaseService, IRequestHandler<GetShipmentTypeByIDCommand, BaseResponse<GetShipmentTypeResponseDTO>>
{
    public GetShipmentTypeByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetShipmentTypeResponseDTO>> Handle(GetShipmentTypeByIDCommand request, CancellationToken cancellationToken)
    {
		IEnumerable<TMShipmentType> resShipmentTypes = await _unitOfWork.Repository<TMShipmentType>().QueryAsync(w => w.ShipmentTypeID == request.shipmenttypeid && w.IsActive);
		if (!resShipmentTypes.Any())
		{
			throw new Exception("ไม่พบข้อมูล");
		}
		return new BaseResponse<GetShipmentTypeResponseDTO>
		{
			result = true,
			data = _mapper.Map<GetShipmentTypeResponseDTO>(resShipmentTypes.FirstOrDefault()),
			status = StatusCodes.Status200OK.ToString(),
			message = "Success",
			soruce = "db"
		};
	}
}
