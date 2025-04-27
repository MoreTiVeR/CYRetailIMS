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

namespace CYRetailIMS.Application.Services.TransactionTypeService.Queries.GetTrasnactionList.v1;
public class GetTrasnactionByCriteriatHandler : BaseService, IRequestHandler<GetTrasnactionByCriteriaQuery, BaseResponse<List<GetTrasnactionByCriteriaResponseDTO>>>
{
    public GetTrasnactionByCriteriatHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetTrasnactionByCriteriaResponseDTO>>> Handle(GetTrasnactionByCriteriaQuery request, CancellationToken cancellationToken)
    {
        IQueryable<TMTransactionType> queryData = await _unitOfWork.Repository<TMTransactionType>().QueryAsync();
        if (request.transactiontypeid.HasValue)
        {
            queryData = queryData.Where(w => w.TransactionTypeID == request.transactiontypeid.Value);
        }

        if (!string.IsNullOrEmpty(request.transactiontypecode))
        {
            queryData = queryData.Where(w => w.TransactionTypeCode == request.transactiontypecode);
        }

        if (request.isactive.HasValue)
        {
            queryData = queryData.Where(w => w.IsActive == request.isactive.Value);
        }
        if(queryData != null && queryData.Count() == 0)
        {
            throw new Exception("Data not found");
        }
        return new BaseResponse<List<GetTrasnactionByCriteriaResponseDTO>>
        {
            result = true,
            data = _mapper.Map<List<GetTrasnactionByCriteriaResponseDTO>>(queryData.ToList()),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
