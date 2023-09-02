using System.Text;
using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
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
    public SaleController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IItemInBranchAPI itemInBranchAPI) : base(httpClientRequest, mapper, log)
    {
        _itemInBranchAPI = itemInBranchAPI;
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
    /// ยังไม่เสร็จ :ควรแยกโหมด ซื้อ-ขาย
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

            //DateTime createDate = DateTime.Now;
            //List<RequestTransactionDetailDto> tmpDetails = new List<RequestTransactionDetailDto>();
            //RequestTransactionDto requestTransactionDto = new RequestTransactionDto
            //{
            //    CustID = 1,
            //    BranchID = BuyingBranchID,
            //    CreatedDate = createDate,
            //    CreatedBy = base.UserProfile.UserID,
            //    Status = (int)EnumDto.Status.Enable,
            //    TransactionTypeID = base.IsSellingCurrency == 0 ? (int)TransactionTypeMode.Buying : (int)TransactionTypeMode.Selling,
            //    TransactionDetails = new List<RequestTransactionDetailDto>()
            //};

            #region Get form value
            List<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>> form = Request.Form.ToList();
            #endregion

            #region Create Transaction detail
            decimal totalAmt = 0;
            decimal totalProfitAmt = 0;
            int idx = form.Where(w => w.Key.Contains("data[group-a]")).Count() / 4;
            for (int i = 0; i < idx; i++)
            {
                //RequestTransactionDetailDto detailDto = new RequestTransactionDetailDto();

                var code = form.Where(w => w.Key == $"data[group-a][{i}][ddlSearchItem]").FirstOrDefault().Value[0];
                var rate = form.Where(w => w.Key == $"data[group-a][{i}][txtItemPrice]").FirstOrDefault().Value[0];
                var qty = form.Where(w => w.Key == $"data[group-a][{i}][txtItemQty]").FirstOrDefault().Value[0];
                var amt = form.Where(w => w.Key == $"data[group-a][{i}][txtAmount]").FirstOrDefault().Value[0];

                if (!string.IsNullOrEmpty(code) &&
                    !string.IsNullOrEmpty(rate) &&
                    !string.IsNullOrEmpty(qty) &&
                    !string.IsNullOrEmpty(amt))
                {
                    totalAmt += decimal.Parse(amt);

                    //#region เปิ้ล ยังไม่เสร็จ : Get Buying Average Price ราคาเฉลี่ยแต่ละสกลุเงิน SUM(b.Price)/SUM(b.Qty) as avg_price เพื่อหา กำไรสุทธิ
                    //Check total qty in stock
                    //if (base.IsSellingCurrency.ToBool())
                    //{
                    //    int cutQty = await _currencyDataService.GetCurrencyQtyInStockByBranchId(code, BuyingBranchID);
                    //    if (cutQty < Convert.ToInt32(qty))
                    //    {
                    //        return Json(new { result = false, msg = $"ไม่สามารถทำรายการขายได้ เนื่องจากจำนวนใน Stock ไม่เพียงพอ (Stock: {cutQty})!." });
                    //    }

                    //}

                    //decimal avgBuyingRate = await _currencyDataService.GetCurrencyAverageRateByCodeBranchId(code, BuyingBranchID);
                    //decimal totalBuyingPrice = Math.Round(decimal.Multiply(Convert.ToDecimal(qty), Convert.ToDecimal(avgBuyingRate)), 4);
                    //decimal netProfit = Math.Round(Math.Round(decimal.Multiply(Convert.ToDecimal(qty), Convert.ToDecimal(rate)), 4) - totalBuyingPrice, 2);
                    //totalProfitAmt += netProfit;
                    //#endregion

                    //#region Create Detail
                    //var resDupplicate = tmpDetails.FirstOrDefault(w => w.Currency.Trim().ToUpper() == code.Trim().ToUpper());
                    //if (resDupplicate == null)
                    //{
                    //    //New
                    //    detailDto.Currency = code;
                    //    detailDto.Qty = int.Parse(qty);
                    //    detailDto.Rate = double.Parse(rate);
                    //    detailDto.Price = decimal.Parse(amt);
                    //    detailDto.CreatedBy = base.UserProfile.UserID;
                    //    detailDto.CreatedDate = createDate;
                    //    detailDto.Status = (int)EnumDto.Status.Enable;

                    //    tmpDetails.Add(detailDto);
                    //}
                    //else
                    //{
                    //    //Dupp
                    //    resDupplicate.Qty += int.Parse(qty);
                    //    resDupplicate.Price += decimal.Parse(amt);
                    //}
                    //#endregion
                }
            }
            #endregion

            #region Create Object
            //requestTransactionDto.TotalPrice = totalAmt;
            //requestTransactionDto.TotalNetProfit = totalProfitAmt;
            //requestTransactionDto.TransactionDetails = tmpDetails;
            #endregion

            //RequestTransactionDto resHtml = requestTransactionDto;

            //if (tmpDetails.Count > 0)
            //{
            //    #region Render HTML Popup before confirm summit
            //    StringBuilder strb = new StringBuilder();
            //    strb.Append("<div class='font-size-small'>");
            //    strb.Append("<table class='table table-responsive-sm'>");
            //    strb.Append("<thead style='font-weight:bold'><tr><td>สกุลเงิน</td><td>จำนวน</td><td>เรท</td><td>ราคา</td></tr></thead>");
            //    strb.Append("<tbody style='color: blue'>");
            //    #region Create Summary Html
            //    tmpDetails.ForEach(x =>
            //    {
            //        strb.Append($"<tr><td>{x.Currency}</td><td>{x.Qty}</td><td style='color: red; font-weight:bold'>{x.Rate:0,0.0000}</td><td>{x.Price:0,0.00}</td></tr>");
            //    });
            //    #endregion
            //    strb.Append("</tbody>");

            //    //If is selling mode
            //    if (base.IsSellingCurrency.ToBool())
            //    {
            //        strb.Append("<tfoot>");
            //        strb.Append($"<tr><td></td><td></td><td colspan='2' class='text-right' style='font-weight:bold'>ราคารวม  <span style='font-size:14px; color: blue'><u>{requestTransactionDto.TotalPrice.Value:0,0.00}</u></span> บาท</td></tr>");
            //        strb.Append($"<tr><td></td><td></td><td colspan='2' class='text-right' style='font-weight:bold'>กำไรสุทธิ  <span style='font-size:14px; color: #049104'><u>{requestTransactionDto.TotalNetProfit.Value:0,0.00}</u></span> บาท</td></tr>");
            //        strb.Append("</tfoot>");
            //    }
            //    else
            //    {
            //        strb.Append($"<tfoot><tr><td></td><td></td><td colspan='2' class='text-right' style='font-weight:bold'>ราคารวม  <span style='font-size:14px; color: blue'><u>{requestTransactionDto.TotalPrice.Value:0,0.00}</u></span> บาท</td></tr></tfoot>");
            //    }
            //    strb.Append("</table></div>");
            //    #endregion
            //    return Json(new { result = true, msg = $"{strb}" });
            //}
            //else
            //{
            //    return Json(new { result = false, msg = "ขออภัย รูปแบบข้อมูลไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!." });
            //}
            return Json(new { result = true, msg = "ขออภัย รูปแบบข้อมูลไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!." });

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

            decimal totalAmt = 0;
            decimal totalProfitAmt = 0;
            int idx = form.Where(w => w.Key.Contains("data[group-a]")).Count() / 4;

            return Json(new { result = false, msg = "ขออภัย รูปแบบข้อมูลไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!." });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, msg = $"ขออภัย รูปแบบข้อมูลไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!. {ex.Message}" });
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
