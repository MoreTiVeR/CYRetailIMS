using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Models;

namespace CYRetailIMS.Infrastructure.Common.Extensions;
public static class HttpResponseExtensions
{
    public static BaseResponse<T> ToResponse<T>(this HttpResponseMessage httpResponse, string strContent)
    {
        httpResponse.Headers.TryGetValues("responsecode", out IEnumerable<string>? outResponseCode);
        httpResponse.Headers.TryGetValues("responsemessage", out IEnumerable<string>? outResponsemessage);
        httpResponse.Headers.TryGetValues("responsedatasource", out IEnumerable<string>? outResponsedatasource);
        return new BaseResponse<T>
        {
            Result = httpResponse.IsSuccessStatusCode,
            Data = httpResponse.IsSuccessStatusCode ? strContent.ToDataObject<T>() : default,
            Error = httpResponse.IsSuccessStatusCode ? default : strContent.ToErrorObject(((int)httpResponse.StatusCode).ToString()),
            Status = outResponseCode != null && outResponseCode.Any() ? outResponseCode.FirstOrDefault() : httpResponse.StatusCode.ToString(),
            Message = outResponsemessage != null && outResponsemessage.Any() ? outResponsemessage.FirstOrDefault() : httpResponse.ReasonPhrase,
            Soruce = outResponsedatasource != null && outResponsedatasource.Any() ? outResponsedatasource.FirstOrDefault() : "N/A"
        };
    }
}
