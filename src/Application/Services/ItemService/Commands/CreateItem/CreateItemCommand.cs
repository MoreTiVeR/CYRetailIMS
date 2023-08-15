using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;
public record CreateItemCommand : IRequest<BaseResponse<CommandResponse>>
{
    [Required]
    public string itemcode { get; init; }

    [Required]
    public int itemtypeid { get; init; }

    [Required]
    public int unitofmeasureid { get; init; }

    [Required]
    public int brandid { get; init; }

    [Required]
    public string name { get; init; }

    public string shortname { get; init; }

    public string description { get; init; }

    public string barcode { get; init; }

    [Required]
    public decimal price { get; init; }

    /// <summary>
    /// Item image path
    /// </summary>
    public string itemimageurl { get; init; }

    [Required]
    public string createdby { get; init; }

    [Required]
    public bool isactive { get; init; }
}
