using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class SearchCountStockViewModel : BasePagination
{
    public string startdate { get; set; }
    public string enddate { get; set; }
    public int branchid { get; set; }
}
