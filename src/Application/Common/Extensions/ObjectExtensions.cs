

using System.Text.RegularExpressions;
using CYRetailIMS.Application.Common.Models;
using Newtonsoft.Json;

namespace CYRetailIMS.Application.Common.Extensions;
public static class ObjectExtensions
{
    public static bool ToBool(this string obj)
    {
        bool res;
        try
        {
            bool.TryParse(obj, out res);
            return res;
        }
        catch { return false; }
    }

    public static bool ToBoolFromIntString(this string obj)
    {
        try
        {
            return !string.IsNullOrEmpty(obj) && obj == "1" ? true : false;
        }
        catch { return false; }
    }

    public static bool ToBool(this int value)
    {
        try
        {
            bool.TryParse(value.ToString(), out var res);
            return res;
        }
        catch { return false; }
    }

    public static int ToInt32(this bool value)
    {
        try
        {
            return value ? 1 : 0;
        }
        catch { return 0; }
    }

    public static int ToInt32(this string obj)
    {
        int.TryParse(obj, out int result);
        return result;
    }

    public static decimal ToDecimal(this string obj)
    {
        decimal.TryParse(obj, out decimal result);
        return result;
    }

    public static double ToFloat(this string obj)
    {
        double.TryParse(obj, out double result);
        return result;
    }

    public static T ToDataObject<T>(this string strContent)
    {
        return JsonConvert.DeserializeObject<T>(strContent);
    }

    public static ErrorResponse ToErrorObject(this string strContent, string statueCode = "500")
    {
        if (string.IsNullOrEmpty(strContent))
        {
            throw new Exception();
        }
        ErrorData errObj = JsonConvert.DeserializeObject<ErrorData>(strContent);
        if (errObj != null)
        {
            return new ErrorResponse
            {
                error = errObj
            };
        }
        return new ErrorResponse
        {
            error = new ErrorData
            {
                status = statueCode,
                type = statueCode,
                message = "N/A"
            }
        };
    }

    public static string ToJson(this object obj)
    {
        try
        {
            return JsonConvert.SerializeObject(obj);
        }
        catch
        {
            return default;
        }
    }

    public static string ToNonAssci(this string msg)
    {
        try
        {
            return Regex.Replace(msg, @"[^\u0000-\u007F]+", string.Empty);
        }
        catch
        {
            return "N/A";
        }
    }
}
