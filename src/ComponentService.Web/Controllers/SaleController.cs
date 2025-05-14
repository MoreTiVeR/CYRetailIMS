using System.Collections.Generic;
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
using CYRetailIMS.Application.ExternalService.ItemTypeAPI;
using CYRetailIMS.Application.ExternalService.ItemUnitOfMeasureAPI;
using CYRetailIMS.Application.ExternalService.TransactionAPI;
using CYRetailIMS.Application.ExternalService.TransactionTypeAPI;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandList.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Commands.DeleteItemInBranch.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Commands.UpdateItemInBranch.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByCriteria.v1;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemByBarcode.v1;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemByID.v1;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeList.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReport.v1;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v2;
using CYRetailIMS.Application.Services.TransactionTypeService.Queries.GetTrasnactionList.v1;
using CYRetailIMS.Application.Services.TransferTypeService.Queries.GetTransferTypeList.v1;
using CYRetailIMS.Application.Services.UnitOfMeasureService.Queries.GetUnitOfMeasureList.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using CYRetailIMS.ComponentService.Web.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Infrastructure.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetTopologySuite.Index.HPRtree;
using NUglify.Helpers;
using static CYRetailIMS.Application.Common.Models.EnumModel;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Sale, RoleName.AreaSale)]
public class SaleController : BaseController
{
    private string _sessionTempSellingItemBarcodeScannerName => "TEMP_SELLING_ITEM_BARCODE_DATA";
    private string _sessionTempSellingItemBarcodeMobileName => "TEMP_SELLING_ITEM_BARCODE_MOBILE_DATA";
    private string _sessionTempSaleItemData => "SALE_ITEM_DATA";

    private readonly IItemInBranchAPI _itemInBranchAPI;
    private readonly IItemAPI _itemAPI;
    private readonly ITransactionAPI _transactionAPI;
    private readonly IItemBrandAPI _itemBrandAPI;
    private readonly IItemTypeAPI _itemTypeAPI;
    private readonly IItemUnitOfMeasureAPI _itemUnitOfMeasureAPI;
    private readonly ITransactionTypeAPI _transactionTypeAPI;

