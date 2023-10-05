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

namespace CYRetailIMS.Application.Services.AdjustItemTypeService.Queries.GetAdjustItemType.v1;
public class GetAdjustItemTypeHandler : BaseService, IRequestHandler<GetAdjustItemTypeQuery, BaseResponse<List<GetAdjustItemTypeResposeDTO>>>
{
    public GetAdjustItemTypeHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetAdjustItemTypeResposeDTO>>> Handle(GetAdjustItemTypeQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TMAdjustItemType> res = await _unitOfWork.Repository<TMAdjustItemType>().FindListAsync(w => w.IsActive);
        if (!res.Any())
        {
            throw new Exception("ไม่พบข้อมูลประเภทการปรับสต๊อก");
        }
        return new BaseResponse<List<GetAdjustItemTypeResposeDTO>>
        {
            result = true,
            data = _mapper.Map<List<GetAdjustItemTypeResposeDTO>>(res),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
