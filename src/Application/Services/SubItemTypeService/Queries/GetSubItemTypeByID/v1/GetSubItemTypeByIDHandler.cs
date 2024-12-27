using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeList.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeByID.v1;
public class GetSubItemTypeByIDHandler : BaseService, IRequestHandler<GetSubItemTypeByIDQuery, BaseResponse<GetSubItemTypeResponseDTO>>
{
    public GetSubItemTypeByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetSubItemTypeResponseDTO>> Handle(GetSubItemTypeByIDQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.Repository<TMSubItemType>().FirstOrDefaultAsync(w => w.SubItemTypeID == request.subitemtypid);
        if (result == null)
        {
            throw new Exception("ไม่ม่พบข้อมูล");
        }
        return new BaseResponse<GetSubItemTypeResponseDTO>
        {
            result = true,
            data = _mapper.Map<GetSubItemTypeResponseDTO>(result),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
