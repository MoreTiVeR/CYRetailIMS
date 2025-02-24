using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class CountStockUpdateModel : CountStockCreateModel
{
    public int CountStockID { get; set; }
    public int CountStockDetailID { get; set; }
    
}