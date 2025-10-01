using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
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
using CYRetailIMS.Application.ExternalService.ReceiptAPI;
using CYRetailIMS.Application.ExternalService.ReceiveTempAPI;
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
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.CreateReceipt.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.GenerateReceiptNo.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempByBranchID.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempList.v1;
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
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NetTopologySuite.Index.HPRtree;
using static CYRetailIMS.Application.Common.Models.EnumModel;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Sale, RoleName.AreaSale)]
public class SaleController : BaseController
{
    private string _sessionTempSellingItemBarcodeScannerName => "TEMP_SELLING_ITEM_BARCODE_DATA";
    private string _sessionTempSellingItemBarcodeMobileName => "TEMP_SELLING_ITEM_BARCODE_MOBILE_DATA";
    private string _sessionTempSaleItemData => "SALE_ITEM_DATA";

    private string _sessionReceiptNo => "SALE_RECEIPT_NO";

    private readonly IItemInBranchAPI _itemInBranchAPI;
    private readonly IItemAPI _itemAPI;
    private readonly ITransactionAPI _transactionAPI;
    private readonly IItemBrandAPI _itemBrandAPI;
    private readonly IItemTypeAPI _itemTypeAPI;
    private readonly IItemUnitOfMeasureAPI _itemUnitOfMeasureAPI;
    private readonly ITransactionTypeAPI _transactionTypeAPI;
    private readonly ICompositeViewEngine _viewEngine;
    private readonly IReceiveTempAPI _receiveTempAPI;
    private readonly IReceiptAPI _receiptAPI;

