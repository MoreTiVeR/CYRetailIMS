using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class ImportItemViewModel
{
    public int qty { get; set; }
    public string itemtype { get; set; }
    public string itemcode { get; set; }
    public string itembrand { get; set; }
    public string itemname { get; set; }
    public string description { get; set; }
}
