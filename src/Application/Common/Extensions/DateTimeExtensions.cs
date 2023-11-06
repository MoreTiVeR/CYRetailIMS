using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Extensions;
public static class DateTimeExtensions
{
    //public static DateTime ToDate(this string value) => DateTime.ParseExact(value, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));

    public static DateTime ToDate(this string value)
    {
        try
        {
            var sDateTime = $"{value} {DateTime.Now.ToString("HH:mm:ss")}";
            DateTime fullDateTime = DateTime.ParseExact(sDateTime, "dd/MM/yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-US"));
            return fullDateTime;
        }
        catch
        {
            return DateTime.Now;
        }
    }

    //public static DateTime ToDateTime(this string value) => DateTime.ParseExact(value, "dd/MM/yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-US"));
    public static DateTime ToDateTime(this string value)
    {
        try
        {
            var dTime = DateTime.ParseExact(value, "dd/MM/yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-US"));
            return dTime;

            //DateTime.TryParse(value, new System.Globalization.CultureInfo("en-US"), out DateTime dTime);
            //return dTime;
        }
        catch
        {
            DateTime.TryParse(value, out DateTime dTime);
            return dTime;
        }
    }
}
