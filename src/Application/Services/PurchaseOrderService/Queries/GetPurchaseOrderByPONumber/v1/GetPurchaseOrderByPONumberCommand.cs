using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderList.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderByPONumber.v1;

[Serializable]
public record GetPurchaseOrderByPONumberCommand : IRequest<BaseResponse<GetPurchaseOrderResposeDTO>>
{
	public string purchaseorderno { get; init; }
}
