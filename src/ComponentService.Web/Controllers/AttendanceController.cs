using CYRetailIMS.Application.Common.Models.UI;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.Web.Controllers;

/// <summary>
/// Google cloud https://console.cloud.google.com/
/// </summary>
public class AttendanceController : Controller
{
    private string googleApiNormalKey => "AIzaSyD4aX-6dnU6tfyhBuGabob1sP6fPMD8LV4";
    private string googleApiRestrictionKey => "AIzaSyDHF4asSLScjVzl7XAwF4EsL5yAYzfSm0g";
    private readonly HttpClient _httpClient;
    public AttendanceController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public IActionResult Index()
    {
        ViewBag.GoogleMapsApiKey = googleApiRestrictionKey;
        return View();
    }

    [HttpGet("getMapData")]
    public async Task<IActionResult> GetMapData(string location = "Bangkok")
    {
        // Construct the URL for the Google Maps API request
        string requestUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={location}&key={googleApiNormalKey}";

        try
        {
            // Make the request to the Google Maps API
            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            // Read the response content
            var content = await response.Content.ReadAsStringAsync();

            // Return the content as JSON
            return Ok(content);
        }
        catch (HttpRequestException e)
        {
            // Handle error (e.g., log it, return a specific error message)
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
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

