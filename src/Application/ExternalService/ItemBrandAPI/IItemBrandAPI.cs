using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandList.v1;

namespace CYRetailIMS.Application.ExternalService.ItemBrandAPI;
public interface IItemBrandAPI
{
    Task<BaseResponse<List<GetItemBrandListResponseDTO>>> GetItemBrandListAsync();
    Task<BaseResponse<GetItemBrandListResponseDTO>> GetItemBrandByIDAsync(int brandid);
}
