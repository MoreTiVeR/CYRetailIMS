using System.Text;
using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.ItemAPI;
using CYRetailIMS.Application.ExternalService.ItemInBranchAPI;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Staff)]
public class SaleController : BaseController
{
    private readonly IItemInBranchAPI _itemInBranchAPI;
    private readonly IItemAPI _itemAPI;
    public SaleController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IItemInBranchAPI itemInBranchAPI,
        IItemAPI itemAPI) : base(httpClientRequest, mapper, log)
    {
        _itemInBranchAPI = itemInBranchAPI;
        _itemAPI = itemAPI;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Create()
    {
        BaseResponse<GetItemInBranchByBranchIDResponseDTO> resItemInBranch = await _itemInBranchAPI.GetItemInBranchByBranchIDAsync(UserProfile.access_branch.FirstOrDefault().branchid);
        ViewBag.ItemInBranch = resItemInBranch.data.itemlist;
        return View();
    }

    public IActionResult Items()
    {
        return View();
    }

    /// <summary>
    /// Only for Validation
    /// </summary>
    /// <param name="currencyDto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> ItemDataValidation(SellingItemViewModel sellingItemObj)
    {
        try
        {
            //if (!base.UserProfile.BranchList.Any(w => w.BranchID == BuyingBranchID))
            //{
            //    return Json(new { result = false, msg = $"{GlobalMessageModel.ErrorInvalidBranch}" });
            //}
            #region Get form value
            List<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>> form = Request.Form.ToList();
            #endregion

            #region Prepare new from with not empty value
            form = form.Where(w => w.Key.Contains("data[outer-item-group]")).Where(w => !string.IsNullOrEmpty(w.Value[0])).ToList();
            if(form.Count == 0)
            {
                return Json(new { result = false, msg = $"ขออภัย ข้อมูลขายสินค้าไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!." });
            }
            #endregion

            #region Validate Selling Item
            bool isValidData = form.Where(w => w.Key.Contains("data[outer-item-group]")).Any(w => !string.IsNullOrEmpty(w.Value[0]));
            if (!isValidData)
            {
                return Json(new { result = false, msg = $"ขออภัย ข้อมูลขายสินค้าไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!." });
            }
            #endregion

            return Json(new { result = true, msg = "ตรวจสอบข้อมูลถูกต้อง." });

            #region Create Transaction detail
            //decimal totalAmt = 0;
            //decimal totalProfitAmt = 0;
            //int idx = form.Count() / 4;
            //for (int i = 0; i < idx; i++)
            //{
            //    var code = form.Where(w => w.Key == $"data[outer-item-group][{i}][ddlSearchItem]").FirstOrDefault().Value[0];
            //    var rate = form.Where(w => w.Key == $"data[outer-item-group][{i}][txtItemPrice]").FirstOrDefault().Value[0];
            //    var qty = form.Where(w => w.Key == $"data[outer-item-group][{i}][txtItemQty]").FirstOrDefault().Value[0];
            //    var amt = form.Where(w => w.Key == $"data[outer-item-group][{i}][txtAmount]").FirstOrDefault().Value[0];

            //    if (!string.IsNullOrEmpty(code) &&
            //        !string.IsNullOrEmpty(rate) &&
            //        !string.IsNullOrEmpty(qty) &&
            //        !string.IsNullOrEmpty(amt))
            //    {
            //        totalAmt += decimal.Parse(amt);
            //    }
            //}
            //return Json(new { result = true, msg = "ตรวจสอบข้อมูลถูกต้อง." });
            #endregion
        }
        catch (Exception ex)
        {
            return Json(new { result = true, msg = $"ขออภัย รูปแบบข้อมูลไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!. {ex.Message}" });
        }
    }

    public async Task<IActionResult> SaveSellingItem(SellingItemViewModel sellingItemObj)
    {
        try
        {
            #region Get form value
            List<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>> form = Request.Form.ToList();
            #endregion

            #region Prepare new from with not empty value
            form = form.Where(w => w.Key.Contains("outer-item-group")).Where(w => !string.IsNullOrEmpty(w.Value[0])).ToList();
            if (form.Count == 0)
            {
                return Json(new { result = false, msg = $"ขออภัย ข้อมูลขายสินค้าไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!." });
            }
            #endregion

            decimal totalAmt = 0;
            decimal totalProfitAmt = 0;
            int idx = form.Count / 4;
            for (int i = 0; i < idx; i++)
            {
                var code = form.Where(w => w.Key == $"outer-item-group[{i}][ddlSearchItem]").FirstOrDefault().Value[0];
                var rate = form.Where(w => w.Key == $"outer-item-group[{i}][txtItemPrice]").FirstOrDefault().Value[0];
                var qty = form.Where(w => w.Key == $"outer-item-group[{i}][txtItemQty]").FirstOrDefault().Value[0];
                var amt = form.Where(w => w.Key == $"outer-item-group[{i}][txtAmount]").FirstOrDefault().Value[0];

                if (!string.IsNullOrEmpty(code) &&
                    !string.IsNullOrEmpty(rate) &&
                    !string.IsNullOrEmpty(qty) &&
                    !string.IsNullOrEmpty(amt))
                {
                    //Code
                }
            }

            #region Create Object

            #endregion
            return Json(new { result = true, msg = "บันทึกข้อมูลสำเร็จ." });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, msg = $"ขออภัย รูปแบบข้อมูลไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!. {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> GetItemPriceByID(string itemId)
    {
        try
        {
            var res = await _itemAPI.GetItemByIdAsync(Convert.ToInt32(itemId));
            if (res.result)
            {
                return Json(new { result = true, data = res.data.price, msg = "สำเร็จ" });
            }
            return Json(new { result = false, msg = res.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, msg = $"ขออภัย, ไม่พบข้อมูลสินค้า. <br> {ex.Message}" });
        }
    }

    #region Private Method

    #endregion

    #region Partial Page
    public async Task<PartialViewResult> GetSellingItemPartialPage()
    {
        BaseResponse<GetItemInBranchByBranchIDResponseDTO> resItemInBranch = await _itemInBranchAPI.GetItemInBranchByBranchIDAsync(UserProfile.access_branch.FirstOrDefault().branchid);
        ViewBag.ItemInBranch = resItemInBranch.data.itemlist;
        return PartialView("_PartialPage/_SellingItemPartialPage");
    }
    #endregion
}
