using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ShipmentTypeService.Queries.GetShipmentTypeList.v1;

namespace CYRetailIMS.Application.ExternalService.ShipmentTypeAPI;
public interface IShipmentTypeAPI
{
	Task<BaseResponse<List<GetShipmentTypeResponseDTO>>> GetShipmentTypeListAsync();
	Task<BaseResponse<GetShipmentTypeResponseDTO>> GetShipmentTypeByIDAsync(int shipmentTypeID);
}
