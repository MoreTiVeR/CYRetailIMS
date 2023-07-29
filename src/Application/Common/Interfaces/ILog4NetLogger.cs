using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Interfaces;
public interface ILog4NetLogger
{
    #region Debug
    void Debug(object message);
    void Debug(object message, Exception exceptionData);
    #endregion
    #region Info
    void Info(object message);
    void Info(object message, Exception exceptionData);
    #endregion
    #region Warn
    void Warn(object message);
    void Warn(object message, Exception exceptionData);
    #endregion
    #region Error
    void Error(object message);
    void Error(object message, Exception exceptionData);
    #endregion
    #region Fatal
    void Fatal(object message);
    void Fatal(object message, Exception exceptionData);
    #endregion
}
