using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class ViewSummaryItemTransferDetailModels
{
    public int subitemtypeid { get; set; }
    public string subitemtypecode { get; set; }
    public int totalrefillqty { get; set; }
    public int totalcheckedqty { get; set; }
}
