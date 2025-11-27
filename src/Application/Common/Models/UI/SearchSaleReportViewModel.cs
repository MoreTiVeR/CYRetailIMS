using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class SearchSaleReportViewModel
{
    public int branchid { get; set; }
    public string startdate { get; set; }
	public string enddate { get; set; }
    public int? itembrandid { get; set; }
    public int start { get; set; } // Added for pagination
    public int length { get; set; } // Added for pagination
    public int draw { get; set; } // Added for DataTable draw parameter
    public string searchValue { get; set; }
    public bool isexportalldata { get; set; }
}
