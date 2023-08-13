using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CYRetailIMS.Application.Common.Models.UI;

[JsonObject]
[Serializable]
public class JsonViewModel
{
    [JsonPropertyName("result")]
    public bool result { get; set; }

    [JsonPropertyName("message")]
    public string message { get; set; }

    [JsonPropertyName("url")]
    public string url { get; set; }
}
