using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderList.v1;
public record GetPurchaseOrderListCommand : IRequest<BaseResponse<List<GetPurchaseOrderResposeDTO>>>
{
}
