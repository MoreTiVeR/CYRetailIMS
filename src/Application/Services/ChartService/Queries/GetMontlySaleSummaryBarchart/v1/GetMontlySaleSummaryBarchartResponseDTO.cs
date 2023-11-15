using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ChartService.Queries.GetMontlySaleSummaryBarchart.v1;

[Serializable]
public class GetMontlySaleSummaryBarchartResponseDTO
{
    public int branchid { get; set; }
    public string branchname { get; set; }
    public decimal totalamount { get; set; }
}
