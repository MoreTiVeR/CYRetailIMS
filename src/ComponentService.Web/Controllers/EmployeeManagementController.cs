using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.AccountAPI;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.ExternalService.DepartmentAPI;
using CYRetailIMS.Application.ExternalService.EmployeeAPI;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchList.v1;
using CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartments.v1;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee.v1;
using CYRetailIMS.Application.Services.EmployeeService.Commands.DeleteEmployee.v1;
using CYRetailIMS.Application.Services.EmployeeService.Commands.UpdateEmployee.v1;
using CYRetailIMS.Application.Services.EmployeeService.Queries.GetEmployee.v1;
using CYRetailIMS.Application.Services.ItemService.Commands.DeleteItem;
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

    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeViewModel createEmployee)
    {
        try
        {
            CreateEmployeeCommand createEmpCommand = MappingCreateEmployeeCommand(createEmployee);
            BaseResponse<CommandResponse> resCreateItem = await _employeeAPI.CreateEmployeeAsync(createEmpCommand);
            return Json(new JsonViewModel { result = resCreateItem.result, message = resCreateItem.result ? "เพิ่มข้อมูลพนักงานสำเร็จ" : $"ไม่สามารถทำรายการได้, {resCreateItem.error.error.message}" });
        }
        catch (Exception ex)
        {
            return Json(new JsonViewModel { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    public async Task<IActionResult> Edit(int empid)
    {
        BaseResponse<GetEmployeeResponseDTO> resEmp = await _employeeAPI.GetEmployeeByIDAsync(empid);
        EditEmployeeViewModel empViewData = _mapper.Map<EditEmployeeViewModel>(resEmp.data);
        BaseResponse<List<GetBranchListResponseDTO>> resBrachList = await _branchAPI.GetBranchListAsync();
        BaseResponse<List<GetDepartmentsResponseDTO>> resDepartmentList = await _departmentAPI.GetDepartmentsAsync();
        ViewBag.BranchList = resBrachList;
        ViewBag.DepartmentList = resDepartmentList;
        return View(empViewData);
    }

    [HttpPost]
    public async Task<IActionResult> SaveEditEmployee([FromBody] EditEmployeeViewModel editEmployee)
    {
        try
        {
            UpdateEmployeeCommand updateEmployeeCommand = MappingEmployeeEditData(editEmployee);
            BaseResponse<CommandResponse> resCreateItem = await _employeeAPI.UpdateEmployee(updateEmployeeCommand);
            return Json(new JsonViewModel { result = resCreateItem.result, message = resCreateItem.result ? "อัพเดทข้อมูลพนักงานสำเร็จ" : $"ไม่สามารถทำรายการได้, {resCreateItem.error.error.message}" });
        }
        catch (Exception ex)
        {
            return Json(new JsonViewModel { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteEmployee([FromBody] DeleteEmployeerViewModel delObj)
    {
        try
        {
            DeleteEmployeeCommand delEmpCommand = new DeleteEmployeeCommand { empid = delObj.empid, updatedby = base.UserProfile.rolename };
            BaseResponse<CommandResponse> resDel = await _employeeAPI.DeleteEmployee(delEmpCommand);
            return Json(new JsonViewModel { result = resDel.result, message = resDel.result ? "ลบข้อมูลพนักงานสำเร็จ" : $"ไม่สามารถทำรายการได้, {resDel.error.error.message}" });
        }
        catch (Exception ex)
        {
            return Json(new JsonViewModel { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    #region CreateEmployeeCommand
    private CreateEmployeeCommand MappingCreateEmployeeCommand(CreateEmployeeViewModel createEmployeeViewModel)
    {
        createEmployeeViewModel.CreatedBy = base.UserProfile.username;
        createEmployeeViewModel.CreatedDate = DateTime.Now;
        CreateEmployeeCommand createData = _mapper.Map<CreateEmployeeCommand>(createEmployeeViewModel);
        return createData;
    }

    private UpdateEmployeeCommand MappingEmployeeEditData(EditEmployeeViewModel empViewData)
    {
        empViewData.UpdatedBy = base.UserProfile.rolename;
        empViewData.UpdatedDate = DateTime.Now;
        return _mapper.Map<UpdateEmployeeCommand>(empViewData);
    }
    #endregion
}
