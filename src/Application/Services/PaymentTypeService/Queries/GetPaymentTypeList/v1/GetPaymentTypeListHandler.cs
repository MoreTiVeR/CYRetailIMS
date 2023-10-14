using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.PaymentTypeService.Queries.GetPaymentTypeList.v1;

public class GetPaymentTypeListHandler : BaseService, IRequestHandler<GetPaymentTypeListCommand, BaseResponse<List<GetPaymentTypeListResponseDTO>>>
{
	public GetPaymentTypeListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
	{
	}

	public async Task<BaseResponse<List<GetPaymentTypeListResponseDTO>>> Handle(GetPaymentTypeListCommand request, CancellationToken cancellationToken)
	{
		IEnumerable<TMPaymentType> tmpaymentList = await _unitOfWork.Repository<TMPaymentType>().QueryAsync(w => w.IsActive);
		if (!tmpaymentList.Any())
		{
			throw new Exception("ไม่พบข้อมูล");
		}
		return new BaseResponse<List<GetPaymentTypeListResponseDTO>>
		{
			result = true,
			data = _mapper.Map<List<GetPaymentTypeListResponseDTO>>(tmpaymentList),
			status = StatusCodes.Status200OK.ToString(),
			message = "Success",
			soruce = "db"
		};
	}
}
