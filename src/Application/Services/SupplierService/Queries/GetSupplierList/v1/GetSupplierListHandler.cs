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

namespace CYRetailIMS.Application.Services.SupplierService.Queries.GetSupplierList.v1;
public class GetSupplierListHandler : BaseService, IRequestHandler<GetSupplierListCommand, BaseResponse<List<GetSupplierResponseDTO>>>
{
	public GetSupplierListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
	{
	}

	public async Task<BaseResponse<List<GetSupplierResponseDTO>>> Handle(GetSupplierListCommand request, CancellationToken cancellationToken)
	{
		IEnumerable<GetSupplierResponseDTO> resSupplier = (from a in await _unitOfWork.Repository<TMSupplier>().QueryAsync()
														   join b in await _unitOfWork.Repository<TMSupplierType>().QueryAsync() on a.SupplierTypeID equals b.SupplierTypeID
														   where a.IsActive
														   select new GetSupplierResponseDTO
														   {
															   supplierid = a.SupplierID,
															   suppliername_th = a.SupplierName_TH,
															   suppliername_en = a.SupplierName_EN,
															   suppliertypeid = a.SupplierTypeID,
															   suppliertypename = b.SupplierTypeName,
															   description = a.Description,
															   createdby = a.CreatedBy,
															   creadeddate = a.CreadedDate,
															   isactive = a.IsActive
														   }).AsEnumerable();

		if (!resSupplier.Any())
		{
			throw new Exception("ไม่พบข้อมูล");
		}
		return new BaseResponse<List<GetSupplierResponseDTO>>
		{
			result = true,
			data = _mapper.Map<List<GetSupplierResponseDTO>>(resSupplier),
			status = StatusCodes.Status200OK.ToString(),
			message = "Success",
			soruce = "db"
		};
	}
}
