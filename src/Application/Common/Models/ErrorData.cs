using Newtonsoft.Json;

namespace CYRetailIMS.Application.Common.Models;

[Serializable]
public class ErrorData
{
    public string type { get; set; }

    public string status { get; set; }

    public string message { get; set; }

    public string path { get; set; }

    public string stracktrace { get; set; }
}
