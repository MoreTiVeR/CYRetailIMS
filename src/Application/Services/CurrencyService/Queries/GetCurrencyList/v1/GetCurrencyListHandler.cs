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

namespace CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyList.v1;
public class GetCurrencyListHandler : BaseService, IRequestHandler<GetCurrencyListCommand, BaseResponse<List<GetCurrencyListResponseDTO>>>
{
    public GetCurrencyListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetCurrencyListResponseDTO>>> Handle(GetCurrencyListCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<TMCurrency> resCurrencyList = await _unitOfWork.Repository<TMCurrency>().QueryAsync(w => w.IsActive);
        if (!resCurrencyList.Any())
        {
            throw new Exception("ไม่พบข้อมูลสกุลเงิน");
        }
        return new BaseResponse<List<GetCurrencyListResponseDTO>>
        {
            result = true,
            data = _mapper.Map<List<GetCurrencyListResponseDTO>>(resCurrencyList),
            soruce = "db",
            message = "Success",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
