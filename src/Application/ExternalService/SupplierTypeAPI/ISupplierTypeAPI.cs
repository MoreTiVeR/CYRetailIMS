
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.SupplierTypeService.Queries.GetSupplierTypeList.v1;

namespace CYRetailIMS.Application.ExternalService.SupplierTypeAPI;
public interface ISupplierTypeAPI
{
    Task<BaseResponse<List<GetSupplierTypeResponseDTO>>> GetSupplierTypeListAsync();

    Task<BaseResponse<GetSupplierTypeResponseDTO>> GetSupplierTypeByIDAsync(int supplierTypeID);
}
