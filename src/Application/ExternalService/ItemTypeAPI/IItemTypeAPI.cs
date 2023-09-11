using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeList.v1;

namespace CYRetailIMS.Application.ExternalService.ItemTypeAPI;
public interface IItemTypeAPI
{
    Task<BaseResponse<List<GetItemTypeListResponseDTO>>> GetItemTypeListAsync();
    Task<BaseResponse<GetItemTypeListResponseDTO>> GetItemTypeByIDAsync(int itemtypeid);
}
