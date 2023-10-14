using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.WarehouseService.Queries.GetWarehouseList.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.WarehouseService.Queries.GetWarehouseByID.v1;

[Serializable]
public record GetWarehouseByIDCommand : IRequest<BaseResponse<GetWarehouseResponseDTO>>
{
    public int warehouseid { get; init; }
}

