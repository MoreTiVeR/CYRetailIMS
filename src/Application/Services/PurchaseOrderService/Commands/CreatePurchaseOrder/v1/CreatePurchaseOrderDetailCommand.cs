using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Commands.CreatePurchaseOrder.v1;

[Serializable]
public record CreatePurchaseOrderDetailCommand
{
    [Required]
    public int itemid { get; init; }

    public string? description { get; init; }

    [Required]
    public int qty { get; init; }

    [Required]
    public decimal price { get; init; }

    private decimal _amount { get; set; }

    [Required]
    public decimal amount
    {
        get => _amount;
        set => _amount = price * qty;
    }

    public decimal discountpercentage { get; init; }
    public decimal discountamount { get; init; }

    private decimal _subtotal { get; set; }

    /// <summary>
    /// เงินวมก่อนภาษี
    /// </summary>
    [Required]
    public decimal subtotal
    {
        get => _subtotal;
        set => _subtotal = _amount;
    }

    /// <summary>
    /// คิดภาษี แบบ %
    /// </summary>
    public decimal taxpercentage { get; set; }

    /// <summary>
    /// รวมภาษี จำนวนเต็ม
    /// </summary>
    [Required]
    public decimal taxamount { get; init; }


    private decimal _total { get; set; }
    /// <summary>
    /// เงินรวมทั้งหมด
    /// </summary>
    [Required]
    public decimal total
    {
        get => _total;
        set => _total = _subtotal - taxamount;
    }
}
