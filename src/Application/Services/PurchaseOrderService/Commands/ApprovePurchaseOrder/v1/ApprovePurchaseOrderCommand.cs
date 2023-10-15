using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Commands.ApprovePurchaseOrder.v1;

[Serializable]
public record ApprovePurchaseOrderCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int purchaseorderid { get; init; }
    public int approvestatus { get; init; }
    public string approvedby { get; init; }
    public DateTime approveddate { get; init; }
}
