using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.PurchaseTypeService.Queries.GetPurchaseTypeList.v1;

namespace CYRetailIMS.Application.ExternalService.PurchaseTypeAPI;
public interface IPurchaseTypeAPI
{
	Task<BaseResponse<List<GetPurchaseTypeResponseDTO>>> GetPurchaseTypeListAsync();
	Task<BaseResponse<GetPurchaseTypeResponseDTO>> PurchaseTypeByIDAsync(int purchaseTypeID);
}
