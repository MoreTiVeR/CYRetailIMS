using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/approvestatus")]
[ApiController]
public class AttendanceController : BaseApiController
{
    public AttendanceController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpPost("checkin")]
    public IActionResult CheckIn([FromBody] AttendanceModel attendance)
    {
        if (attendance == null || string.IsNullOrEmpty(attendance.EmployeeId))
        {
            return BadRequest("Invalid attendance data.");
        }

        // Set the check-in time to now
        attendance.CheckInTime = DateTime.UtcNow;
        AttendanceRepository.AddAttendance(attendance);

        return Ok("Checked in successfully.");
    }

    [HttpGet]
    public ActionResult<List<AttendanceModel>> GetAllAttendances()
    {
        return Ok(AttendanceRepository.GetAllAttendances());
    }
}

public static class AttendanceRepository
{
    private static List<AttendanceModel> attendances = new List<AttendanceModel>();

    public static void AddAttendance(AttendanceModel attendance)
    {
        attendances.Add(attendance);
    }

    public static List<AttendanceModel> GetAllAttendances()
    {
        return attendances;
    }
}
