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

namespace CYRetailIMS.Application.Services.UnitOfMeasureService.Queries.GetUnitOfMeasureList.v1;
public class GetUnitOfMeasureListHandler : BaseService, IRequestHandler<GetUnitOfMeasureListQuery, BaseResponse<List<GetUnitOfMeasureListResponseDTO>>>
{
    public GetUnitOfMeasureListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetUnitOfMeasureListResponseDTO>>> Handle(GetUnitOfMeasureListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TMUnitOfMeasure> resData = await _unitOfWork.Repository<TMUnitOfMeasure>().FindListAsync(w => w.IsActive);
        if(!resData.Any())
        {
            throw new Exception("Data not found");
        }
        return new BaseResponse<List<GetUnitOfMeasureListResponseDTO>>
        {
            result = true,
            data = _mapper.Map<List<GetUnitOfMeasureListResponseDTO>>(resData),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