    public SaleController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IItemInBranchAPI itemInBranchAPI,
        IItemAPI itemAPI,
        ITransactionAPI transactionAPI,
        IItemBrandAPI itemBrandAPI,
        IItemTypeAPI itemTypeAPI,
        IItemUnitOfMeasureAPI itemUnitOfMeasureAPI,
        ITransactionTypeAPI transactionTypeAPI) : base(httpClientRequest, mapper, log)
    {
        _itemInBranchAPI = itemInBranchAPI;
        _itemAPI = itemAPI;
        _transactionAPI = transactionAPI;
        _itemBrandAPI = itemBrandAPI;
        _itemTypeAPI = itemTypeAPI;
        _itemUnitOfMeasureAPI = itemUnitOfMeasureAPI;
        _transactionTypeAPI = transactionTypeAPI;
    }

    public async Task<IActionResult> Index()
    {
        BaseResponse<GetItemInBranchByBranchIDResponseDTO> resItemBranch = await _itemInBranchAPI.GetItemInBranchByBranchIDAsync(base.UserProfile.access_branch.FirstOrDefault().branchid);

        //BaseResponse<List<GetTransactionByBranchIDResponseDTO>> resTransaction = await _transactionAPI.GetTransactionByBranchIDAsync(base.UserProfile.access_branch.FirstOrDefault().branchid);

        //if (!resTransaction.result)
        //{
        //    resTransaction.data = new List<GetTransactionByBranchIDResponseDTO>();
        //}
        //resTransaction.data = resTransaction.data.OrderByDescending(s => s.transactiondate).ToList();
        ViewBag.BranchList = base.UserProfile.access_branch;
        ViewBag.ItemBranch = resItemBranch;
        //ViewBag.TransactionList = resTransaction;
        return View();
    }

    public async Task<IActionResult> Create()
    {
        BaseResponse<GetItemInBranchByBranchIDResponseDTO> resItemInBranch = await _itemInBranchAPI.GetItemInBranchByBranchIDAsync(UserProfile.access_branch.FirstOrDefault().branchid);
        ViewBag.ItemInBranch = resItemInBranch;
        return View();
    }

    public async Task<IActionResult> Barcode()
    {
        #region Get- Set Item
        BaseResponse<List<GetItemListResponseDTO>> resItemList = await GetItemSessionDataAsync();
        #endregion

        BaseResponse<List<GetTrasnactionByCriteriaResponseDTO>> resTransactionType = await _transactionTypeAPI.GetTransactionTypeByCriteriaAsync(new GetTrasnactionByCriteriaQuery
        {
            isactive = true
        });
        ViewBag.TransactionType = resTransactionType;
        ViewBag.SellingTransactionTypeList = PrepareSelectSellingType();
        return View();
    }

    public async Task<IActionResult> Mobile()
    {
        #region Get- Set Item
        BaseResponse<List<GetItemListResponseDTO>> resItemList = await GetItemSessionDataAsync();
        #endregion

        BaseResponse<List<GetTrasnactionByCriteriaResponseDTO>> resTransactionType = await _transactionTypeAPI.GetTransactionTypeByCriteriaAsync(new GetTrasnactionByCriteriaQuery
        {
            isactive = true
        });
        ViewBag.TransactionType = resTransactionType;
        ViewBag.SellingTransactionTypeList = PrepareSelectSellingType();
        return View();
    }

    /// <summary>
    /// ค้นหาสำหรับ Select2
    /// http://dotnetqueries.com/Article/159/how-to-implement-select2-with-ajax-and-json-in-asp-net-mvc
    /// </summary>
    /// <param name="search"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<JsonResult> SearchItemBranchs(string search, string type)
    {
        try
        {
            BaseResponse<GetItemInBranchByBranchIDResponseDTO> resItemInBranch = await _itemInBranchAPI.GetItemInBranchByBranchIDAsync(UserProfile.access_branch.FirstOrDefault().branchid);
            List<SelectListItem> searchItemList = resItemInBranch.data.itemlist.Where(w => w.itemcode.ToLower().StartsWith(search.ToLower())
            || w.itemname.ToLower().StartsWith(search.ToLower())).Select(s => new SelectListItem
            {
                Value = s.itemid.ToString(),
                Text = s.itemname
            }).ToList();
            return Json(new { items = searchItemList });
        }
        catch (Exception ex)
        {
            return Json(new { items = new List<SelectListItem>(), message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    public async Task<IActionResult> ItemsAsync()
    {
        //Default first branch
        BaseResponse<GetItemInBranchByBranchIDResponseDTO> resItemInBranch = await _itemInBranchAPI.GetItemInBranchByBranchIDAsync(base.UserProfile.access_branch.FirstOrDefault().branchid);

        ViewBag.BranchList = base.UserProfile.access_branch;
        ViewBag.ItemBranch = resItemInBranch;
        return View();
    }

    public IActionResult ReceiveItem()
    {
        return View();
    }

    public async Task<IActionResult> Edit(int itemid)
    {
        //Get Item Detail
        BaseResponse<GetItemInBranchByCriteriaResponseDTO> resItemBranch = await _itemInBranchAPI.GetItemInBranchByCriteriaAsync(new GetItemInBranchByCriteriaQuery
        {
            branchid = base.UserProfile.access_branch.FirstOrDefault().branchid,
            itemid = itemid
        });
        EditItemViewModel viewModel = EditItemMapping(resItemBranch.data.item);

        //Get Master Data
        BaseResponse<List<GetItemTypeListResponseDTO>> resItemTypeList = await _itemTypeAPI.GetItemTypeListAsync();
        BaseResponse<List<GetItemBrandListResponseDTO>> resItemBrandList = await _itemBrandAPI.GetItemBrandListAsync();
        BaseResponse<List<GetUnitOfMeasureListResponseDTO>> resUnitOfMeasureList = await _itemUnitOfMeasureAPI.GetUnitOfMeasureListAsync();

        ViewBag.ItemTypeList = resItemTypeList;
        ViewBag.ItemBrandList = resItemBrandList;
        ViewBag.ItemUOMList = resUnitOfMeasureList;
        return View(viewModel);
    }

    public IActionResult CountStockHistory()
    {
        return View();
    }

    [HttpGet]
    public IActionResult GetCountStockHistory()
    {
        try
        {
            //int dayInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            //BaseResponse<List<SaleSummaryReportResponseDTO>> resSaleSummaryReport = await _reportAPI.GetSaleSummaryReportAsync(new SaleSummaryReportQuery
            //{
            //    starttransactiondate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
            //    endtransactiondate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, dayInMonth)
            //});
            //if (!resSaleSummaryReport.result)
            //{
            //    throw new Exception(resSaleSummaryReport.error.error.message);
            //}
            //return Json(new { data = resSaleSummaryReport.data });
            return Json(new { result = true, message="สำเร็จ", data = new List<SaleSummaryReportResponseDTO>() });
        }
        catch
        {
            return Json(new { result = false, message = "ไม่สามารถดึงข้อมูลนับสต๊อกได้ กรุณาลองใหม่อีกครั้ง", data = new List<SaleSummaryReportResponseDTO>() });
        }
    }

    public IActionResult CreateCountStock()
    {
        return View();
    }

    public IActionResult EditCountStock(int countstockid)
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
            int idx = form.Count / 5;
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
            CreateTransactionCommand createTransactionCommand = PrepareCreateTransactionCommand(sellingItemObj, createTransactionDetailCommands, SellTransactionType.RT);
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
                return Json(new { result = true, price = res.data.price, curqty = res.data.qty, msg = "สำเร็จ" });
            }
            return Json(new { result = false, msg = res.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, msg = $"ขออภัย, ไม่พบข้อมูลสินค้า. <br> {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> GetItemPriceByCriteria(SearchItemByCriteriaViewModel searchItem)
    {
        try
        {
            if (!IsValidSearchCriteria(searchItem))
            {
                return Json(new { result = false, msg = $"ข้อมูลค้นหาไม่ถูกต้อง, กรุณาลองใหม่อีกครั้ง" });
            }
            var res = await _itemInBranchAPI.GetItemInBranchByCriteriaAsync(new GetItemInBranchByCriteriaQuery { branchid = searchItem.branchid, itemid = searchItem.itemid });
            if (res.result)
            {
                return Json(new { result = true, data = res.data.item, msg = "สำเร็จ" });
            }
            return Json(new { result = false, msg = res.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, msg = $"ขออภัย, ไม่พบข้อมูลสินค้า. <br> {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditItem([FromBody] EditItemViewModel editItemObj)
    {
        try
        {
            if (base.UserProfile.roleid != (int)UserRole.Admin)
            {
                return Json(new JsonViewModel { result = false, message = "ขออภัย, คุณไม่มีสิทธิ์ในการทำรายการ" });
            }
            UpdateItemInBranchCommand updateItemCommand = PrepareUpdateItemInBranch(editItemObj);
            BaseResponse<CommandResponse> resUpdateItem = await _itemInBranchAPI.UpdateItemInBranchAsync(updateItemCommand);
            if (resUpdateItem.result)
            {
                return Json(new JsonViewModel { result = resUpdateItem.result, message = "ปรับปรุงข้อมูลสินค้าสำเร็จ" });
            }
            return Json(new JsonViewModel { result = resUpdateItem.result, message = resUpdateItem.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteItem([FromBody] DeleteItemViewModel delItemObj)
    {
        try
        {
            if (base.UserProfile.roleid != (int)UserRole.Admin)
            {
                return Json(new JsonViewModel { result = false, message = "ขออภัย, คุณไม่มีสิทธิ์ในการทำรายการ" });
            }
            BaseResponse<CommandResponse> resDeleteItem = await _itemInBranchAPI.DeleteItemInBranchAsync(new DeleteItemInBranchCommand
            {
                branchid = base.UserProfile.access_branch.FirstOrDefault().branchid,
                itemid = delItemObj.itemid,
                updatedby = base.UserProfile.username,
                updateddate = DateTime.Now
            });
            if (resDeleteItem.result)
            {
                return Json(new JsonViewModel { result = resDeleteItem.result, message = "ลบสินค้าสำเร็จ" });
            }
            return Json(new JsonViewModel { result = resDeleteItem.result, message = resDeleteItem.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    #region Private Method
    private CreateTransactionCommand PrepareCreateTransactionCommand(SellingItemViewModel reqObj, 
        List<CreateTransactionDetailCommand> createTransactionDetailCommands,
        SellTransactionType sellTransactionType)
    {
        decimal toalAmt = createTransactionDetailCommands.Select(s => decimal.Multiply(s.price, s.qty)).Sum();
        return new CreateTransactionCommand
        {
            transactiontypeid = (int)sellTransactionType,
            amountcash = reqObj.mcash,
            amountdeposit = reqObj.mdeposit,
            amounttransfer = reqObj.mtransfer,
            fee = reqObj.mfee,
            branchid = reqObj.branch.ToInt32(),
            totalamount = toalAmt,
            isactive = true,
            isexcludevat = false,
            transactiondate = reqObj.saledate.ToDate(),
            createddate = DateTime.Now,
            createdby = base.UserProfile.username,
            transactiondetail = createTransactionDetailCommands,
            remark = reqObj.Remark
        };
    }

    private CreateTransactionCommand PrepareCreateTransactionByBarcodeCommand(SellingItemViewModel reqObj,
        List<CreateTransactionDetailCommand> createTransactionDetailCommands)
    {
        decimal toalAmt = createTransactionDetailCommands.Select(s => decimal.Multiply(s.price, s.qty)).Sum();
        bool isPayWithCash = reqObj.iscash.HasValue && reqObj.iscash == true ? true : false;
        decimal mDeposit = isPayWithCash ? toalAmt - 1 : 0;
        decimal nDepositFee = isPayWithCash ? 1 : 0;
        decimal mTransfer = isPayWithCash ? 0 : toalAmt;
        int transactionType = reqObj.transactiontype != null ? reqObj.transactiontype.Value : (int)EnumModel.SellTransactionType.RT01;
        return new CreateTransactionCommand
        {
            transactiontypeid = transactionType,
            amountcash = reqObj.mcash,
            amountdeposit = mDeposit,
            amounttransfer = mTransfer,
            fee = nDepositFee,
            branchid = reqObj.branch.ToInt32(),
            totalamount = toalAmt,
            isactive = true,
            isexcludevat = false,
            transactiondate = reqObj.saledate.ToDate(),
            createddate = DateTime.Now,
            createdby = base.UserProfile.username,
            transactiondetail = createTransactionDetailCommands,
            remark = reqObj.Remark,
            paymenttypeid = isPayWithCash ? (int)EnumModel.PaymentType.CA : (int)EnumModel.PaymentType.TR
        };
    }

    private UpdateItemInBranchCommand PrepareUpdateItemInBranch(EditItemViewModel reqData)
    {
        return new UpdateItemInBranchCommand
        {
            itemid = reqData.ItemID,
            branchid = base.UserProfile.access_branch.FirstOrDefault().branchid,
            //isactive = reqData.IsActive.ToBool(),
            price = reqData.Price,
            qty = reqData.Qty,
            updatedby = base.UserProfile.username,
            updateddate = DateTime.Now
        };
    }

    private EditItemViewModel EditItemMapping(GetItemListResponseDTO itemResponseDTO)
    {
        EditItemViewModel editItemViewModel = _mapper.Map<EditItemViewModel>(itemResponseDTO);
        return editItemViewModel;
    }

    private EditItemViewModel EditItemMapping(GetItemInBranchByBranchIDItemResponseDTO itemResponseDTO)
    {
        EditItemViewModel editItemViewModel = _mapper.Map<EditItemViewModel>(itemResponseDTO);
        return editItemViewModel;
    }

    #endregion

    #region Partial Page
    public async Task<PartialViewResult> GetSellingItemPartialPage()
    {
        BaseResponse<GetItemInBranchByBranchIDResponseDTO> resItemInBranch = await _itemInBranchAPI.GetItemInBranchByBranchIDAsync(UserProfile.access_branch.FirstOrDefault().branchid);
        ViewBag.ItemInBranch = resItemInBranch.data.itemlist;
        return PartialView("_PartialPage/_SellingItemPartialPage");
    }

    private bool IsValidSearchCriteria(SearchItemByCriteriaViewModel reqObj)
    {
        if (reqObj.branchid == 0 || reqObj.itemid == 0)
        {
            return false;
        }
        return true;
    }
    #endregion

    #region Selling via Scanner
    [AllowAnonymous]
    [HttpGet]
    public IActionResult GetTempItemDataAsync()
    {
        try
        {
            List<SellingBarcodeItemViewModel> tempList = HttpContext.Session.GetDataFromSession<List<SellingBarcodeItemViewModel>>(_sessionTempSellingItemBarcodeScannerName);

            #region if list is null => create new list with 0 member
            if (tempList == null)
            {
                tempList = new List<SellingBarcodeItemViewModel>();
                HttpContext.Session.SetDataToSession(_sessionTempSellingItemBarcodeScannerName, tempList);
            }
            #endregion
            return Json(new { data = tempList.OrderBy(o => o.seq).ToList(), amount = tempList.Sum(s => s.totalprice) });
        }
        catch
        {
            return Json(new { data = new List<TransferItemDetailViewModel>() });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> AddTempItemSellingBarcode([FromBody] SellingBarcodeItemViewModel sellingBarcodeItem)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { result = false, message = "กรุณาตรวจสอบจำนวนสินค้า/บาร์โค้ดให้ถูกต้อง" });
        }

        try
        {
            if (sellingBarcodeItem.qty <= 0)
            {
                return Json(new { result = false, message = "กรุณาระบุจำนวนสินค้าไม่น้อยกว่า 0" });
            }

            //Get Current List
            List<SellingBarcodeItemViewModel> tempSellingBarcodeItemList = HttpContext.Session.GetDataFromSession<List<SellingBarcodeItemViewModel>>(_sessionTempSellingItemBarcodeScannerName);

            #region Update when Already added
            if (tempSellingBarcodeItemList != null)
            {
                //Check is exist from temp
                SellingBarcodeItemViewModel existData = tempSellingBarcodeItemList.FirstOrDefault(w => w.barcode == sellingBarcodeItem.barcode);
                if (existData != null)
                {
                    //Update QTY
                    tempSellingBarcodeItemList.Where(w => w.barcode == sellingBarcodeItem.barcode).ForEach(e =>
                    {
                        e.qty = e.qty + sellingBarcodeItem.qty;
                    });
                }
                else
                {
                    //Add new if doesn't exist in temp list
                    int lastId = tempSellingBarcodeItemList != null && tempSellingBarcodeItemList.Count > 0 ? tempSellingBarcodeItemList.Last().qty : 0;
                    lastId++;
                    sellingBarcodeItem.seq = lastId;
                    MappingSellingBarcodeItem(ref sellingBarcodeItem);
                    tempSellingBarcodeItemList.Add(sellingBarcodeItem);
                }
            }
            else
            {
                tempSellingBarcodeItemList = new List<SellingBarcodeItemViewModel>();
                //Add new get last seq
                int lastId = tempSellingBarcodeItemList != null && tempSellingBarcodeItemList.Count > 0 ? tempSellingBarcodeItemList.Last().seq : 0;
                lastId++;
                sellingBarcodeItem.seq = lastId;
                MappingSellingBarcodeItem(ref sellingBarcodeItem);
                tempSellingBarcodeItemList.Add(sellingBarcodeItem);
            }
            #endregion

            #region Validate Qty in Stock TMItem by barcode before response
            BaseResponse<GetItemByIDResponseDTO> resItem = await _itemAPI.GetItemByBarCodeV2Async(new GetItemByBarcodeQuery { itembarcode = sellingBarcodeItem.barcode });
            if (!resItem.result)
            {
                return Json(new { result = false, message = $"{resItem.error.error.message} บาร์โค้ด {sellingBarcodeItem.barcode}" });
            }

            if (resItem.data.qty < tempSellingBarcodeItemList.FirstOrDefault(w => w.itemid == resItem.data.itemid)?.qty)
            {
                return Json(new { result = false, message = $"ไม่สามารภทำรายการได้, เนื่องจากจำนวนสต๊อกสินไม่เพียงพอ" });
            }
            #endregion

            HttpContext.Session.SetDataToSession(_sessionTempSellingItemBarcodeScannerName, tempSellingBarcodeItemList);
            return Json(new { result = true, message = "เพิ่มสินค้าสำเร็จ", amount = tempSellingBarcodeItemList.Sum(w => w.totalprice) });

        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public JsonResult DeleteTempItemSellingBarcode(int seq)
    {
        try
        {
            List<SellingBarcodeItemViewModel> tempTransferItemList = HttpContext.Session.GetDataFromSession<List<SellingBarcodeItemViewModel>>(_sessionTempSellingItemBarcodeScannerName);
            SellingBarcodeItemViewModel todo = tempTransferItemList?.FirstOrDefault(m => m.seq == seq);
            if (todo == null)
            {
                throw new Exception("ไม่สามารถลบข้อมูลได้");
            }

            tempTransferItemList.Remove(todo);
            HttpContext.Session.SetDataToSession(_sessionTempSellingItemBarcodeScannerName, tempTransferItemList);
            return Json(new { result = true, message = "ลบข้อมูลสำเร็จ.", amount = tempTransferItemList.Sum(w => w.totalprice) });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> SaveSellingItemByBarcode([FromBody] SellingItemViewModel sellingItemObj)
    {
        try
        {
            #region PrePare TransactionRequest
            List<SellingBarcodeItemViewModel> tempSellingBarcodeItemList = HttpContext.Session.GetDataFromSession<List<SellingBarcodeItemViewModel>>(_sessionTempSellingItemBarcodeScannerName);
            List<CreateTransactionDetailCommand> createTransactionDetailCommands = tempSellingBarcodeItemList.Select(s => new CreateTransactionDetailCommand
            {
                itemid = s.itemid,
                price = s.itemprice,
                qty = s.qty,
                amount = decimal.Multiply(s.itemprice, s.qty),
                isactive = true
            }).ToList();
            #endregion

            #region Prepare & Create Transaction
            CreateTransactionCommand createTransactionCommand = PrepareCreateTransactionByBarcodeCommand(sellingItemObj, createTransactionDetailCommands);
            BaseResponse<CommandResponse> resCreateTrn = await _transactionAPI.CreateTransactionAsync(createTransactionCommand);
            if (!resCreateTrn.result)
            {
                return Json(new { result = false, msg = resCreateTrn.error.error.message });
            }
            #endregion

            HttpContext.Session.Remove(_sessionTempSellingItemBarcodeScannerName);
            return Json(new { result = true, msg = "บันทึกข้อมูลสำเร็จ." });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, msg = $"ขออภัย รูปแบบข้อมูลไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!. {ex.Message}" });
        }
    }

    #endregion

    #region Selling via Mobile Camera
    [AllowAnonymous]
    [HttpGet]
    public IActionResult GetTempItemDataMobile()
    {
        try
        {
            List<SellingBarcodeItemViewModel> tempList = HttpContext.Session.GetDataFromSession<List<SellingBarcodeItemViewModel>>(_sessionTempSellingItemBarcodeMobileName);

            #region if list is null => create new list with 0 member
            if (tempList == null)
            {
                tempList = new List<SellingBarcodeItemViewModel>();
                HttpContext.Session.SetDataToSession(_sessionTempSellingItemBarcodeMobileName, tempList);
            }
            #endregion
            return Json(new { data = tempList.OrderBy(o => o.seq).ToList(), amount = tempList.Sum(s => s.totalprice) });
        }
        catch
        {
            return Json(new { data = new List<TransferItemDetailViewModel>() });
        }
    }

    [HttpPost]
    public IActionResult IsExistItemDataByMobileBarcode([FromBody] SellingCheckExistItemByBarcodeModel reqSellingCheckExistItem)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { result = false, message = "กรุณาตรวจสอบจำนวนสินค้า/บาร์โค้ดให้ถูกต้อง" });
        }
        try
        {
            //Get Current List
            List<SellingBarcodeItemViewModel> tempSellingBarcodeItemList = HttpContext.Session.GetDataFromSession<List<SellingBarcodeItemViewModel>>(_sessionTempSellingItemBarcodeMobileName);
            if(tempSellingBarcodeItemList != null)
            {
                SellingBarcodeItemViewModel resExist = tempSellingBarcodeItemList.FirstOrDefault(s => s.barcode == reqSellingCheckExistItem.barcode);
                if (resExist != null)
                {
                    return Json(new { result = true, message = $"ต้องการเพิ่มจำนวนสินค้า {resExist.itemname} รายการเดิมหรือไม่?" });
                }
            }
            return Json(new { result = false, message = $"ตรวจสอบข้อมูลสำเร็จ ไม่พบราบการสินค้าซ้ำ." });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddTempItemSellingMobileBarcode([FromBody] SellingBarcodeItemViewModel sellingBarcodeItem)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { result = false, message = "กรุณาตรวจสอบจำนวนสินค้า/บาร์โค้ดให้ถูกต้อง" });
        }

        try
        {
            if (sellingBarcodeItem.qty <= 0)
            {
                return Json(new { result = false, message = "กรุณาระบุจำนวนสินค้าไม่น้อยกว่า 0" });
            }

            //Get Current List
            List<SellingBarcodeItemViewModel> tempSellingBarcodeItemList = HttpContext.Session.GetDataFromSession<List<SellingBarcodeItemViewModel>>(_sessionTempSellingItemBarcodeMobileName);

            #region Update when Already added
            if (tempSellingBarcodeItemList != null)
            {
                //Check is exist from temp
                SellingBarcodeItemViewModel existData = tempSellingBarcodeItemList.FirstOrDefault(w => w.barcode == sellingBarcodeItem.barcode);
                if (existData != null)
                {
                    //Update QTY
                    tempSellingBarcodeItemList.Where(w => w.barcode == sellingBarcodeItem.barcode).ForEach(e =>
                    {
                        e.qty = e.qty + sellingBarcodeItem.qty;
                    });
                }
                else
                {
                    //Add new if doesn't exist in temp list
                    int lastId = tempSellingBarcodeItemList != null && tempSellingBarcodeItemList.Count > 0 ? tempSellingBarcodeItemList.Last().qty : 0;
                    lastId++;
                    sellingBarcodeItem.seq = lastId;
                    MappingSellingBarcodeItem(ref sellingBarcodeItem);
                    tempSellingBarcodeItemList.Add(sellingBarcodeItem);
                }
            }
            else
            {
                tempSellingBarcodeItemList = new List<SellingBarcodeItemViewModel>();
                //Add new get last seq
                int lastId = tempSellingBarcodeItemList != null && tempSellingBarcodeItemList.Count > 0 ? tempSellingBarcodeItemList.Last().seq : 0;
                lastId++;
                sellingBarcodeItem.seq = lastId;
                MappingSellingBarcodeItem(ref sellingBarcodeItem);
                tempSellingBarcodeItemList.Add(sellingBarcodeItem);
            }
            #endregion

            #region Validate Qty in Stock TMItem by barcode before response
            BaseResponse<GetItemByIDResponseDTO> resItem = await _itemAPI.GetItemByBarCodeV2Async(new GetItemByBarcodeQuery { itembarcode = sellingBarcodeItem.barcode });
            if (!resItem.result)
            {
                return Json(new { result = false, message = $"{resItem.error.error.message} บาร์โค้ด {sellingBarcodeItem.barcode}" });
            }

            if (resItem.data.qty < tempSellingBarcodeItemList.FirstOrDefault(w => w.itemid == resItem.data.itemid)?.qty)
            {
                return Json(new { result = false, message = $"ไม่สามารภทำรายการได้, เนื่องจากจำนวนสต๊อกสินไม่เพียงพอ" });
            }
            #endregion

            HttpContext.Session.SetDataToSession(_sessionTempSellingItemBarcodeMobileName, tempSellingBarcodeItemList);
            return Json(new { result = true, message = "เพิ่มสินค้าสำเร็จ", amount = tempSellingBarcodeItemList.Sum(w => w.totalprice) });

        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public JsonResult DeleteTempItemSellingMobileBarcode(int seq)
    {
        try
        {
            List<SellingBarcodeItemViewModel> tempTransferItemList = HttpContext.Session.GetDataFromSession<List<SellingBarcodeItemViewModel>>(_sessionTempSellingItemBarcodeMobileName);
            SellingBarcodeItemViewModel todo = tempTransferItemList?.FirstOrDefault(m => m.seq == seq);
            if (todo == null)
            {
                throw new Exception("ไม่สามารถลบข้อมูลได้");
            }

            tempTransferItemList.Remove(todo);
            HttpContext.Session.SetDataToSession(_sessionTempSellingItemBarcodeMobileName, tempTransferItemList);
            return Json(new { result = true, message = "ลบข้อมูลสำเร็จ.", amount = tempTransferItemList.Sum(w => w.totalprice) });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> SaveSellingItemByMobileBarcode([FromBody] SellingItemViewModel sellingItemObj)
    {
        try
        {
            #region PrePare TransactionRequest
            List<SellingBarcodeItemViewModel> tempSellingBarcodeItemList = HttpContext.Session.GetDataFromSession<List<SellingBarcodeItemViewModel>>(_sessionTempSellingItemBarcodeMobileName);
            List<CreateTransactionDetailCommand> createTransactionDetailCommands = tempSellingBarcodeItemList.Select(s => new CreateTransactionDetailCommand
            {
                itemid = s.itemid,
                price = s.itemprice,
                qty = s.qty,
                amount = decimal.Multiply(s.itemprice, s.qty),
                isactive = true
            }).ToList();
            #endregion

            #region Prepare & Create Transaction
            CreateTransactionCommand createTransactionCommand = PrepareCreateTransactionByBarcodeCommand(sellingItemObj, createTransactionDetailCommands);
            BaseResponse<CommandResponse> resCreateTrn = await _transactionAPI.CreateTransactionAsync(createTransactionCommand);
            if (!resCreateTrn.result)
            {
                return Json(new { result = false, msg = resCreateTrn.error.error.message });
            }
            #endregion

            HttpContext.Session.Remove(_sessionTempSellingItemBarcodeMobileName);
            return Json(new { result = true, msg = "บันทึกข้อมูลสำเร็จ." });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, msg = $"ขออภัย รูปแบบข้อมูลไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!. {ex.Message}" });
        }
    }
    #endregion

    #region Func for Scanner ,Mobile
    private async Task<BaseResponse<List<GetItemListResponseDTO>>> GetItemSessionDataAsync()
    {
        BaseResponse<List<GetItemListResponseDTO>> res = HttpContext.Session.GetDataFromSession<BaseResponse<List<GetItemListResponseDTO>>>(_sessionTempSaleItemData);
        if (res != null)
        {
            return res;
        }
        res = await _itemAPI.GetItemListAsync();
        HttpContext.Session.SetDataToSession(_sessionTempSaleItemData, res);
        return res;
    }

    private void MappingSellingBarcodeItem(ref SellingBarcodeItemViewModel sellingBarcodeItemView)
    {
        BaseResponse<List<GetItemListResponseDTO>> resItems = HttpContext.Session.GetDataFromSession<BaseResponse<List<GetItemListResponseDTO>>>(_sessionTempSaleItemData);
        string itemBarcode = sellingBarcodeItemView.barcode;

        GetItemListResponseDTO existItem = resItems.data.Where(w => !string.IsNullOrEmpty(w.barcode)).FirstOrDefault(w => w.barcode.Trim().ToUpper() == itemBarcode.Trim().ToUpper());
        if (existItem == null)
        {
            throw new Exception("ไม่พบข้อมูลบาร์โค้ดสินค้า!");
        }
        sellingBarcodeItemView.itemid = existItem != null ? existItem.itemid : 0;
        sellingBarcodeItemView.itemname = existItem != null ? existItem.name : null;
        sellingBarcodeItemView.itemprice = existItem != null ? existItem.price : 0;
        sellingBarcodeItemView.qty = sellingBarcodeItemView.qty > 0 ? sellingBarcodeItemView.qty : 1;
    }

    private List<SelectListItem> PrepareSelectSellingType()
    {
        //BaseResponse<List<GetTrasnactionByCriteriaResponseDTO>> resItemTransaferTypeList = await _transactionTypeAPI.GetTransactionTypeByCriteriaAsync(new GetTrasnactionByCriteriaQuery
        //{
        //    isactive
        //});
        List<SelectListItem> SellingTypeList = new List<SelectListItem>();
        SellingTypeList.Add(new SelectListItem { Text = "เงินสด", Value = "1" });
        SellingTypeList.Add(new SelectListItem { Text = "เงินโอน", Value = "2" });
        return SellingTypeList;
    }
    #endregion

    #region Sale Index

    /// <summary>
    /// First page of employee and set all query data from firstday to end of month
    /// </summary>
    /// <param name="searchItem"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SearchSaleTransaction([FromBody] SearchTransactionViewModel searchItem)
    {
        try
        {
            #region Prepare Search Start & End Date
            DateTime sDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime eDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            if (!string.IsNullOrEmpty(searchItem.startdate))
            {
                string[] sTransferDate = searchItem.startdate.Split("-");
                if (sTransferDate.Count() != 3)
                {
                    throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
                }
                sDate = new DateTime(sTransferDate[2].ToInt32(), sTransferDate[1].ToInt32(), sTransferDate[0].ToInt32());
            }

            if (!string.IsNullOrEmpty(searchItem.enddate))
            {
                string[] sTransferEndDate = searchItem.enddate.Split("-");
                if (sTransferEndDate.Count() != 3)
                {
                    throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
                }
                eDate = new DateTime(sTransferEndDate[2].ToInt32(), sTransferEndDate[1].ToInt32(), sTransferEndDate[0].ToInt32());
            }

            //เช็ควันที่สิ้นสุดน้อยกว่า วันเริ่มต้น
            if (DateTime.Compare(sDate, eDate) == 1)
            {
                throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
            }
            #endregion

            BaseResponse<GetTransactionByBranchIDV2ReseponseDTO> resSaleTransactions = await _transactionAPI.GetTransactionByBranchIDV2Async(new GetTransactionByBranchIDV2Query
            {
                transaction_startdate = sDate,
                transaction_enddate = eDate,
                branchid = base.UserProfile.access_branch.FirstOrDefault().branchid,
                startrow = searchItem.start,
                pagesize = searchItem.length,
                //searchvalue = searchItem.searchValue.Replace("\t", "").Replace("\n", ""),
                isexportalldata = searchItem.isexportalldata,
            });

            if (!resSaleTransactions.result)
            {
                return Json(new { data = new List<GetTransactionByBranchIDResponseDTO>(), recordsTotal = 0, recordsFiltered = 0 });
            }

            #region Search Filter
            if (!string.IsNullOrEmpty(searchItem.searchValue))
            {
                string searchValue = searchItem.searchValue.Replace("\t", "").Replace("\n", "");
                resSaleTransactions.data.transactiondata = resSaleTransactions.data.transactiondata.Where(w => w.branchname.Contains(searchValue)
                || w.transactiontypedesc.Contains(searchValue)
                || w.totalamount.CompareTo(searchValue.ToDecimal()) == 0
                || w.amounttransfer.CompareTo(searchValue.ToDecimal()) == 0
                || w.amountdeposit.CompareTo(searchValue.ToDecimal()) == 0
                || w.depositfee.CompareTo(searchValue.ToDecimal()) == 0
                || w.createdby.Contains(searchValue)).ToList();
            }
            #endregion

            //var totalRows = resReport.data.totalrow;
            var totalItems = resSaleTransactions.data.totalrow; // Get total item count for pagination

            // Filter based on searchValue if necessary
            var query = resSaleTransactions.data.transactiondata;

            // Calculate paginated data
            //var items = searchItem.isexportalldata ? query : query.Skip(searchItem.start).Take(searchItem.length).ToList();

            // Prepare response for DataTables
            return Json(new
            {
                draw = searchItem.draw, // Echo the draw parameter
                recordsTotal = totalItems, // Total records before filtering
                recordsFiltered = totalItems, // Total records after applying filtering
                data = resSaleTransactions.data.transactiondata // The actual data to be displayed
            });
        }
        catch
        {
            return Json(new { data = new List<GetTransactionByBranchIDResponseDTO>(), recordsTotal = 0, recordsFiltered = 0 });
        }
    }

    #endregion

}