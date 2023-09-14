using CYRetailIMS.Application.Common.Models.UI;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class ItemBrandController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateBrand(CreateItemBrandViewModel createItemBrandViewModel)
    {
        return Json(new { result = true, msg = "บันทึกข้อมูลสำเร็จ." });
    }
}
