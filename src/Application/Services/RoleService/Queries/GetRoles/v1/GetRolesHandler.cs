using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartments.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.RoleService.Queries.GetRoles.v1;
public class GetRolesHandler : BaseService, IRequestHandler<GetRolesQuery, BaseResponse<List<GetRolesResponseDTO>>>
{
    public GetRolesHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetRolesResponseDTO>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TMRole> resRole = await _unitOfWork.Repository<TMRole>().QueryAsync(w => w.IsActive);
        if(resRole.Count() == 0)
        {
            throw new Exception("ไม่พบข้อูลสิทธิ์ผู้ใช้งาน");
        }
        return new BaseResponse<List<GetRolesResponseDTO>>
        {
            result = true,
            data = _mapper.Map<List<GetRolesResponseDTO>>(resRole.ToList()).OrderBy(w => w.roleid).ToList(),
            message = " Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
