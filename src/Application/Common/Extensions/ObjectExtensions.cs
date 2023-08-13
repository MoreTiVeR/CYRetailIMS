

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
}
