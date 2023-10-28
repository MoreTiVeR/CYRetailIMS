using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Extensions;
public static class DateTimeExtensions
{
    public static DateTime ToDate(this string value) => DateTime.ParseExact(value, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));

    public static DateTime ToDateTime(this string value) => DateTime.ParseExact(value, "dd/MM/yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-US"));
}
