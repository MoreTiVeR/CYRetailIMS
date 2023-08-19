using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;

namespace CYRetailIMS.Infrastructure.ExternalService;
public class HttpClientService
{
    protected readonly ILog4NetLogger _log;
    protected readonly IHttpClientRequest _httpClientRequest;
    public HttpClientService(ILog4NetLogger log, IHttpClientRequest httpClientRequest)
    {
        _log = log;
        _httpClientRequest = httpClientRequest;
    }
}
