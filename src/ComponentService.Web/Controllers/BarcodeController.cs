using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class BarcodeController : Controller
{
    // GET: Barcode
    public IActionResult Zxing()
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
}
