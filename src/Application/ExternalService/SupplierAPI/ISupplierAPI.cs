using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.SupplierService.Commands.CreateSupplier.v1;
using CYRetailIMS.Application.Services.SupplierService.Commands.DeleteSupplier.v1;
using CYRetailIMS.Application.Services.SupplierService.Commands.UpdateSupplier.v1;
using CYRetailIMS.Application.Services.SupplierService.Queries.GetSupplierList.v1;

namespace CYRetailIMS.Application.ExternalService.SupplierAPI;
public interface ISupplierAPI
{
	Task<BaseResponse<CommandResponse>> CreateSupplierAsync(CreateSupplierCommand createSupplierCommand);

    Task<BaseResponse<CommandResponse>> UpdateSupplierAsync(UpdateSupplierCommand updateSupplierCommand);

    Task<BaseResponse<CommandResponse>> DeleteSupplierAsync(DeleteSupplierCommand deleteSupplierCommand);

    Task<BaseResponse<List<GetSupplierResponseDTO>>> GetSupplierListAsync();

	Task<BaseResponse<GetSupplierResponseDTO>> GetSupplierByIDAsync(int supplierID);
}
