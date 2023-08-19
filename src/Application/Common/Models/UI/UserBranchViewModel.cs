using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;

[Serializable]
public class UserBranchViewModel
{
    public int branchid { get; set; }
    public string branchname { get; set; }
    public string branchcode { get; set; }
}
