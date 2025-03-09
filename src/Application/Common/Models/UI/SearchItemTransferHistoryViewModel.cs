using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class SearchItemTransferHistoryViewModel
{
    public string? transferstartdate { get; set; }
    public string? transferenddate { get; set; }
    public int? branchid { get; set; }
    public int? transferstatusid { get; set; }

}

public class SearchItemTransferHistoryViewModelV2
{
    public string transferstartdate { get; set; }
    public string transferenddate { get; set; }
    public int branchid { get; set; }
    public int transferstatusid { get; set; }
    public int start { get; set; } // Added for pagination
    public int length { get; set; } // Added for pagination
    public int draw { get; set; } // Added for DataTable draw parameter
    public string searchValue { get; set; }
    public bool isexportalldata { get; set; }
}