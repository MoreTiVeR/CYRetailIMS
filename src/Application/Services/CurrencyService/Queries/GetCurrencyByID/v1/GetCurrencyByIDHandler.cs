using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyList.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyByID.v1;
public class GetCurrencyByIDHandler : BaseService, IRequestHandler<GetCurrencyByIDCommand, BaseResponse<GetCurrencyByIDResponseDTO>>
{
    public GetCurrencyByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetCurrencyByIDResponseDTO>> Handle(GetCurrencyByIDCommand request, CancellationToken cancellationToken)
    {
		IEnumerable<TMCurrency> resCurrencyList = await _unitOfWork.Repository<TMCurrency>().QueryAsync(w => w.CurrencyID == request.currencyid && w.IsActive);
		if (!resCurrencyList.Any())
		{
			throw new Exception("ไม่พบข้อมูลสกุลเงิน");
		}
		return new BaseResponse<GetCurrencyByIDResponseDTO>
		{
			result = true,
			data = _mapper.Map<GetCurrencyByIDResponseDTO>(resCurrencyList.FirstOrDefault()),
			soruce = "db",
			message = "Success",
			status = StatusCodes.Status200OK.ToString()
		};
	}
}
