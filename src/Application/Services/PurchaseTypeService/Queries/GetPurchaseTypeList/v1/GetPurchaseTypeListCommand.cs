using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.PurchaseTypeService.Queries.GetPurchaseTypeList.v1;
public record GetPurchaseTypeListCommand : IRequest<BaseResponse<List<GetPurchaseTypeResponseDTO>>>
{
}
