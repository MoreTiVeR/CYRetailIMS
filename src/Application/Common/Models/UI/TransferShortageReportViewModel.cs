using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class TransferShortageReportViewModel : BasePagination
{
    public int? branchid { get; set; }
    public string? startdate { get; set; }
    public string? enddate { get; set; }
    public int? subitemtypeid { get; set; }
}
