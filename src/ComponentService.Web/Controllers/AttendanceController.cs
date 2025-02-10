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


    [HttpPost]
    public async Task<IActionResult> SubmitLocation([FromBody] LocationModel model)
    {
        if (model == null || model.Latitude == 0 || model.Longitude == 0)
        {
            return BadRequest("Invalid location data.");
        }

        // Save the latitude and longitude to the database or process it as needed
        // Example: SaveToDatabase(model.Latitude, model.Longitude);

        return Ok("Check-in successful!");
    }
}

public class LocationModel
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

