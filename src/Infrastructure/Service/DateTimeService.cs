
using CYRetailIMS.Application.Common.Interfaces;

namespace CYRetailIMS.Infrastructure.Common.Service;
public class DateTimeService : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}
