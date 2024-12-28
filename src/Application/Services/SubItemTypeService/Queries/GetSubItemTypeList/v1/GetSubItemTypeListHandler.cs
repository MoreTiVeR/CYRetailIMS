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

namespace CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeList.v1;
public class GetSubItemTypeListHandler : BaseService, IRequestHandler<GetSubItemTypeListQuery, BaseResponse<List<GetSubItemTypeResponseDTO>>>
{
    public GetSubItemTypeListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetSubItemTypeResponseDTO>>> Handle(GetSubItemTypeListQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.Repository<TMSubItemType>().GetAllAsync();
        if (!result.Any())
        {
            throw new Exception("ไม่ม่พบข้อมูล");
        }
        return new BaseResponse<List<GetSubItemTypeResponseDTO>>
        {
            result = true,
            data = _mapper.Map<List<GetSubItemTypeResponseDTO>>(result),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
