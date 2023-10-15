using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.PaymentTypeService.Queries.GetPaymentTypeList.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.PaymentTypeService.Queries.PaymentTypeByID.v1;
public class PaymentTypeByIDHandler : BaseService, IRequestHandler<PaymentTypeByIDCommand, BaseResponse<PaymentTypeByIDResponseDTO>>
{
    public PaymentTypeByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<PaymentTypeByIDResponseDTO>> Handle(PaymentTypeByIDCommand request, CancellationToken cancellationToken)
    {
		IEnumerable<TMPaymentType> tmpaymentList = await _unitOfWork.Repository<TMPaymentType>().QueryAsync(w => w.PaymenTypeID == request.paymenttypeid && w.IsActive);
		if (!tmpaymentList.Any())
		{
			throw new Exception("ไม่พบข้อมูล");
		}
		return new BaseResponse<PaymentTypeByIDResponseDTO>
		{
			result = true,
			data = _mapper.Map<PaymentTypeByIDResponseDTO>(tmpaymentList.FirstOrDefault()),
			status = StatusCodes.Status200OK.ToString(),
			message = "Success",
			soruce = "db"
		};
	}
}
