using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.CreateAdjustItem.v1;

[Serializable]
public record CreateAdjustItemDetailCommand
{
    public int adjusttypeid { get; init; }
    public int itemid { get; init; }
    public int branchid { get; set; }
    public int qty { get; init; }
}
