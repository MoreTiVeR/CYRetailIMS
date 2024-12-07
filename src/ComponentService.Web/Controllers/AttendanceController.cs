using CYRetailIMS.Application.Common.Models.UI;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class AttendanceController : Controller
{

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CheckIn(string EmployeeId, double Latitude, double Longitude)
    {
        if (string.IsNullOrEmpty(EmployeeId))
        {
            return BadRequest("Invalid Employee ID.");
        }

        // Assuming you have some attendance logic to handle here
        AttendanceModel attendance = new AttendanceModel
        {
            EmployeeId = EmployeeId,
            Latitude = Latitude,
            Longitude = Longitude,
            CheckInTime = DateTime.UtcNow // Assuming you are still setting check-in time
        };

        //AttendanceRepository.AddAttendance(attendance);

        return RedirectToAction("Index");
    }
}


