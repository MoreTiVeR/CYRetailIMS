namespace CYRetailIMS.ComponentService.Web.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public string Path { get; set; }
    public string ErrorMsg { get; set; }

    public int StatusCode { get; set; }
    public string StatusDescription { get; set; }
    public bool IsDeveloperMode { get; set; }
    public string RequestController { get; set; }
    public string RequestAction { get; set; }

}
