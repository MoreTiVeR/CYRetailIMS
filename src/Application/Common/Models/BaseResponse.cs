using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models;
public class BaseResponse<T>
{
    public bool Result { get; set; }
    public string Status { get; set; }
    public string Message { get; set; }
    public string Soruce { get; set; }
    public T Data { get; set; }
    public ErrorResponse Error { get; set; }
}
