using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class CountStockDetail
{
    public string ProductCode { get; set; }
    public int Stock { get; set; }
    public int Count { get; set; }
}