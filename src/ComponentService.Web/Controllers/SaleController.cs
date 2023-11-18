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
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandList.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Commands.DeleteItemInBranch.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Commands.UpdateItemInBranch.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchList.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByCriteria.v1;
using CYRetailIMS.Application.Services.ItemService.Commands.DeleteItem;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeList.v1;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;
using CYRetailIMS.Application.Services.UnitOfMeasureService.Queries.GetUnitOfMeasureList.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using CYRetailIMS.ComponentService.Web.Models;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.Application.Common.Models.EnumModel;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Sale, RoleName.AreaSale)]
public class SaleController : BaseController
{
    private readonly IItemInBranchAPI _itemInBranchAPI;
    private readonly IItemAPI _itemAPI;
    private readonly ITransactionAPI _transactionAPI;
    private readonly IItemBrandAPI _itemBrandAPI;
    private readonly IItemTypeAPI _itemTypeAPI;
    private readonly IItemUnitOfMeasureAPI _itemUnitOfMeasureAPI;

    public SaleController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IItemInBranchAPI itemInBranchAPI,
        IItemAPI itemAPI,
        ITransactionAPI transactionAPI,
        IItemBrandAPI itemBrandAPI,
        IItemTypeAPI itemTypeAPI,
        IItemUnitOfMeasureAPI itemUnitOfMeasureAPI) : base(httpClientRequest, mapper, log)
    {
        _itemInBranchAPI = itemInBranchAPI;
        _itemAPI = itemAPI;
        _transactionAPI = transactionAPI;
        _itemBrandAPI = itemBrandAPI;
        _itemTypeAPI = itemTypeAPI;
        _itemUnitOfMeasureAPI = itemUnitOfMeasureAPI;
    }

    public async Task<IActionResult> Index()
    {
        BaseResponse<GetItemInBranchByBranchIDResponseDTO> resItemBranch = await _itemInBranchAPI.GetItemInBranchByBranchIDAsync(base.UserProfile.access_branch.FirstOrDefault().branchid);

        BaseResponse<List<GetTransactionByBranchIDResponseDTO>> resTransaction = await _transactionAPI.GetTransactionByBranchIDAsync(base.UserProfile.access_branch.FirstOrDefault().branchid);

        ViewBag.BranchList = base.UserProfile.access_branch;
        ViewBag.ItemBranch = resItemBranch;
        ViewBag.TransactionList = resTransaction;
        return View();
    }

    public async Task<IActionResult> Create()
    {
        BaseResponse<GetItemInBranchByBranchIDResponseDTO> resItemInBranch = await _itemInBranchAPI.GetItemInBranchByBranchIDAsync(UserProfile.access_branch.FirstOrDefault().branchid);
        ViewBag.ItemInBranch = resItemInBranch;
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
            List<Select2Model> searchItemList = resItemInBranch.data.itemlist.Where(w => w.itemcode.ToLower().StartsWith(search.ToLower())
            || w.itemname.ToLower().StartsWith(search.ToLower())).Select(s => new Select2Model
            {
                id = s.itemid.ToString(),
                text = s.itemname
            }).ToList();
            return Json(new { items = searchItemList });
        }
        catch (Exception ex)
        {
            return Json(new { items = new List<Select2Model>(), message = $"พบข้อผิดพลาด {ex.Message}" });
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

        //BaseResponse<GetItemListResponseDTO> resItem = await _itemAPI.GetItemByIdAsync(itemid);
        //EditItemViewModel viewModel = EditItemMapping(resItem.data);

        //Get Master Data
        BaseResponse<List<GetItemTypeListResponseDTO>> resItemTypeList = await _itemTypeAPI.GetItemTypeListAsync();
        BaseResponse<List<GetItemBrandListResponseDTO>> resItemBrandList = await _itemBrandAPI.GetItemBrandListAsync();
        BaseResponse<List<GetUnitOfMeasureListResponseDTO>> resUnitOfMeasureList = await _itemUnitOfMeasureAPI.GetUnitOfMeasureListAsync();

        ViewBag.ItemTypeList = resItemTypeList;
        ViewBag.ItemBrandList = resItemBrandList;
        ViewBag.ItemUOMList = resUnitOfMeasureList;
        return View(viewModel);
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
    private CreateTransactionCommand PrepareCreateTransactionCommand(SellingItemViewModel reqObj, List<CreateTransactionDetailCommand> createTransactionDetailCommands)
    {
        decimal toalAmt = createTransactionDetailCommands.Select(s => decimal.Multiply(s.price, s.qty)).Sum();
        return new CreateTransactionCommand
        {
            transactiontypeid = 1, //Retail
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
            transactiondetail = createTransactionDetailCommands
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
}

public class Select2Model
{
    public string id { get; set; }
    public string text { get; set; }
}