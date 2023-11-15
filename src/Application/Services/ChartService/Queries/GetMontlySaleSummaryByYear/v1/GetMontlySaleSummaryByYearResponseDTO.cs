using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ChartService.Queries.GetMontlySaleSummaryByYear.v1;
public class GetMontlySaleSummaryByYearResponseDTO
{
    public int month { get; set; }
    public string monthname { get; set; }
    public decimal totalamount { get; set; }
}
