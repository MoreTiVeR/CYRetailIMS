using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemByID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartments.v1;
public class GetDepartmentsHandler : BaseService, IRequestHandler<GetDepartmentQuery, BaseResponse<List<GetDepartmentsResponseDTO>>>
{
    public GetDepartmentsHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetDepartmentsResponseDTO>>> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TMDepartment> resDepartments = await _unitOfWork.Repository<TMDepartment>().QueryAsync(w => w.IsActive);
        if(resDepartments.Count() == 0)
        {
            throw new Exception("ไม่พบข้อมูลแผนก");
        }
        return new BaseResponse<List<GetDepartmentsResponseDTO>>
        {
            result = true,
            data = _mapper.Map<List<GetDepartmentsResponseDTO>>(resDepartments.ToList()).OrderBy(w => w.departmentid).ToList(),
            message = " Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
