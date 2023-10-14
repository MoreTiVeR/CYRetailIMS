using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.PaymentTypeService.Queries.PaymentTypeByID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.PurchaseTypeService.Queries.GetPurchaseTypeList.v1;

public class GetPurchaseTypeListHandler : BaseService, IRequestHandler<GetPurchaseTypeListCommand, BaseResponse<List<GetPurchaseTypeResponseDTO>>>
{
    public GetPurchaseTypeListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetPurchaseTypeResponseDTO>>> Handle(GetPurchaseTypeListCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<TMPurchaseType> resPaymntType = await _unitOfWork.Repository<TMPurchaseType>().QueryAsync(w => w.IsActive);
		if (!resPaymntType.Any())
		{
			throw new Exception("ไม่พบข้อมูล");
		}
		return new BaseResponse<List<GetPurchaseTypeResponseDTO>>
		{
			result = true,
			data = _mapper.Map<List<GetPurchaseTypeResponseDTO>>(resPaymntType),
			status = StatusCodes.Status200OK.ToString(),
			message = "Success",
			soruce = "db"
		};
	}
}
