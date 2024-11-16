using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Infrastructure.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CYRetailIMS.Infrastructure.Common.HttpClientRequest;
public class HttpClientRequest : IHttpClientRequest
{
    private readonly string _cyApiUrl = string.Empty;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    public string CYApiUrl { get => _cyApiUrl; }

    public HttpClientRequest(IHttpContextAccessor httpContextAccessor,
        IHttpClientFactory httpClientFactory,
        HttpClient httpClient, 
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
        _httpClient = _httpClientFactory.CreateClient();
        _cyApiUrl = configuration.GetSection("CyApiUrl").Get<string>();
    }

    private async Task<HttpResponseMessage> Invoke<TReq>(HttpMethod method, Uri Endpoint, TReq? Req)
    {
        HttpResponseMessage Response;
        HttpRequestMessage ReqMsg = new HttpRequestMessage(method, Endpoint);
        if (Req != null)
        {
            string StringBodyRequest = JsonConvert.SerializeObject(Req);
            StringContent ReqConten = new StringContent(StringBodyRequest, Encoding.UTF8, "application/json");
            //ReqConten.Headers.ContentType = new MediaTypeHeaderValue("application/json; charset=utf-8");
            //ReqConten.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("charset", "utf-8"));
            //ReqConten.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("IEEE754Compatible", "true"));
            ReqMsg.Content = ReqConten;
        }
        Response = await _httpClient.SendAsync(ReqMsg);
        return Response;
    }

    public async Task<BaseResponse<TRes>> HttpRequestToObject<TRes, TReq>(HttpMethod method, Uri Endpoint, TReq? Req)
    {
        HttpResponseMessage response = await Invoke(method, Endpoint, Req);
        string SearchResultString = await response.Content.ReadAsStringAsync();
        var res = response.ToResponse<TRes>(SearchResultString);
        return res;
    }
}
