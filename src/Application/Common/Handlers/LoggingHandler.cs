using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Handlers;
public class LoggingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //Console.WriteLine("Outgoing Request Headers:");
        foreach (var header in request.Headers)
        {
            var logMsg = $"{header.Key}: {string.Join(", ", header.Value)}";
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
