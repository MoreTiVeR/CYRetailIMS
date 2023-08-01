using Newtonsoft.Json;

namespace CYRetailIMS.Application.Common.Models;

[Serializable]
public class ErrorData
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("path")]
    public string Path { get; set; }

    [JsonProperty("stracktrace")]
    public string StrackTrace { get; set; }
}
