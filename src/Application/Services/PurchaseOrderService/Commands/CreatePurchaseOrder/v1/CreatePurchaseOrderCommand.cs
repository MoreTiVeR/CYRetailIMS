using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Commands.CreatePurchaseOrder.v1;

[Serializable]
public record CreatePurchaseOrderCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int purchasetypeid { get; init; }

    public int supplierid { get; init; }

    public int currencyid { get; init; }

    public DateTime orderdate { get; init; }

    public DateTime? receiveddate { get; init; }

    public int paymentypeid { get; init; }

    public string? remarks { get; init; }

    public decimal amount { get; init; }

    public decimal discount { get; init; }

    /// <summary>
    /// (qty*price) - discount
    /// </summary>
    public decimal subtotal { get; init; }

    /// <summary>
    /// vat 7% of subtotal
    /// </summary>
    public decimal tax { get; init; }

    /// <summary>
    /// subtotal - tax
    /// </summary>
    public decimal total { get; init; }

    public string createdby { get; init; }

    public DateTime createddate { get; init; }

    public bool isactive { get; init; }

    public int approvestatus { get; init; }

    public CreateShipmentCommand shipment { get; init; }

    public List<CreatePurchaseOrderDetailCommand> detail { get; init; }
}
