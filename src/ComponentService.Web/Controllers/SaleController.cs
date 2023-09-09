using System.Globalization;
using System.Text;
using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.ItemAPI;
using CYRetailIMS.Application.ExternalService.ItemBrandAPI;
using CYRetailIMS.Application.ExternalService.ItemInBranchAPI;
using CYRetailIMS.Application.ExternalService.TransactionAPI;
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandList.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchList.v1;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using CYRetailIMS.ComponentService.Web.Models;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Staff)]
public class SaleController : BaseController
{
    private readonly IItemInBranchAPI _itemInBranchAPI;
    private readonly IItemAPI _itemAPI;
	private readonly ITransactionAPI _transactionAPI;

    public SaleController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IItemInBranchAPI itemInBranchAPI,
        IItemAPI itemAPI,
		ITransactionAPI transactionAPI) : base(httpClientRequest, mapper, log)
    {
        _itemInBranchAPI = itemInBranchAPI;
        _itemAPI = itemAPI;
		_transactionAPI = transactionAPI;
    }

    public async Task<IActionResult> IndexAsync()
    {
        BaseResponse<List<GetItemInBranchByBranchListResponseDTO>> resItemBrandList = await _itemInBranchAPI.GetItemInBranchByBranchListAsync(new GetItemInBranchByBranchListQuery
		{
            branchid_list = base.UserProfile.access_branch.Select(s => s.branchid).ToList()
		});

		BaseResponse<List<GetTransactionByBranchIDResponseDTO>> resTransaction = await _transactionAPI.GetTransactionByBranchIDAsync(base.UserProfile.access_branch.FirstOrDefault().branchid);

		ViewBag.BranchList = base.UserProfile.access_branch;
        ViewBag.ItemBranchList = resItemBrandList.data.SelectMany(s => s.itemlist);
        ViewBag.TransactionList = resTransaction;
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
            if (form.Count == 0)
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

    [HttpPost]
    public async Task<IActionResult> SaveSellingItem(SellingItemViewModel sellingItemObj)
    {
        try
        {
            if (!base.UserProfile.access_branch.Any(w => w.branchid == sellingItemObj.branch.ToInt32()))
            {
                return Json(new { result = false, msg = $"{GlobalMessageModel.ErrorInvalidBranch}" });
            }

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

            #region PrePare TransactionRequest
            List<CreateTransactionDetailCommand> createTransactionDetailCommands = new List<CreateTransactionDetailCommand>();
            #endregion

            decimal totalAmt = 0;
            decimal totalProfitAmt = 0;
            int idx = form.Count / 4;
            for (int i = 0; i < idx; i++)
            {
                var itemid = form.Where(w => w.Key == $"outer-item-group[{i}][ddlSearchItem]").FirstOrDefault().Value[0];
                var itemprice = form.Where(w => w.Key == $"outer-item-group[{i}][txtItemPrice]").FirstOrDefault().Value[0];
                var qty = form.Where(w => w.Key == $"outer-item-group[{i}][txtItemQty]").FirstOrDefault().Value[0];
                var amt = form.Where(w => w.Key == $"outer-item-group[{i}][txtAmount]").FirstOrDefault().Value[0];

                if (!string.IsNullOrEmpty(itemid) &&
                    !string.IsNullOrEmpty(itemprice) &&
                    !string.IsNullOrEmpty(qty) &&
                    !string.IsNullOrEmpty(amt))
                {
                    createTransactionDetailCommands.Add(new CreateTransactionDetailCommand
                    {
                        itemid = itemid.ToInt32(),
                        price = itemprice.ToDecimal(),
                        qty = qty.ToInt32(),
                        //amount = amt.ToDecimal(),
                        amount = decimal.Multiply(itemprice.ToDecimal(), qty.ToInt32()),
                        isactive = true
                    });
                }
            }

            #region Prepare & Create Transaction
            CreateTransactionCommand createTransactionCommand = PrepareCreateTransactionCommand(sellingItemObj, createTransactionDetailCommands);
            BaseResponse<CommandResponse> resCreateTrn = await _transactionAPI.CreateTransactionAsync(createTransactionCommand);
            if (!resCreateTrn.result)
            {
                return Json(new { result = false, msg = resCreateTrn.error.error.message });
            }
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
    private CreateTransactionCommand PrepareCreateTransactionCommand(SellingItemViewModel reqObj, List<CreateTransactionDetailCommand> createTransactionDetailCommands)
    {
        //DateTime.TryParseExact(reqObj.saledate, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, out DateTime dt);
        //var dt2 = DateTime.ParseExact(reqObj.saledate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //var dt3 = DateTime.ParseExact(reqObj.saledate, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));

        decimal toalAmt = createTransactionDetailCommands.Select(s => decimal.Multiply(s.price, s.qty)).Sum();
        return new CreateTransactionCommand
        {
            transactiontypeid = 1, //Retail
            amountcash = reqObj.mcash,
            amountdeposit = reqObj.mdeposit,
            amounttransfer = reqObj.mtransfer,
            branchid = reqObj.branch.ToInt32(),
            totalamount = toalAmt,
            isactive = true,
            isexcludevat = false,
            transactiondate = reqObj.saledate.ToDateTime(),
            creadeddate = DateTime.Now,
            createdby = base.UserProfile.username,
            transactiondetail = createTransactionDetailCommands
        };
    }
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
