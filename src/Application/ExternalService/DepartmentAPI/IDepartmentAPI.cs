using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartmentByID.v1;
using CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartments.v1;

namespace CYRetailIMS.Application.ExternalService.DepartmentAPI;

public interface IDepartmentAPI
{
    Task<BaseResponse<List<GetDepartmentsResponseDTO>>> GetDepartmentsAsync();
    Task<BaseResponse<GetDepartmentByIDResponseDTO>> GetDepartmentByIDAsync(int departmentid);
}
