using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer.v1;

[Serializable]
public record CreateItemTransferDetailCommand
{
    [Required(ErrorMessage = "Required field")]
    public int itemid { get; init; }

    [Required(ErrorMessage = "Required field")]
    public int qty { get; init; }

}
