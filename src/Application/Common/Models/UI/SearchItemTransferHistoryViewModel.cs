using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class SearchItemTransferHistoryViewModel
{
    public string? transferstrtdate { get; set; }
    public string? transferenddate { get; set; }
    public int? branchid { get; set; }
    public int? transferstatusid { get; set; }

}
