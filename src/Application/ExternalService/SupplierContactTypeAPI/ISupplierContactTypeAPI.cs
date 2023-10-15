using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.SupplierContactTypeService.Queries.GetSupplierContactTypeList.v1;

namespace CYRetailIMS.Application.ExternalService.SupplierContactTypeAPI;
public interface ISupplierContactTypeAPI
{
	Task<BaseResponse<List<GetSupplierContactTypeResposeDTO>>> GetSupplierContactTypeListAsync();
	Task<BaseResponse<GetSupplierContactTypeResposeDTO>> GetSupplierContactTypeByIDAsync(int supplierID);
}
