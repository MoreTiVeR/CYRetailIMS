using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeList.v1;
public class GetSubItemTypeListQuery : IRequest<BaseResponse<List<GetSubItemTypeResponseDTO>>>
{
}
