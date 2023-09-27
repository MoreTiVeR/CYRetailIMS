using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.RoleService.Queries.GetRoles.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.RoleService.Queries.GetRoleByID.v1;
public class GetRoleByIDHandler : BaseService, IRequestHandler<GetRoleByIDQuery, BaseResponse<GetRoleByIDResponseDTO>>
{
    public GetRoleByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetRoleByIDResponseDTO>> Handle(GetRoleByIDQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TMRole> resRole = await _unitOfWork.Repository<TMRole>().QueryAsync(w => w.RoleID == request.roleid && w.IsActive);
        if (resRole.Count() == 0)
        {
            throw new Exception("ไม่พบข้อูลสิทธิ์ผู้ใช้งาน");
        }
        return new BaseResponse<GetRoleByIDResponseDTO>
        {
            result = true,
            data = _mapper.Map<GetRoleByIDResponseDTO>(resRole.FirstOrDefault()),
            message = " Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
