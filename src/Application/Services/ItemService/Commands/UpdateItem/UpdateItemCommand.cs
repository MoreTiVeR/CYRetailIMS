using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemService.Commands.UpdateItem;

[Serializable]
public class UpdateItemCommand : IRequest<BaseResponse<CommandResponse>>
{
    [Required]
    public int itemid { get; init; }

    [Required]
    public string name { get; init; }

    public string shortname { get; init; }

    public string description { get; init; }

    public string barcode { get; init; }

    [Required]
    public decimal price { get; init; }

    [Required]
    public double discountpercent { get; init; }

    [Required]
    public int qty { get; init; }

    [Required]
    public int notifyqty { get; init; }

    /// <summary>
    /// Item image path
    /// </summary>
    public string itemimageurl { get; init; }

    [Required]
    public string updatedby { get; init; }

    [Required]
    public bool isactive { get; init; }
}
