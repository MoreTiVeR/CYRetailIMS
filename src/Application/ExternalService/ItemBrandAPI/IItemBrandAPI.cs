using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemBrandService.Commands.CreateBrand.v1;
using CYRetailIMS.Application.Services.ItemBrandService.Commands.DeleteBrand.v1;
using CYRetailIMS.Application.Services.ItemBrandService.Commands.UpdateBrand.v1;
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandList.v1;

namespace CYRetailIMS.Application.ExternalService.ItemBrandAPI;
public interface IItemBrandAPI
{
    Task<BaseResponse<CommandResponse>> CreateItemBrandAsync(CreateBrandCommand createBrandCommand);
    Task<BaseResponse<CommandResponse>> UpdateItemBrandAsync(UpdateBrandCommand updateBrandCommand);
    Task<BaseResponse<CommandResponse>> DeleteItemBrandAsync(DeleteBrandCommand deleteBrandCommand);
    Task<BaseResponse<List<GetItemBrandListResponseDTO>>> GetItemBrandListAsync();
    Task<BaseResponse<GetItemBrandListResponseDTO>> GetItemBrandByIDAsync(int brandid);
}
