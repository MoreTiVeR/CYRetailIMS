using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;

[Serializable]
public class ItemInBranchViewModel
{
    public int itemid { get; set; }
    public string itemname { get; set; }
    public string shortname { get; set; }
}
