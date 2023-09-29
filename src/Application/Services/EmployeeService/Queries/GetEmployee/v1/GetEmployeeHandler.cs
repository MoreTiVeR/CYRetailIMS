using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.RoleService.Queries.GetRoleByID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.EmployeeService.Queries.GetEmployee.v1;
public class GetEmployeeHandler : BaseService, IRequestHandler<GetEmployeeQuery, BaseResponse<List<GetEmployeeResponseDTO>>>
{
    public GetEmployeeHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetEmployeeResponseDTO>>> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
    {
        List<GetEmployeeResponseDTO> resEmp = (from emp in await _unitOfWork.Repository<TMEmployee>().QueryAsync()
                                               join department in await _unitOfWork.Repository<TMDepartment>().QueryAsync() on emp.DepartmentID equals department.DepartmentID
                                               join c in await _unitOfWork.Repository<TMUsers>().QueryAsync() on emp.UserID equals c.UserID
                                               into jUser
                                               from user in jUser.DefaultIfEmpty()
                                               //where emp.IsActive
                                               select new { emp, department, user }).ToList().Select(s => new GetEmployeeResponseDTO
                                               {
                                                   empid = s.emp.EmpID,
                                                   empcode = s.emp.EmpCode,
                                                   username = s.user == null ? null : s.user.UserName,
                                                   departmentid = s.emp.DepartmentID,
                                                   departmentname = s.department.DepartmentName,
                                                   firstname = s.emp.FirstName,
                                                   lastname = s.emp.LastName,
                                                   email = s.emp.Email,
                                                   mobileno = s.emp.MobileNo,
                                                   nickname = s.emp.NickName,
                                                   salary = s.emp.Salary,
                                                   startworkingdate = s.emp.StartWorkingDate,
                                                   createdby = s.emp.CreatedBy,
                                                   creadeddate = s.emp.CreadedDate,
                                                   isactive = s.emp.IsActive,
                                                   isregister = s.user == null ? false : true
                                               }).ToList();
        if (!resEmp.Any())
        {
            throw new Exception("ไม่พบข้อมูลพนักงาน");
        }
        return new BaseResponse<List<GetEmployeeResponseDTO>>
        {
            result = true,
            data = resEmp.OrderBy(w => w.empid).ToList(),
            message = " Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
