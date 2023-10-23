using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.SupplierService.Queries.GetSupplierList.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.SupplierService.Queries.GetSupplierByID.v1;
public class GetSupplierByIDHandler : BaseService, IRequestHandler<GetSupplierByIDCommand, BaseResponse<GetSupplierResponseDTO>>
{
	public GetSupplierByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
	{
	}

	public async Task<BaseResponse<GetSupplierResponseDTO>> Handle(GetSupplierByIDCommand request, CancellationToken cancellationToken)
	{
		IEnumerable<GetSupplierResponseDTO> resSupplier = (from a in await _unitOfWork.Repository<TMSupplier>().QueryAsync()
														   join contact in await _unitOfWork.Repository<TMSupplierContact>().QueryAsync() on a.SupplierID equals contact.SupplierID
														   join b in await _unitOfWork.Repository<TMSupplierType>().QueryAsync() on a.SupplierTypeID equals b.SupplierTypeID
														   join c in await _unitOfWork.Repository<TMSupplierContactType>().QueryAsync() on contact.SupplierContactTypeID equals c.SupplierContactTypeID
														   where a.SupplierID == request.supplierid && a.IsActive
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
															   isactive = a.IsActive,
															   suppliercontacttypeid = contact.SupplierContactTypeID,
                                                               suppliercontacttypename = c.SupplierContactTypeName,
                                                               contactaccountname = contact.ContactAccountName,
															   contactperson = contact.ContactPerson,
															   mobileno = contact.MobileNo,
															   contactdesctiption = contact.Description
                                                           }).AsEnumerable();

		if (!resSupplier.Any())
		{
			throw new Exception("ไม่พบข้อมูล");
		}
		return new BaseResponse<GetSupplierResponseDTO>
		{
			result = true,
			data = _mapper.Map<GetSupplierResponseDTO>(resSupplier.FirstOrDefault()),
			status = StatusCodes.Status200OK.ToString(),
			message = "Success",
			soruce = "db"
		};
	}
}
