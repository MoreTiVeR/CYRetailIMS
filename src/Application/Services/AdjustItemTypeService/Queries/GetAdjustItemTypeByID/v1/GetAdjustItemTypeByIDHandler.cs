using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.AdjustItemTypeService.Queries.GetAdjustItemType.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.AdjustItemTypeService.Queries.GetAdjustItemTypeByID.v1;
internal class GetAdjustItemTypeByIDHandler : BaseService, IRequestHandler<GetAdjustItemTypeByIDQuery, BaseResponse<List<GetAdjustItemTypeByIDResponseDTO>>>
{
    public GetAdjustItemTypeByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetAdjustItemTypeByIDResponseDTO>>> Handle(GetAdjustItemTypeByIDQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TMAdjustItemType> res = await _unitOfWork.Repository<TMAdjustItemType>().FindListAsync(w => w.AdjustTypeID == request.adjusttypeid && w.IsActive);
        if (!res.Any())
        {
            throw new Exception("ไม่พบข้อมูลประเภทการปรับสต๊อก");
        }
        return new BaseResponse<List<GetAdjustItemTypeByIDResponseDTO>>
        {
            result = true,
            data = _mapper.Map<List<GetAdjustItemTypeByIDResponseDTO>>(res),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
