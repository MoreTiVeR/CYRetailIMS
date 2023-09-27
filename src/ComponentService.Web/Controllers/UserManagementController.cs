using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.ExternalService.EmployeeAPI;
using CYRetailIMS.Application.ExternalService.UserAPI;
using CYRetailIMS.Application.ExternalService.UserRoleAPI;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchList.v1;
using CYRetailIMS.Application.Services.RoleService.Queries.GetRoles.v1;
using CYRetailIMS.Application.Services.UserService.Queries.GetUser.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin)]
public class UserManagementController : BaseController
{
    private readonly IEmployeeAPI _employeeAPI;
    private readonly IBranchAPI _branchAPI;
    private readonly IUserRoleAPI _userRoleAPI;
    private readonly IUserAPI _userAPI;
    public UserManagementController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IEmployeeAPI employeeAPI,
        IBranchAPI branchAPI,
        IUserRoleAPI userRoleAPI,
        IUserAPI userAPI) : base(httpClientRequest, mapper, log)
    {
        _employeeAPI = employeeAPI;
        _branchAPI = branchAPI;
        _userRoleAPI = userRoleAPI;
        _userAPI = userAPI;
    }

    public async Task<IActionResult> Index()
    {
        BaseResponse<List<GetUserResponseDTO>> resUserList = await _userAPI.GetUsersAsync();
        ViewBag.UserList = resUserList;
        return View();
    }

    public async Task<IActionResult> CreateAsync()
    {
        BaseResponse<List<GetBranchListResponseDTO>> resBrachList = await _branchAPI.GetBranchListAsync();
        BaseResponse<List<GetRolesResponseDTO>> resRoleList = await _userRoleAPI.GetRolesAsync();
        ViewBag.BranchList = resBrachList;
        ViewBag.RoleList = resRoleList;
        return View();
    }

    public IActionResult Edit(int empid)
    {
        return View();
    }
}
