using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.AdjustItemTypeService.Queries.GetAdjustItemType.v1;
using CYRetailIMS.Application.Services.AdjustItemTypeService.Queries.GetAdjustItemTypeByID.v1;

namespace CYRetailIMS.Application.ExternalService.AdjustItemTypeAPI;
public interface IAdjustItemTypeAPI
{
    Task<BaseResponse<List<GetAdjustItemTypeResposeDTO>>> GetAdjustTypesAsync();

    Task<BaseResponse<List<GetAdjustItemTypeByIDResponseDTO>>> GetAdjustTypesAsync(int adjusttypeid);
}
