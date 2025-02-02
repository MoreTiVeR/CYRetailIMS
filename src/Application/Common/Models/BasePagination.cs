using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models;
public class BasePagination
{
    public int start { get; set; } // Added for pagination
    public int length { get; set; } // Added for pagination
    public int draw { get; set; } // Added for DataTable draw parameter
    public string searchValue { get; set; }
}
