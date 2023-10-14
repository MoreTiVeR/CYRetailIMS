using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ShipmentTypeService.Queries.GetShipmentTypeList.v1;
public record GetShipmentTypeListCommand : IRequest<BaseResponse<List<GetShipmentTypeResponseDTO>>>
{
}
