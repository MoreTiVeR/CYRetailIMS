using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.EODSummaryService.Queries.GetEndOfDaySummaryByCriteria.v1;
public class GetEndOfDaySummaryByCriteriaDetail
{
    public int endofdayid { get; set; }
    public DateTime summarydate { get; set; }
    public int branchid { get; set; }
    public string branchname { get; set; }
    public decimal totalcash { get; set; }
    public decimal depositedcash { get; set; }
    public decimal totaltransfer { get; set; }
    public decimal customertransfer { get; set; }
    public decimal grandtotal { get; set; }
    public decimal? substitutewage { get; set; }
    public decimal? fee { get; set; }
    public decimal? otherexpense { get; set; }
    public string? otherexpensenote { get; set; }
    public decimal finaltotal { get; set; }
    public bool isactive { get; set; }
    public string createdby { get; set; }
    public DateTime createddate { get; set; }
    public string? updatedby { get; set; }
    public DateTime? updateddate { get; set; }
}
