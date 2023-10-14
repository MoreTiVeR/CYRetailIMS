using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.SupplierContactTypeService.Queries.GetSupplierContactTypeList.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.SupplierContactTypeService.Queries.GetSupplierContactTypeByID.v1;


public class GetSupplierContactTypeByIDHandler : BaseService, IRequestHandler<GetSupplierContactTypeByIDCommand, BaseResponse<GetSupplierContactTypeResposeDTO>>
{
    public GetSupplierContactTypeByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetSupplierContactTypeResposeDTO>> Handle(GetSupplierContactTypeByIDCommand request, CancellationToken cancellationToken)
    {
		IEnumerable<TMSupplierContactType> resSupplierContactTypes = await _unitOfWork.Repository<TMSupplierContactType>().QueryAsync(w => 
		w.SupplierContactTypeID == request.suppliercontacttypeid &&  w.IsActive);
		if (!resSupplierContactTypes.Any())
		{
			throw new Exception("ไม่พบข้อมูล");
		}
		return new BaseResponse<GetSupplierContactTypeResposeDTO>
		{
			result = true,
			data = _mapper.Map<GetSupplierContactTypeResposeDTO>(resSupplierContactTypes.FirstOrDefault()),
			status = StatusCodes.Status200OK.ToString(),
			message = "Success",
			soruce = "db"
		};
	}
}
