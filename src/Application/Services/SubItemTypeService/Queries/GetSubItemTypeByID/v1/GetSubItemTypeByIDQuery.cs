using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeList.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeByID.v1;
public record GetSubItemTypeByIDQuery : IRequest<BaseResponse<GetSubItemTypeResponseDTO>>
{
    public int subitemtypid { get; init; }
}
