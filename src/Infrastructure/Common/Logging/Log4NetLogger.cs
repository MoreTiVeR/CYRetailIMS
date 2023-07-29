using System.Reflection;
using CYRetailIMS.Application.Common.Interfaces;
using log4net;
using Microsoft.AspNetCore.Http;

namespace INSCore.Template.Infrastructure.Logging;
public class Log4NetLogger : ILog4NetLogger
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDateTimeProvider _dateTimeProvider;
    private ILog _log => LogManager.GetLogger(Assembly.GetEntryAssembly(), typeof(Log4NetLogger));
    private string _sessionID => $"{_httpContextAccessor.HttpContext.Request.Headers["refer"]}{_httpContextAccessor.HttpContext.Request.Headers["branch"]}-{_httpContextAccessor.HttpContext.Request.Headers["sender"]}{DateTime.Now.ToString("yyyyMMdd")}-{_httpContextAccessor.HttpContext.Request.Headers["forward"]}";
    public Log4NetLogger(IHttpContextAccessor httpContextAccessor, IDateTimeProvider dateTimeProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _dateTimeProvider = dateTimeProvider;
    }

    public void Debug(object message)
    {
        throw new NotImplementedException();
    }

    public void Debug(object message, Exception exceptionData)
    {
        throw new NotImplementedException();
    }

    public void Error(object message) => _log.Error($"SessionID[{_sessionID}] Time[{_dateTimeProvider.Now:yyyyMMdd H:mm:ss fff}] [{message}]");
    public void Error(object message, Exception exceptionData) => _log.Error($"SessionID[{_sessionID}] Time[{_dateTimeProvider.Now:yyyyMMdd H:mm:ss fff}] [{message}]", exceptionData);

    public void Fatal(object message)
    {
        throw new NotImplementedException();
    }

    public void Fatal(object message, Exception exceptionData)
    {
        throw new NotImplementedException();
    }

    public void Info(object message) => _log.Info($"SessionID[{_sessionID}] Time[{_dateTimeProvider.Now:yyyyMMdd H:mm:ss fff}] [{message}]");

    public void Info(object message, Exception exceptionData) => _log.Info($"SessionID[{_sessionID}] Time[{_dateTimeProvider.Now:yyyyMMdd H:mm:ss fff}] [{message}]", exceptionData);

    public void Warn(object message)
    {
        throw new NotImplementedException();
    }

    public void Warn(object message, Exception exceptionData)
    {
        throw new NotImplementedException();
    }
}
