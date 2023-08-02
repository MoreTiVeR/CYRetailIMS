using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models;
public class ExceptionSettings
{
    public bool IsDeveloperMode { get; set; }
    public string ExceptionController { get; set; }
    public string ExceptionAction { get; set; }
}
