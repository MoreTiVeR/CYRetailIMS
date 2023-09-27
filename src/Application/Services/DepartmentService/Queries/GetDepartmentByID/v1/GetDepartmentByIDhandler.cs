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

namespace CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartmentByID.v1;
public class GetDepartmentByIDhandler : BaseService, IRequestHandler<GetDepartmentByIDQuery, BaseResponse<GetDepartmentByIDResponseDTO>>
{
    public GetDepartmentByIDhandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetDepartmentByIDResponseDTO>> Handle(GetDepartmentByIDQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TMDepartment> resDepartments = await _unitOfWork.Repository<TMDepartment>().QueryAsync(w => w.DepartmentID == request.departmentid && w.IsActive);
        if (resDepartments.Count() == 0)
        {
            throw new Exception("ไม่พบข้อมูลแผนก");
        }
        return new BaseResponse<GetDepartmentByIDResponseDTO>
        {
            result = true,
            data = _mapper.Map<GetDepartmentByIDResponseDTO>(resDepartments.FirstOrDefault()),
            message = " Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
