
namespace CYRetailIMS.Domain.Infrastructure.HealthCheck;
public class DependencyHealthCheckResponse
{
    public string Status { get; set; }
    public string Component { get; set; }
    public string Description { get; set; }
}
