using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleReportGroupByBranch.v1;
public class SaleReportGroupByBranchDetailDTO
{
    public DateTime transactiondate { get; set; }
    public int branchid { get; set; }
    public string branchname { get; set; }

    public int itemid { get; set; }
    public string? itemcode { get; set; }
    public string? itemname { get; set; }
    public int? brandid { get; set; }
    public string? brandname { get; set; }
    public int totalsaleqty { get; set; }
    public decimal itempriceinbranch { get; set; }
   
    private decimal _totalamount { get; set; }

    public decimal totalamount
    {
        get
        {
            if (_totalamount == 0)
            {
                return totalsaleqty * itempriceinbranch;
            }
            return _totalamount;
        }
        set => _totalamount = value;
    }
}
