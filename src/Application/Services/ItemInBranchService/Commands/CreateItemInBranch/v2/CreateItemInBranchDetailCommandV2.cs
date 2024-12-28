using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.ItemInBranchService.Commands.CreateItemInBranch.v1;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Commands.CreateItemInBranch.v2;
public record CreateItemInBranchDetailCommandV2 : CreateItemInBranchDetailCommand
{
    public int? subitemtypeid { get; set; }
}
