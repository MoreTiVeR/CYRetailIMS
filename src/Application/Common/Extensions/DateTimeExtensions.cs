using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    public static DateTime ToDateTime(this string value, string sTime)
    {
        try
        {
            string time = !string.IsNullOrEmpty(sTime) ? sTime : DateTime.Now.ToString("HH:mm:ss");
            var sDateTime = $"{value} {time}:00";
            DateTime fullDateTime = DateTime.ParseExact(sDateTime, "dd/MM/yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-US"));
            return fullDateTime;
        }
        catch
        {
            return DateTime.Now;
        }
    }

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

    public static DateTime DatetimePickerToDate(this string datetimepickerValue)
    {
		string[] startDate = datetimepickerValue.Split("-");
		if (startDate.Count() != 3)
		{
			throw new Exception("รูปแบบวันที่ไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
		}
		return new DateTime(startDate[2].ToInt32(), startDate[1].ToInt32(), startDate[0].ToInt32());
	}

    public static DateTime DatetimePickerToMonthYear(this string datetimepickerValue)
    {
        string[] startDate = datetimepickerValue.Split("-");
        if (startDate.Count() != 2)
        {
            throw new Exception("รูปแบบวันที่ไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        }
        return new DateTime(startDate[1].ToInt32(), startDate[0].ToInt32(), 1);
    }

    public static string ToDateString(this DateTime dateTime)
    {
        try
        {
            return $"{dateTime:dd/MM/yyyy}";
        }
        catch
        {
            return default;
        }
    }
}
