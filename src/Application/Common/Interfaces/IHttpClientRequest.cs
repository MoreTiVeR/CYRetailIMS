using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;

namespace CYRetailIMS.Application.Common.Interfaces;
public interface IHttpClientRequest
{
    string CYApiUrl { get; }
    Task<BaseResponse<TRes>> HttpRequestToObject<TRes, TReq>(HttpMethod method, Uri Endpoint, TReq Req);
}
