using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;

[Serializable]
public record CreateTransactionDetailCommand
{
    [Required(ErrorMessage = "Required field")]
    public int itemid { get; init; }

    [Required(ErrorMessage = "Required field")]
    public decimal price { get; init; }

    [Required(ErrorMessage = "Required field")]
    public int qty { get; init; }

    [Required(ErrorMessage = "Required field")]
    public decimal amount { get; init; }

    [Required(ErrorMessage = "Required field")]
    public bool isactive { get; init; }
}
