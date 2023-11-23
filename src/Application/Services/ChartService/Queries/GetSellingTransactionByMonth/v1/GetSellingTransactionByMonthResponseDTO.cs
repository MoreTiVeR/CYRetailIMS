using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ChartService.Queries.GetSellingTransactionByMonth.v1;

[Serializable]
public class GetSellingTransactionByMonthResponseDTO
{
    public string itemname { get; set; }

    /// <summary>
    /// Market Share
    /// </summary>
    public double percent { get; set; }
    public bool isselected { get; set; }
    public bool issliced { get; set; }
}
