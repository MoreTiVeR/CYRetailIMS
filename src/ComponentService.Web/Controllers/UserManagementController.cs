using System.Text.RegularExpressions;
using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.ExternalService.EmployeeAPI;
using CYRetailIMS.Application.ExternalService.UserAPI;
using CYRetailIMS.Application.ExternalService.UserRoleAPI;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchList.v1;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee.v1;
using CYRetailIMS.Application.Services.EmployeeService.Commands.DeleteEmployee.v1;
using CYRetailIMS.Application.Services.EmployeeService.Commands.UpdateEmployee.v1;
using CYRetailIMS.Application.Services.EmployeeService.Queries.GetEmployee.v1;
using CYRetailIMS.Application.Services.RoleService.Queries.GetRoles.v1;
using CYRetailIMS.Application.Services.UserService.Commands.CreateUser.v1;
using CYRetailIMS.Application.Services.UserService.Commands.DeleteUser.v1;
using CYRetailIMS.Application.Services.UserService.Commands.UpdateUser.v1;
using CYRetailIMS.Application.Services.UserService.Queries.GetUser.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using CYRetailIMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.Application.Common.Models.EnumModel;
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
		BaseResponse<List<GetBranchResponseDTO>> resBrachList = await _branchAPI.GetBranchListAsync();
		BaseResponse<List<GetRolesResponseDTO>> resRoleList = await _userRoleAPI.GetRolesAsync();
		BaseResponse<List<GetEmployeeResponseDTO>> resEmpList = await _employeeAPI.GetEmployeesAsync();
		resEmpList.data = resEmpList.data.Where(w => !w.isregister && w.isactive).ToList();
		ViewBag.BranchList = resBrachList;
		ViewBag.RoleList = resRoleList;
		ViewBag.EmpList = resEmpList;
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> CreateAccount([FromBody] CreateUserViewModel regisData)
	{
		try
		{
			CreateUserCommand createAccountCommand = MappingCreateUserCommand(regisData);
			BaseResponse<CommandResponse> res = await _userAPI.CreateUser(createAccountCommand);
			return Json(new { result = res.result, message = res.result ? "ลงทะเบียนสมาชิกสำเร็จ" : $"ไม่สามารถทำรายการได้, {res.error.error.message}" });
		}
		catch (Exception ex)
		{
			return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
		}
	}

	public async Task<IActionResult> EditAsync(int userid)
	{
		BaseResponse<GetUserResponseDTO> resEmp = await _userAPI.GetUserByIDAsync(userid);
		EditUserViewModel userViewModel = _mapper.Map<EditUserViewModel>(resEmp.data);

		BaseResponse<List<GetBranchResponseDTO>> resBrachList = await _branchAPI.GetBranchListAsync();
		BaseResponse<List<GetRolesResponseDTO>> resRoleList = await _userRoleAPI.GetRolesAsync();
		ViewBag.BranchList = resBrachList;
		ViewBag.RoleList = resRoleList;
		return View(userViewModel);
	}

	[HttpPost]
	public async Task<IActionResult> SaveEditAccount([FromBody] EditUserViewModel userData)
	{
		try
		{
			if (!string.IsNullOrEmpty(userData.Password))
			{
				if (!ValidatePassword(userData.Password))
				{
					return Json(new { result = false, message = $"รุปแบบรหัสผ่านไม่ถูกต้อง, รหัสผ่านประกอบไปด้วย ตัวอักษรภาษาอังกฤษและตัวเลข ความยาวไม่เกิน10ตัวอักษร" });
				}
			}
			UpdateUserCommand updateUserCommand = MappingUpdateUserCommand(userData);
			BaseResponse<CommandResponse> res = await _userAPI.UpdateUser(updateUserCommand);
			return Json(new { result = res.result, message = res.result ? "อัพเดทข้อมูลบัญชีสมาชิกสำเร็จ" : $"ไม่สามารถทำรายการได้, {res.error.error.message}" });
		}
		catch (Exception ex)
		{
			return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
		}
	}

	[HttpPost]
	public async Task<IActionResult> DeleteUser([FromBody] DeleteUserViewModel delObj)
	{
		try
		{
			DeleteUserCommand delUserCommand = new DeleteUserCommand { userid = delObj.userid, updatedby = base.UserProfile.rolename };
			BaseResponse<CommandResponse> resDel = await _userAPI.DeleteUser(delUserCommand);
			return Json(new JsonViewModel { result = resDel.result, message = resDel.result ? "ลบข้อมูลผู้ใช้งานสำเร็จ" : $"ไม่สามารถทำรายการได้, {resDel.error.error.message}" });
		}
		catch (Exception ex)
		{
			return Json(new JsonViewModel { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
		}
	}

	private CreateUserCommand MappingCreateUserCommand(CreateUserViewModel registerViewModel)
	{
		registerViewModel.CreatedBy = base.UserProfile.username;
		registerViewModel.CreatedDate = DateTime.Now;
		registerViewModel.ApproveStatus = (int)ApproveStatus.Approve;
		CreateUserCommand res = _mapper.Map<CreateUserCommand>(registerViewModel);
		return res;
	}

	private UpdateUserCommand MappingUpdateUserCommand(EditUserViewModel editUserData)
	{
		editUserData.UpdatedBy = base.UserProfile.rolename;
		editUserData.UpdatedDate = DateTime.Now;
		return _mapper.Map<UpdateUserCommand>(editUserData);
	}

	/// <summary>
	/// Pattern
	/// </summary>
	/// <param name="password"></param>
	/// <returns></returns>
	private bool ValidatePassword(string password)
	{
		Regex regex = new Regex("^[A-Za-z0-9].{4,10}$");
		Match matchPass = regex.Match(password);
		return matchPass.Success;
	}
}
