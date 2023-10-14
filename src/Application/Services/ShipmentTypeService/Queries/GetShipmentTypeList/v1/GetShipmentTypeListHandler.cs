using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.PurchaseTypeService.Queries.GetPurchaseTypeList.v1;
using CYRetailIMS.Application.Services.ShipmentTypeService.Queries.GetShipmentTypeList.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ShipmentTypeService.Queries.GetShipmentTypeByID.v1;
public class GetShipmentTypeByIDtHandler : BaseService, IRequestHandler<GetShipmentTypeListCommand, BaseResponse<List<GetShipmentTypeResponseDTO>>>
{
    public GetShipmentTypeByIDtHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetShipmentTypeResponseDTO>>> Handle(GetShipmentTypeListCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<TMShipmentType> resShipmentTypes = await _unitOfWork.Repository<TMShipmentType>().QueryAsync(w => w.IsActive);
		if (!resShipmentTypes.Any())
		{
			throw new Exception("ไม่พบข้อมูล");
		}
		return new BaseResponse<List<GetShipmentTypeResponseDTO>>
		{
			result = true,
			data = _mapper.Map<List<GetShipmentTypeResponseDTO>>(resShipmentTypes),
			status = StatusCodes.Status200OK.ToString(),
			message = "Success",
			soruce = "db"
		};
	}
}
