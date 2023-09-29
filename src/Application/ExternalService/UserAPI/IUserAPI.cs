using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee.v1;
using CYRetailIMS.Application.Services.UserService.Commands.CreateUser.v1;
using CYRetailIMS.Application.Services.UserService.Commands.DeleteUser.v1;
using CYRetailIMS.Application.Services.UserService.Commands.UpdateUser.v1;
using CYRetailIMS.Application.Services.UserService.Queries.GetUser.v1;

namespace CYRetailIMS.Application.ExternalService.UserAPI;
public interface IUserAPI
{
    Task<BaseResponse<CommandResponse>> CreateUser(CreateUserCommand createUserCommand);

    Task<BaseResponse<List<GetUserResponseDTO>>> GetUsersAsync();

    Task<BaseResponse<GetUserResponseDTO>> GetUserByIDAsync(int userid);

    Task<BaseResponse<CommandResponse>> UpdateUser(UpdateUserCommand updateUserCommand);

    Task<BaseResponse<CommandResponse>> DeleteUser(DeleteUserCommand deleteUserCommand);
}
