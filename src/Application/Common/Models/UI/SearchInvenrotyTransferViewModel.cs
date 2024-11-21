using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;

public class SearchInvenrotyTransferViewModel
{
    public int? branchid { get; set; }
    public int? brandid { get; set; }
    public string? startdate { get; set; }
    public string? enddate { get; set; }
}