    public SaleController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IItemInBranchAPI itemInBranchAPI,
        IItemAPI itemAPI,
        ITransactionAPI transactionAPI,
        IItemBrandAPI itemBrandAPI,
        IItemTypeAPI itemTypeAPI,
        IItemUnitOfMeasureAPI itemUnitOfMeasureAPI,
        ITransactionTypeAPI transactionTypeAPI, 
        ICompositeViewEngine viewEngine,
        IReceiveTempAPI receiveTempAPI,
        IReceiptAPI receiptAPI) : base(httpClientRequest, mapper, log)
    {
        _itemInBranchAPI = itemInBranchAPI;
        _itemAPI = itemAPI;
        _transactionAPI = transactionAPI;
        _itemBrandAPI = itemBrandAPI;
        _itemTypeAPI = itemTypeAPI;
        _itemUnitOfMeasureAPI = itemUnitOfMeasureAPI;
        _transactionTypeAPI = transactionTypeAPI;
        _viewEngine = viewEngine;
        _receiveTempAPI = receiveTempAPI;
        _receiptAPI = receiptAPI;
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

    public async Task<IActionResult> Scanner()
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
            return Json(new { result = true, message = "สำเร็จ", data = new List<SaleSummaryReportResponseDTO>() });
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
                    tempSellingBarcodeItemList.Where(w => w.barcode == sellingBarcodeItem.barcode).ToList().ForEach(e =>
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
            if (tempSellingBarcodeItemList == null || tempSellingBarcodeItemList.Count == 0)
            {
                return Json(new { result = false, msg = $"ขออภัย ไม่พบรายการสินค้า" });
            }
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

            #region Prepare & Create Receipt
            string receiptNo = HttpContext.Session.GetDataFromSession<string>(_sessionReceiptNo);
            CreateReceiptCommand createReceiptCommand = new CreateReceiptCommand
            {
                transactionid = resCreateTrn.data.transactionid,
                createdby = base.UserProfile.username,
                receiptno = receiptNo
            };
            var resCreateReceipt = await _receiptAPI.CreateReceiptAsync(createReceiptCommand);
            HttpContext.Session.Remove(_sessionReceiptNo);
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
            if (tempSellingBarcodeItemList != null)
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
                    tempSellingBarcodeItemList.Where(w => w.barcode == sellingBarcodeItem.barcode).ToList().ForEach(e =>
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

    #region POS

    /// <summary>
    /// Method to generate receipt text for printing
    /// </summary>
    /// <param name="sellingItemView"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> GenerateReceiveSlipText([FromBody] SellingItemViewModel sellingItemView)
    {
        int branchID = sellingItemView.branch.ToInt32();
        var tempSellingBarcodeItemList = HttpContext.Session.GetDataFromSession<List<SellingBarcodeItemViewModel>>(_sessionTempSellingItemBarcodeScannerName);
        if (tempSellingBarcodeItemList == null || tempSellingBarcodeItemList.Count == 0)
        {
            return Json(new { result = false, msg = "ขออภัย ไม่พบรายการสินค้า" });
        }

        BaseResponse<GetReceiveTempResponseDTO> getReceiveTemplate = await _receiveTempAPI.GetReceiveTemplatehByBranchIDAsync(new GetReceiveTempByBranchIDQuery
        {
            branchid = branchID
        });
        if (!getReceiveTemplate.result)
        {
            return Json(new { result = false, msg = "ขออภัย ไม่พบแม่แบบใบเสร็จรับเงิน" });
        }

        #region Generate Receipt Number by branch code
        string currentReceiptNo = HttpContext.Session.GetDataFromSession<string>(_sessionReceiptNo);
        if (string.IsNullOrEmpty(currentReceiptNo))
        {
            var resGenReceiptNo = await _receiptAPI.GenerateReceiptNoByBranchAsync(new GenerateReceiptNoCommand
            {
                branchcode = getReceiveTemplate.data.branchcode,
            });
            if (!resGenReceiptNo.result)
            {
                return Json(new { result = false, msg = "ขออภัย ไม่สามารถสร้างเลขที่ใบเสร็จได้." });
            }
            HttpContext.Session.SetDataToSession(_sessionReceiptNo, resGenReceiptNo.data.receiptno);
            currentReceiptNo = resGenReceiptNo.data.receiptno;
        }
        #endregion

        // คำนวณ
        decimal subTotal = tempSellingBarcodeItemList.Sum(s => s.totalprice);
        decimal discount = 0.0m;
        decimal shipping = 0.0m;
        decimal vat = subTotal.ToExVat();
        //decimal totalBill = subTotal - discount + shipping + vat;
        //inc vat VAT รวมในสินค้าแล้ว ไม่ต้องตำนวนบวกเพิ่ม
        decimal totalBill = subTotal - discount + shipping;
        decimal due = 0.0m;

        var model = new ReceiptViewModel
        {
            InvoiceNo = currentReceiptNo,
            CompanyName = getReceiveTemplate.data.shopheadernametext,
            CompanyAddress = getReceiveTemplate.data.shopheaderaddresstext,
            AdditionalHeaderText = getReceiveTemplate.data.additionalheadertext,
            TelephoneNo = getReceiveTemplate.data.telephoneno,
            ShopFooterText = getReceiveTemplate.data.shopfootertext,
            AdditionalFooterText = getReceiveTemplate.data.additionalfootertext,
            Items = tempSellingBarcodeItemList,
            SubTotal = subTotal,
            Discount = discount,
            Shipping = shipping,
            Vat = vat,
            TotalBill = totalBill,
            Due = due,
            Date = DateTime.Now
        };

        // Text Mode
        string recetiveText = !string.IsNullOrEmpty(sellingItemView.version) && sellingItemView.version.Equals("2") ? GenerateReceiptEscPosV2(model) : GenerateReceiptEscPos(model);

        // Convert to Base64 for JS transfer
        byte[] bytes = Encoding.GetEncoding(874).GetBytes(recetiveText);
        string recetiveBase64 = Convert.ToBase64String(bytes);

        string printerName = getReceiveTemplate.data.printername;

        // return PartialView เป็น string
        string html = RenderPartialViewToString("_ReceiptModal", model);
        return Json(new { result = true, msg = html, cmds = recetiveBase64, text = recetiveText, printername = printerName });
    }

    /// <summary>
    /// Method to render a partial view to string Modal ReceiptModal
    /// </summary>
    /// <param name="viewName"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    private string RenderPartialViewToString(string viewName, object model)
    {
        ViewData.Model = model;
        using var sw = new StringWriter();
        var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);
        var viewContext = new ViewContext(
            ControllerContext,
            viewResult.View,
            ViewData,
            TempData,
            sw,
            new HtmlHelperOptions()
        );
        viewResult.View.RenderAsync(viewContext).Wait();
        return sw.ToString();
    }

    /// <summary>
    /// Method to generate ESC/POS receipt text for printing [Version 1]
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    private string GenerateReceiptEscPos(ReceiptViewModel model)
    {
        var sb = new StringBuilder();

        // ESC/POS Init
        sb.Append("\x1B\x40"); // Initialize

        // Reduce top margin
        //sb.Insert(0, "\x1B\x4A\x00"); // optional

        // ==== Header ====
        sb.AppendLine();
        sb.Append("\x1B\x61\x01"); // Center align
        sb.AppendLine(model.CompanyName);
        sb.AppendLine(model.CompanyAddress);
        if (!string.IsNullOrEmpty(model.TelephoneNo))
            sb.AppendLine(model.TelephoneNo);
        sb.AppendLine(new string('-', 48));

        // Invoice & Date
        sb.Append("\x1B\x61\x00"); // Left align
        sb.AppendLine($"เลขที่ใบเสร็จ: {model.InvoiceNo}");
        sb.AppendLine($"วันที่: {model.Date:dd/MM/yyyy HH:mm}");
        sb.AppendLine(new string('-', 48));

        // ==== Items ====
        sb.AppendLine($"{"# Item",-24}{"Price",5}{"Qty",9}{"Total",10}");
        sb.AppendLine(new string('-', 48));
        foreach (var item in model.Items)
        {

            // Item line with matching column widths
            //string name = PadItemNameEscPos(item.itemname, 24);
            //string line = $"{name}{item.qty,5}{item.itemprice,9:N2}{item.totalprice,10:N2}";
            //sb.AppendLine(line);

            string name = TruncateItemNameEscPos(item.itemname, 24);
            sb.AppendLine($"{name,-24}{item.qty,5}{item.itemprice,9:N2}{item.totalprice,10:N2}");
        }
        sb.AppendLine(new string('-', 48));

        // ==== Totals ====
        sb.AppendLine($"{"ราคารวม :",-32}{model.SubTotal,16:N2}");
        sb.AppendLine($"{"ส่วนลด :",-32}{model.Discount,16:N2}");
        //sb.AppendLine($"{"Shipping",-32}{model.Shipping,16:N2}");
        sb.AppendLine($"{"VAT(7%) :",-32}{model.Vat,16:N2}");
        sb.AppendLine($"{"ยอดรวมทั้งหมด :",-32}{model.TotalBill,16:N2}");
        sb.AppendLine(new string('-', 48));

        // ==== Footer ====
        sb.Append("\x1B\x61\x01"); // Center align
        if (!string.IsNullOrEmpty(model.ShopFooterText))
            sb.AppendLine(model.ShopFooterText);
        if (!string.IsNullOrEmpty(model.AdditionalFooterText))
            sb.AppendLine(model.AdditionalFooterText);
        sb.AppendLine("THANK YOU!");
        sb.AppendLine();

        // Cut paper
        sb.Append("\x1D\x56\x41"); // Partial cut
        return sb.ToString();
    }

    /// <summary>
    /// Method to truncate item name for ESC/POS printing
    /// </summary>
    /// <param name="name"></param>
    /// <param name="maxLength"></param>
    /// <returns></returns>
    private string TruncateItemNameEscPos(string name, int maxLength = 24)
    {
        if (string.IsNullOrEmpty(name)) return "";
        if (name.Length <= maxLength) return name;
        return name.Substring(0, maxLength - 3) + "...";
    }


    /// <summary>
    /// Method to generate ESC/POS receipt text for printing [Version 2]
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    private string GenerateReceiptEscPosV2(ReceiptViewModel model)
    {
        var sb = new StringBuilder();
        var enc = Encoding.GetEncoding("TIS-620"); // TIS-620 Thai encoding for ESC/POS

        //// ===== Utility: Pad with byte awareness =====
        //string PadRightEscPos(string text, int width) => PadEscPos(text, width, enc, right: false);
        //string PadLeftEscPos(string text, int width) => PadEscPos(text, width, enc, right: true);

        // ===== ESC/POS Init =====
        //sb.Append("\x1B\x40"); // Initialize
        sb.Append("\x1B\x4D\x00"); // Initialize Font A (default)

        // ✅ เลือก Code Page ไทย (กรณีเครื่อง map CP874 ไว้ที่ 18)
        //sb.Append("\x1B\x74\x12");

        // ✅ เลือก Code Page ไทย (กรณีเครื่อง map CP874 ไว้ที่ 255) not working BUG? use .setCharacterCodeTable(255) in js script
        //sb.Append("\x1B\x74\xFF");
        //sb.Append("\x1B\x74\xFF");

        //sb.Append("\x1B\x4D\x01"); // Initialize Font B
        sb.AppendLine();
        sb.Append("\x1B\x61\x01"); // Center align

        // ===== Header =====
        sb.AppendLine(model.CompanyName);
        sb.AppendLine(model.CompanyAddress);
        if (!string.IsNullOrEmpty(model.TelephoneNo))
            sb.AppendLine(model.TelephoneNo);
        sb.AppendLine(new string('-', 48));

        // ===== Invoice & Date =====
        sb.Append("\x1B\x61\x00"); // Left align
        sb.AppendLine($"Invoice: {model.InvoiceNo}");
        sb.AppendLine($"Date: {model.Date:dd/MM/yyyy HH:mm}");
        sb.AppendLine(new string('-', 48));

        // ===== Items Header =====
        sb.AppendLine($"{PadRightEscPos("Item", 24)}{PadLeftEscPos("Qty", 5)}{PadLeftEscPos("Price", 9)}{PadLeftEscPos("Total", 10)}");
        sb.AppendLine(new string('-', 48));

        // ===== Items =====
        foreach (var item in model.Items)
        {
            string stripItemName = PadRightEscPos(item.itemname, 24);
            if (!IsAscii(stripItemName))
            {
                //string name = item.itemname.Length > 24 ? PadItemNameRightEscPos(item.itemname.Substring(0, 24), 27) : PadItemNameRightEscPos(item.itemname, 27);
                //string name = LimitByByteLength(item.itemname, 24, Encoding.GetEncoding(874));
                //string name = PadRightEscPos(item.itemname, 24);
                //name = name + "..";
                string name = PadItemNameRightEscPos(item.itemname, 27);
                //name = PadTextEscPos(name, 27, Encoding.GetEncoding(874), false);
                int targetSize = 62;
                byte[] utf8Bytes = Encoding.UTF8.GetBytes(name);
                int padBytes = utf8Bytes.Length - targetSize;
                if (padBytes > 0)
                {
                    name = name.Remove(name.Length - 1);
                    //name = name + new string(' ', padBytes);
                    //for (int i = 1; i <= padBytes; i++)
                    //{
                    //    name = name + ".";
                    //}
                }
                byte[] paddedBytes = Encoding.UTF8.GetBytes(name);

                string qty = PadLeftEscPos(item.qty.ToString(), 5);
                string price = PadLeftEscPos(item.itemprice.ToString("N2"), 9);
                string total = PadLeftEscPos(item.totalprice.ToString("N2"), 10);

                // ข้อความ (encode เป็น CP874 ก่อน)
                //sb.Append(Encoding.GetEncoding(874).GetString(Encoding.GetEncoding(874).GetBytes($"{name}{qty}{price}{total}")));
                sb.AppendLine($"{Encoding.GetEncoding(874).GetString(Encoding.GetEncoding(874).GetBytes(name))}{qty}{price}{total}");
            }
            else
            {
                string name = PadRightEscPos(item.itemname, 24);
                byte[] finalByte = Encoding.GetEncoding(874).GetBytes(name);
                string qty = PadLeftEscPos(item.qty.ToString(), 5);
                string price = PadLeftEscPos(item.itemprice.ToString("N2"), 9);
                string total = PadLeftEscPos(item.totalprice.ToString("N2"), 10);
                sb.AppendLine($"{name}{qty}{price}{total}");
            }
        }
        sb.AppendLine(new string('-', 48));

        // ===== Totals =====
        //sb.AppendLine($"{PadRightEscPos("SubTotal", 32)}{PadLeftEscPos(model.SubTotal.ToString("N2"), 16)}");
        //sb.AppendLine($"{PadRightEscPos("Discount", 32)}{PadLeftEscPos(model.Discount.ToString("N2"), 16)}");
        ////sb.AppendLine($"{PadRightEscPos("Shipping", 32)}{PadLeftEscPos(model.Shipping.ToString("N2"), 16)}");
        //sb.AppendLine($"{PadRightEscPos("VAT", 32)}{PadLeftEscPos(model.Vat.ToString("N2"), 16)}");
        //sb.AppendLine($"{PadRightEscPos("Total", 32)}{PadLeftEscPos(model.TotalBill.ToString("N2"), 16)}");

        sb.AppendLine($"{PadTextRightEscPos("ราคารวม :", 40-9)}{PadTextLeftEscPos(model.SubTotal.ToString("N2"), (48 - (40 + model.SubTotal.ToString("N2").Length)))}");
        //sb.AppendLine($"{PadTextRightEscPos("ส่วนลด :", 40-8)}{PadTextLeftEscPos(model.Discount.ToString("N2"), (48 - (40 + model.Discount.ToString("N2").Length)) + 1)}");
        sb.AppendLine($"{PadTextRightEscPos("VAT(7%) :", 40-9)}{PadTextLeftEscPos(model.Vat.ToString("N2"), (48 - (40 + model.Vat.ToString("N2").Length)))}");
        sb.AppendLine($"{PadTextRightEscPos("ยอดรวมทั้งหมด :", 40-15)}{PadTextLeftEscPos(model.TotalBill.ToString("N2"), (48 - (40 + model.TotalBill.ToString("N2").Length)) + 2)}");


        //sb.AppendLine($"{PadRightEscPos("ราคารวม :", GetDisplayWidth("ราคารวม :"))}{PadLeftEscPos(model.SubTotal.ToString("N2"), model.SubTotal.ToString("N2").Length)}");
        //sb.AppendLine($"{PadRightEscPos("ส่วนลด :", GetDisplayWidth("ส่วนลด :"))}{PadLeftEscPos(model.Discount.ToString("N2"), model.Discount.ToString("N2").Length)}");
        //sb.AppendLine($"{PadRightEscPos("VAT(7%) :", GetDisplayWidth("VAT(7%) :"))}{PadLeftEscPos(model.Vat.ToString("N2"), model.Vat.ToString("N2").Length)}");
        //sb.AppendLine($"{PadRightEscPos("ยอดรวมทั้งหมด :", GetDisplayWidth("ยอดรวมทั้งหมด :"))}{PadLeftEscPos(model.TotalBill.ToString("N2"), model.TotalBill.ToString("N2").Length)}");

        
        sb.AppendLine(new string('-', 48));

        // ===== Footer =====
        sb.Append("\x1B\x61\x01"); // Center align
        if (!string.IsNullOrEmpty(model.ShopFooterText))
            sb.AppendLine(model.ShopFooterText);
        if (!string.IsNullOrEmpty(model.AdditionalFooterText))
            sb.AppendLine(model.AdditionalFooterText);
        sb.AppendLine("THANK YOU!");
        sb.AppendLine();

        // Cut paper
        sb.Append("\x1D\x56\x41"); // Partial cut
        return sb.ToString();
    }

    /// <summary>
    /// Method to pad text for ESC/POS printing with byte-length awareness
    /// </summary>
    /// <param name="text"></param>
    /// <param name="width"></param>
    /// <param name="encoding"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    private string PadEscPos(string text, int width, Encoding encoding, bool right)
    {
        if (string.IsNullOrEmpty(text)) text = "";

        int textWidth = GetDisplayWidth(text);
        if (textWidth > width)
        {
            // ตัดข้อความโดยไม่เกินความกว้าง
            int currentWidth = 0;
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                int charWidth = (c < 128) ? 1 : 2;
                if (currentWidth + charWidth > width)
                    break;
                sb.Append(c);
                currentWidth += charWidth;
            }
            text = sb.ToString();
            textWidth = currentWidth;
        }

        int spaces = width - textWidth;
        return right ? new string(' ', spaces) + text : text + new string(' ', spaces);
    }

    private string PadTextEscPos(string text, int width, Encoding encoding, bool right)
    {
        int spaces = width - text.Length;
        return right ? new string(' ', width) + text : text + new string(' ', width);
    }

    private string PadItemTextEscPos(string text, int width, Encoding encoding, bool right)
    {
        var dsd = Encoding.UTF8.GetBytes(text);
        int spaces = width - text.Length;
        if (spaces == 0)
        {
            return text;
        }
        if (spaces < 0)
        {
            return text.Substring(0, width);
        }
        return right ? new string(' ', spaces) + text : text + new string(' ', spaces);
    }

    private string PadRightEscPos(string text, int width) => PadEscPos(text, width, Encoding.GetEncoding("TIS-620"), right: false);

    private string PadLeftEscPos(string text, int width) => PadEscPos(text, width, Encoding.GetEncoding("TIS-620"), right: true);

    private string PadItemNameRightEscPos(string text, int width) => PadItemTextEscPos(text, width, Encoding.GetEncoding("TIS-620"), right: false);

    private string PadTextRightEscPos(string text, int width) => PadTextEscPos(text, width, Encoding.GetEncoding("TIS-620"), right: false);

    private string PadTextLeftEscPos(string text, int width) => PadTextEscPos(text, width, Encoding.GetEncoding("TIS-620"), right: true);

    private int GetDisplayWidth(string text)
    {
        //int width = 0;
        //foreach (char c in text)
        //{
        //    // ASCII → กว้าง 1
        //    if (c < 128)
        //        width += 1;
        //    else
        //        width += 2; // ไทย/ยูนิโค้ด ส่วนใหญ่กิน 2 ช่อง
        //}
        int width = 0;

        foreach (char c in text)
        {
            width += (c < 128) ? 1 : 2;
        }

        return width;
    }

    private bool IsAscii(string text)
    {
        foreach (char c in text)
        {
            if (c > 127) return false; // not ASCII
        }
        return true;
    }

    private string PadItemNameEscPos(string text, int columnWidth, Encoding encoding)
    {
        // Convert to printer bytes
        byte[] bytes = encoding.GetBytes(text);

        if (bytes.Length > columnWidth)
        {
            // truncate properly by bytes, not chars
            int cutIndex = columnWidth;
            while (cutIndex > 0 && (bytes[cutIndex] & 0xC0) == 0x80) // avoid cutting inside multibyte
            {
                cutIndex--;
            }
            return encoding.GetString(bytes, 0, cutIndex);
        }

        // Pad with spaces to reach columnWidth
        var res = text + new string(' ', columnWidth - bytes.Length);
        return res;
    }

    private string LimitByByteLength(string text, int maxBytes, Encoding encoding)
    {
        if (string.IsNullOrEmpty(text)) return "";

        byte[] bytes = encoding.GetBytes(text);
        if (bytes.Length <= maxBytes) return text;

        // ตัดให้ไม่ขาดกลางตัวอักษร
        int cut = maxBytes;
        while (cut > 0)
        {
            try
            {
                string sub = encoding.GetString(bytes, 0, cut);
                if (encoding.GetByteCount(sub) <= maxBytes)
                    return sub;
            }
            catch { }
            cut--;
        }
        return "";
    }

    #endregion


}