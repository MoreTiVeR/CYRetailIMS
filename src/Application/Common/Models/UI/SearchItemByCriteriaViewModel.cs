using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;

[Serializable]
public class SearchItemByCriteriaViewModel
{
    public int itemid { get; set; }
    public int branchid { get; set; }
}
