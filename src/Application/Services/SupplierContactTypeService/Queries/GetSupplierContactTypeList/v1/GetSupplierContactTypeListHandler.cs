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

namespace CYRetailIMS.Application.Services.SupplierContactTypeService.Queries.GetSupplierContactTypeList.v1;
public class GetSupplierContactTypeListHandler : BaseService, IRequestHandler<GetSupplierContactTypeListCommand, BaseResponse<List<GetSupplierContactTypeResposeDTO>>>
{
    public GetSupplierContactTypeListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetSupplierContactTypeResposeDTO>>> Handle(GetSupplierContactTypeListCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<TMSupplierContactType> resSupplierContactTypes = await _unitOfWork.Repository<TMSupplierContactType>().QueryAsync(w => w.IsActive);
		if (!resSupplierContactTypes.Any())
		{
			throw new Exception("ไม่พบข้อมูล");
		}
		return new BaseResponse<List<GetSupplierContactTypeResposeDTO>>
		{
			result = true,
			data = _mapper.Map<List<GetSupplierContactTypeResposeDTO>>(resSupplierContactTypes),
			status = StatusCodes.Status200OK.ToString(),
			message = "Success",
			soruce = "db"
		};
	}
}
