using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Commands.UpdatePurchaseOrder.v1;

[Serializable]
public record UpdatePurchaseOrderCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int purchaseorderid { get; init; }
    public bool isactive { get; set; }
    public string updatedby { get; init; }
    public DateTime updateddate { get; init; }
}
