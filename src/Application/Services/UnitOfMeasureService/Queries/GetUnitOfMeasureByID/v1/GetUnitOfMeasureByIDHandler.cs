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

namespace CYRetailIMS.Application.Services.UnitOfMeasureService.Queries.GetUnitOfMeasureByID.v1;
public class GetUnitOfMeasureByIDHandler : BaseService, IRequestHandler<GetUnitOfMeasureByIDQuery, BaseResponse<GetUnitOfMeasureByIDResponseDTO>>
{
    public GetUnitOfMeasureByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetUnitOfMeasureByIDResponseDTO>> Handle(GetUnitOfMeasureByIDQuery request, CancellationToken cancellationToken)
    {
        TMUnitOfMeasure resData = await _unitOfWork.Repository<TMUnitOfMeasure>().FindAsync(w => w.UnitOfMeasureID == request.unitofmeasureid && w.IsActive);
        if(resData == null)
        {
            throw new Exception("Data not found");
        }
        return new BaseResponse<GetUnitOfMeasureByIDResponseDTO>
        {
            result = true,
            data = _mapper.Map<GetUnitOfMeasureByIDResponseDTO>(resData),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
