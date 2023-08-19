using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.UnitOfMeasureService.Queries.GetUnitOfMeasureList.v1;

namespace CYRetailIMS.Application.ExternalService.ItemUnitOfMeasureAPI;
public interface IItemUnitOfMeasureAPI
{
    Task<BaseResponse<List<GetUnitOfMeasureListResponseDTO>>> GetUnitOfMeasureListAsync();
    Task<BaseResponse<GetUnitOfMeasureListResponseDTO>> GetUnitOfMeasureByIDAsync(int uomid);
}
