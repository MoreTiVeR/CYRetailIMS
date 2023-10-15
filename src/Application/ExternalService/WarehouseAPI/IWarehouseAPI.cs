using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.WarehouseService.Queries.GetWarehouseList.v1;

namespace CYRetailIMS.Application.ExternalService.WarehouseAPI;
public interface IWarehouseAPI
{
	Task<BaseResponse<List<GetWarehouseResponseDTO>>> GetWarehouseListAsync();
	Task<BaseResponse<GetWarehouseResponseDTO>> GetWarehouseByIDAsync(int warehouseID);
}
