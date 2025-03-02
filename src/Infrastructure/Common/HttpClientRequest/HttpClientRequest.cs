using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
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
        _httpClient = _httpClientFactory.CreateClient("ApiClient");
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
            ReqMsg.Content = ReqConten;
        }
        //ReqMsg.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        //_httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        //_httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
        Response = await _httpClient.SendAsync(ReqMsg);
        return Response;
    }

    public async Task<BaseResponse<TRes>> HttpRequestToObject<TRes, TReq>(HttpMethod method, Uri Endpoint, TReq? Req)
    {
        string SearchResultString = string.Empty;
        HttpResponseMessage response = await Invoke(method, Endpoint, Req);

        //#region Sample Get size
        //byte[] contentBytes = await response.Content.ReadAsByteArrayAsync();
        //long contentSize = contentBytes.Length;
        //#endregion

        //#region Method_1: Manual Response DeCompression
        //// Read the content as a byte array
        //byte[] compressedData = await response.Content.ReadAsByteArrayAsync();

        //// Manually decompress the data
        //using var compressedStream = new MemoryStream(compressedData);
        //using var decompressionStream = new GZipStream(compressedStream, CompressionMode.Decompress);
        //using var resultStream = new MemoryStream();
        //await decompressionStream.CopyToAsync(resultStream);
        //SearchResultString = Encoding.UTF8.GetString(resultStream.ToArray());
        //#endregion

        SearchResultString = await response.Content.ReadAsStringAsync();
        var res = response.ToResponse<TRes>(SearchResultString);
        return res;
    }
}
