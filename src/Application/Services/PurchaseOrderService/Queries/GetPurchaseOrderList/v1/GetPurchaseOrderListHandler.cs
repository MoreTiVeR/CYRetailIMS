using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderList.v1;
public class GetPurchaseOrderListHandler : BaseService, IRequestHandler<GetPurchaseOrderListCommand, BaseResponse<List<GetPurchaseOrderResposeDTO>>>
{
}
