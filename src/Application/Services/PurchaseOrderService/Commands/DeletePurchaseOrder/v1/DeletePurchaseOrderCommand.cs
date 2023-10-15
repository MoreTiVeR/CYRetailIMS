using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Commands.DeletePurchaseOrder.v1;

[Serializable]
public record DeletePurchaseOrderCommand : IRequest<BaseResponse<CommandResponse>>
{
	public int purchaseorderid { get; init; }
    public string deletedby { get; init; }
    public DateTime deleteddate { get; init; }
}
