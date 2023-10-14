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

namespace CYRetailIMS.Application.Services.PurchaseTypeService.Queries.PurchaseTypeByID.v1;
internal class PurchaseTypeByIDHandler : BaseService, IRequestHandler<PurchaseTypeByIDCommand, BaseResponse<GetPurchaseTypeResponseDTO>>
{
    public PurchaseTypeByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetPurchaseTypeResponseDTO>> Handle(PurchaseTypeByIDCommand request, CancellationToken cancellationToken)
    {
		IEnumerable<TMPurchaseType> resPaymntType = await _unitOfWork.Repository<TMPurchaseType>().QueryAsync(w => w.PurchaseTypeID == request.purchasetypeid && w.IsActive);
		if (!resPaymntType.Any())
		{
			throw new Exception("ไม่พบข้อมูล");
		}
		return new BaseResponse<GetPurchaseTypeResponseDTO>
		{
			result = true,
			data = _mapper.Map<GetPurchaseTypeResponseDTO>(resPaymntType.FirstOrDefault()),
			status = StatusCodes.Status200OK.ToString(),
			message = "Success",
			soruce = "db"
		};
	}
}
