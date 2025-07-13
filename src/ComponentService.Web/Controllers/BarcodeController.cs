using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class BarcodeController : Controller
{

    public IActionResult Index()
    {
        return View();
    }

    //GET: Barcode
    public IActionResult Zxing()
    {
        return View();
    }

    public IActionResult QRCode()
    {
        return View();
    }

    // POST: Process the scanned barcode
    [HttpPost]
    public JsonResult ProcessBarcode(string barcode)
    {
        // Process the barcode (e.g., save to database, validate, etc.)
        return Json(new { success = true, scannedBarcode = barcode });
    }

    [HttpPost]
    public IActionResult ScanBarcode([FromBody] BarcodeModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.Barcode))
        {
            return BadRequest("Invalid barcode data.");
        }

        // Process the barcode (e.g., update inventory, etc.)
        // Example: var item = inventoryService.GetItemByBarcode(model.Barcode);
        // Handle item logic here

        return Ok(new { message = "Barcode processed successfully." });
    }

    public class BarcodeModel
    {
        public string Barcode { get; set; }
    }
}
