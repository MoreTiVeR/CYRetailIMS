using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.SupplierTypeService.Queries.GetSupplierTypeList.v1;
public record GetSupplierTypeListQuery : IRequest<BaseResponse<List<GetSupplierTypeResponseDTO>>>
{
}
