using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models;
public class BaseResponse<T>
{
    public bool result { get; set; }
    public string status { get; set; }
    public string message { get; set; }
    public string soruce { get; set; }
    public T data { get; set; }
    public ErrorResponse error { get; set; }
}
