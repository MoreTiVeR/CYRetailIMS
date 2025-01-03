using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Confiuration;

public interface IAppConfig
{
    string GetConnectionStringDefault();
    string GetUserSecretKey();
    string GetImportItemFilePath();
    int GetSessionTimeoutMinute();
}
