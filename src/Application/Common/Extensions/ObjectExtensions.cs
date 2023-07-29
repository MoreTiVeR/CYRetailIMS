

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
        ErrorResponse? errObj = JsonConvert.DeserializeObject<ErrorResponse>(strContent);
        if (errObj?.Error != null)
        {
            return errObj;
        }
        return new ErrorResponse
        {
            Error = new ErrorData
            {
                Status = statueCode,
                Type = statueCode,
                Message = strContent
            }
        };
    }
}
