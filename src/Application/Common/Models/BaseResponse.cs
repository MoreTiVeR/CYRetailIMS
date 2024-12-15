using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Extensions;

namespace CYRetailIMS.Application.Common.Models;
public class BaseResponse<T>
{
    public bool result { get; set; }
    public string status { get; set; }

    private string _msg { get; set; }
    public string message
    {
        get
        {
            return _msg;
            //return _msg.ToNonAssci();
        }
        set
        {
            _msg = value;
        }
    }
    public string soruce { get; set; }
    public T data { get; set; }
    public ErrorResponse error { get; set; }
}
