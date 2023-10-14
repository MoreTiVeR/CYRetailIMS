using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ShipmentTypeService.Queries.GetShipmentTypeList.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.ShipmentTypeService.Queries.GetShipmentTypeByID.v1;

[Serializable]
public record GetShipmentTypeByIDCommand : IRequest<BaseResponse<GetShipmentTypeResponseDTO>>
{
	public int shipmenttypeid { get; init; }
}
