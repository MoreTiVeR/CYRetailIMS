using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class AttendanceModel
{
    public int Id { get; set; }
    public string EmployeeId { get; set; }
    public DateTime CheckInTime { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
