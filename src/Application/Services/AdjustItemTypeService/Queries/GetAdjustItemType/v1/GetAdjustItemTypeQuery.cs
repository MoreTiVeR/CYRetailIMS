using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.AdjustItemTypeService.Queries.GetAdjustItemType.v1;

[Serializable]
public record GetAdjustItemTypeQuery : IRequest<BaseResponse<List<GetAdjustItemTypeResposeDTO>>>
{
}
