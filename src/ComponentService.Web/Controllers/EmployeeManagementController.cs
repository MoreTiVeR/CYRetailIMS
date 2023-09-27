using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.AccountAPI;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.ExternalService.DepartmentAPI;
using CYRetailIMS.Application.ExternalService.EmployeeAPI;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchList.v1;
using CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartments.v1;
using CYRetailIMS.Application.Services.EmployeeService.Queries.GetEmployee.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin)]
public class EmployeeManagementController : BaseController
{
    private readonly IEmployeeAPI _employeeAPI;
    private readonly IBranchAPI _branchAPI;
    private readonly IDepartmentAPI _departmentAPI;
    public EmployeeManagementController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IEmployeeAPI employeeAPI,
        IBranchAPI branchAPI,
        IDepartmentAPI departmentAPI) : base(httpClientRequest, mapper, log)
    {
        _employeeAPI = employeeAPI;
        _branchAPI = branchAPI;
        _departmentAPI = departmentAPI;
    }

    public async Task<IActionResult> Index()
    {
        BaseResponse<List<GetEmployeeResponseDTO>> resEmpList = await _employeeAPI.GetEmployeesAsync();
        ViewBag.EmployeeList = resEmpList;
        return View();
    }

    public async Task<IActionResult> CreateAsync()
    {
        BaseResponse<List<GetBranchListResponseDTO>> resBrachList = await _branchAPI.GetBranchListAsync();
        BaseResponse<List<GetDepartmentsResponseDTO>> resDepartmentList = await _departmentAPI.GetDepartmentsAsync();
        ViewBag.BranchList = resBrachList;
        ViewBag.DepartmentList = resDepartmentList;
        return View();
    }

    public IActionResult Edit(int empid)
    {
        return View();
    }
}
