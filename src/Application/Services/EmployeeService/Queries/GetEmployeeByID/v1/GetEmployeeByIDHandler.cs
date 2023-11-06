using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.EmployeeService.Queries.GetEmployee.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.EmployeeService.Queries.GetEmployeeByID.v1;
public class GetEmployeeByIDHandler : BaseService, IRequestHandler<GetEmployeeByIDQuery, BaseResponse<GetEmployeeResponseDTO>>
{
    public GetEmployeeByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetEmployeeResponseDTO>> Handle(GetEmployeeByIDQuery request, CancellationToken cancellationToken)
    {
        GetEmployeeResponseDTO resEmp = (from emp in await _unitOfWork.Repository<TMEmployee>().QueryAsync()
                                         join department in await _unitOfWork.Repository<TMDepartment>().QueryAsync() on emp.DepartmentID equals department.DepartmentID
                                         join c in await _unitOfWork.Repository<TMUsers>().QueryAsync() on emp.UserID equals c.UserID
                                         into jUser
                                         from user in jUser.DefaultIfEmpty()
                                         where emp.EmpID == request.empid
                                         && emp.IsActive
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
                                             createddate = s.emp.CreatedDate,
                                             isactive = s.emp.IsActive,
                                             isregister = s.user == null ? false : true
                                         }).FirstOrDefault();
        if (resEmp == null)
        {
            throw new Exception("ไม่พบข้อมูลพนักงาน");
        }
        return new BaseResponse<GetEmployeeResponseDTO>
        {
            result = true,
            data = resEmp,
            message = " Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
