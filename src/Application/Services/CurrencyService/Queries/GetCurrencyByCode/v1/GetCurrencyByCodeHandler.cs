using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyByID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyByCode.v1;


public class GetCurrencyByCodeHandler : BaseService, IRequestHandler<GetCurrencyByCodeCommand, BaseResponse<GetCurrencyByCodeResponseDTO>>
{
    public GetCurrencyByCodeHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetCurrencyByCodeResponseDTO>> Handle(GetCurrencyByCodeCommand request, CancellationToken cancellationToken)
    {
		IEnumerable<TMCurrency> resCurrencyList = await _unitOfWork.Repository<TMCurrency>().QueryAsync(w => w.CurrencyCode == request.currencycode && w.IsActive);
		if (!resCurrencyList.Any())
		{
			throw new Exception("ไม่พบข้อมูลสกุลเงิน");
		}
		return new BaseResponse<GetCurrencyByCodeResponseDTO>
		{
			result = true,
			data = _mapper.Map<GetCurrencyByCodeResponseDTO>(resCurrencyList.FirstOrDefault()),
			soruce = "db",
			message = "Success",
			status = StatusCodes.Status200OK.ToString()
		};
	}
}
