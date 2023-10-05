using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.AdjustItemTypeService.Queries.GetAdjustItemTypeByID.v1;
public record GetAdjustItemTypeByIDQuery : IRequest<BaseResponse<List<GetAdjustItemTypeByIDResponseDTO>>>
{
    public int adjusttypeid { get; init; }
}
