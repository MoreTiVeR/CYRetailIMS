using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.AdjustItemAPI;
using CYRetailIMS.Application.ExternalService.AdjustItemTypeAPI;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.ExternalService.ItemAPI;
using CYRetailIMS.Application.ExternalService.ItemInBranchAPI;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.CreateAdjustItem.v1;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Queries.GetAdjustItemTransactions.v1;
using CYRetailIMS.Application.Services.AdjustItemTypeService.Queries.GetAdjustItemType.v1;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchList.v1;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using CYRetailIMS.Infrastructure.ExternalService.ItemAPI;
using CYRetailIMS.Infrastructure.ExternalService.ItemInBranchAPI;
using Microsoft.AspNetCore.Mvc;
using CYRetailIMS.Infrastructure.Common.Extensions;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using Microsoft.AspNetCore.Mvc.Rendering;
using NUglify.Helpers;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Queries.GetAdjustItemTransactionByID.v1;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Stock)]
public class AdjustItemController : BaseController
{
    private readonly IAdjustItemAPI _adjustItemAPI;
    private readonly IAdjustItemTypeAPI _adjustItemTypeAPI;
    private readonly IItemAPI _itemAPI;
    private readonly IItemInBranchAPI _itemInBranchAPI;
    private readonly IBranchAPI _branchAPI;

    public AdjustItemController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IAdjustItemAPI adjustItemAPI,
        IAdjustItemTypeAPI adjustItemTypeAPI,
        IItemAPI itemAPI,
        IItemInBranchAPI itemInBranchAPI,
        IBranchAPI branchAPI) : base(httpClientRequest, mapper, log)
    {
        _adjustItemAPI = adjustItemAPI;
        _adjustItemTypeAPI = adjustItemTypeAPI;
        _itemAPI = itemAPI;
        _itemInBranchAPI = itemInBranchAPI;
        _branchAPI = branchAPI;
    }

    public async Task<IActionResult> Index()
    {
        BaseResponse<List<GetBranchResponseDTO>> resBranchList = await _branchAPI.GetBranchListAsync();
        ViewBag.BranchList = resBranchList;
        return View();
    }

    public async Task<IActionResult> Adjust()
    {
        #region Get- Set Item, AdjustType, Branch to Session
        BaseResponse<List<GetAdjustItemTypeResposeDTO>> resAdjustType = await GetAdjustItemTypeSessionDataAsync();
        BaseResponse<List<GetItemListResponseDTO>> resItem = await GetItemSessionDataAsync();
        BaseResponse<List<GetBranchResponseDTO>> resBranch = await GetBranchSessionDataAsync();
        #endregion

        ViewBag.AdjustItemType = resAdjustType;
        ViewBag.ItemList = resItem;
        ViewBag.BranchList = resBranch;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAdjustItem([FromBody] CreateAdjustItemViewModel adjustItemData)
    {
        try
        {
            //if (adjustItemData.Qty < 0)
            //{
            //    throw new Exception("กรุณาระบุจำนวนไม่น้อยกว่า 0");
            //}
            CreateAdjustItemCommand CreateAdjustItemCommand = MappingCreateAdjustItemCommand(adjustItemData);
            BaseResponse<CommandResponse> res = await _adjustItemAPI.CreateAdjustItemAsync(CreateAdjustItemCommand);

            if (res.result)
            {
                //Clear TEMP_ADJUST_ITEM_DATA
                HttpContext.Session.Remove("TEMP_ADJUST_ITEM_DATA");
            }
            return Json(new { result = res.result, message = res.result ? "ปรับสต๊อกสินค้าสำเร็จ" : $"ไม่สามารถทำรายการได้, {res.error.error.message}" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public IActionResult AddTempItem([FromBody] AdjustItemViewModel adjustItemData)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            if (adjustItemData.nqty < 0)
            {
                throw new Exception("กรุณาระบุจำนวนไม่น้อยกว่า 0");
            }

            //Get Current List
            List<AdjustItemViewModel> tempAdjustItemList = HttpContext.Session.GetDataFromSession<List<AdjustItemViewModel>>("TEMP_ADJUST_ITEM_DATA");

            #region Update when Already added
            var existData = tempAdjustItemList.FirstOrDefault(w => w.nbranchid == adjustItemData.nbranchid
            && w.nitemid == adjustItemData.nitemid
            && w.nadjusttypeid == adjustItemData.nadjusttypeid);
            if (existData != null)
            {
                //tempAdjustItemList = tempAdjustItemList.Where(w => w.nbranchid == adjustItemData.nbranchid && w.nitemid == adjustItemData.nitemid).Select(s =>
                //{
                //    s.ntqy = s.nseq + adjustItemData.ntqy;
                //    return s;
                //}).ToList();
                tempAdjustItemList.Where(w => w.nbranchid == adjustItemData.nbranchid
                && w.nitemid == adjustItemData.nitemid
                && w.nadjusttypeid == adjustItemData.nadjusttypeid).ForEach(e =>
                {
                    e.nqty = e.nqty + adjustItemData.nqty;
                });
            }
            else
            {
                //Add new
                int lastId = tempAdjustItemList != null && tempAdjustItemList.Count > 0 ? tempAdjustItemList.Last().nseq : 0;
                lastId++;
                adjustItemData.nseq = lastId;
                MappingAddAdjustItem(ref adjustItemData);
                tempAdjustItemList.Add(adjustItemData);
            }
            #endregion

            //int lastId = tempAdjustItemList != null && tempAdjustItemList.Count > 0 ? tempAdjustItemList.Last().nseq : 0;
            //lastId++;
            //adjustItemData.nseq = lastId;
            //MappingAddAdjustItem(ref adjustItemData);
            //tempAdjustItemList.Add(adjustItemData);
            HttpContext.Session.SetDataToSession("TEMP_ADJUST_ITEM_DATA", tempAdjustItemList);

            //test get
            //var res = HttpContext.Session.GetDataFromSession<List<AdjustItemViewModel>>("TEMP_ADJUST_ITEM_DATA");
            return Json(new { result = true, message = "เพิ่มข้อมูลปรับสต๊อกสำเร็จ" });

        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetTempItem()
    {
        try
        {
            List<AdjustItemViewModel> tempList = HttpContext.Session.GetDataFromSession<List<AdjustItemViewModel>>("TEMP_ADJUST_ITEM_DATA");

            #region if list is null => create new list with 0 member
            if (tempList == null)
            {
                tempList = new List<AdjustItemViewModel>();
                HttpContext.Session.SetDataToSession("TEMP_ADJUST_ITEM_DATA", tempList);
            }
            #endregion

            var resJson = Json(new { data = tempList.OrderBy(o => o.nseq).ToList() });
            var _resJson = resJson;

            return Json(new { data = tempList.OrderBy(o => o.nseq).ToList() });
        }
        catch
        {
            return Json(new { data = new List<AdjustItemViewModel>() });
        }

    }

    [HttpPost]
    public IActionResult EditTempItem([FromBody] AdjustItemViewModel adjustItemData)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            if (adjustItemData.nseq == 0)
            {
                //Get Current List
                List<AdjustItemViewModel> tempList = HttpContext.Session.GetDataFromSession<List<AdjustItemViewModel>>("TEMP_ADJUST_ITEM_DATA");

                int lastId = tempList != null && tempList.Count > 0 ? tempList.Last().nseq : 0;
                lastId++;
                adjustItemData.nseq = lastId;

                //Re-update object data
                MappingAddAdjustItem(ref adjustItemData);

                tempList.Add(adjustItemData);
                HttpContext.Session.SetDataToSession("TEMP_ADJUST_ITEM_DATA", tempList);

                //var res = HttpContext.Session.GetDataFromSession<List<AdjustItemViewModel>>("TEMP_ADJUST_ITEM_DATA");
                return Json(new { result = true, message = "Add new data success." });
            }
            else
            {
                List<AdjustItemViewModel> tempList = HttpContext.Session.GetDataFromSession<List<AdjustItemViewModel>>("TEMP_ADJUST_ITEM_DATA");
                AdjustItemViewModel data = tempList.FirstOrDefault(w => w.nseq == adjustItemData.nseq);

                //Mapping data
                MappingAdjustItem(ref data, adjustItemData);

                //Re-update object data
                MappingAddAdjustItem(ref adjustItemData);

                tempList.ToList().ForEach(ent =>
                {
                    if (ent.nseq == data.nseq)
                    {
                        ent = data;
                    }
                });

                HttpContext.Session.SetDataToSession("TEMP_ADJUST_ITEM_DATA", tempList);
                //var resUpdated = HttpContext.Session.GetDataFromSession<List<AdjustItemViewModel>>("TEMP_ADJUST_ITEM_DATA");
                return Json(new { result = true, message = "Edit data success." });
            }
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public JsonResult DeleteTempItem(int seq)
    {
        try
        {
            List<AdjustItemViewModel> res = HttpContext.Session.GetDataFromSession<List<AdjustItemViewModel>>("TEMP_ADJUST_ITEM_DATA");
            AdjustItemViewModel todo = res.FirstOrDefault(m => m.nseq == seq);
            if (todo == null)
            {
                throw new Exception("ไม่สามารถลบข้อมูลได้");
            }

            res.Remove(todo);
            HttpContext.Session.SetDataToSession("TEMP_ADJUST_ITEM_DATA", res);
            return Json(new { result = true, message = "Delete success." });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<JsonResult> FillItemByBranchID(int branchid)
    {
        List<SelectListItem> itemList = null;
        bool result = false;
        try
        {
            //Warehouse
            if (branchid == 1)
            {
                BaseResponse<List<GetItemListResponseDTO>> res = await GetItemSessionDataAsync();
                if (!res.result)
                {
                    throw new Exception("ไม่พบข้อมูลสินค้า");
                }
                itemList = (from a in res.data
                            select new SelectListItem
                            {
                                Text = a.name,
                                Value = a.itemid.ToString()
                            }).ToList();
            }
            else
            {
                //Branch
                BaseResponse<GetItemInBranchByBranchIDResponseDTO> reItemList = await _itemInBranchAPI.GetItemInBranchByBranchIDAsync(branchid);
                if (reItemList.data == null)
                {
                    return Json(new { result = false, message = "ไม่พข้อมูลสินค้าในสาขาดังกล่าว" });
                    //throw new Exception("ไม่พบข้อมูลสินค้า");
                }
                itemList = (from a in reItemList.data.itemlist
                            select new SelectListItem
                            {
                                Text = a.itemname,
                                Value = a.itemid.ToString()
                            }).ToList();
            }

            result = itemList?.Count > 0 ? true : false;
            return Json(new { result = result, data = result ? itemList : null, message = "สำเร็จ" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }


    [HttpGet]
    public async Task<IActionResult> GetAdjustItems()
    {
        try
        {
            BaseResponse<List<GetAdjustItemTransactionsResponseDTO>> resData = await _adjustItemAPI.GetAdjustItemTransactionAsync();
            if (!resData.result)
            {
                throw new Exception(resData.error.error.message);
            }
            return Json(new { data = resData.data });
        }
        catch
        {
            return Json(new { data = new List<GetAdjustItemTransactionsResponseDTO>() });
        }
    }

    [HttpPost]
    public async Task<JsonResult> SearchAdjustItemByBranch(int branchid)
    {
        try
        {
            BaseResponse<List<GetAdjustItemTransactionsResponseDTO>> resData = await _adjustItemAPI.GetAdjustItemTransactionByBranchIDAsync(branchid);
            if (!resData.result)
            {
                throw new Exception(resData.error.error.message);
            }
            return Json(new { result = true, data = resData.data, message = "สำเร็จ" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ไม่สามารถทำรายการได้. {ex.Message}" });
        }
    }

    #region Private
    private async Task<BaseResponse<List<GetAdjustItemTypeResposeDTO>>> GetAdjustItemTypeSessionDataAsync()
    {
        BaseResponse<List<GetAdjustItemTypeResposeDTO>> res = HttpContext.Session.GetDataFromSession<BaseResponse<List<GetAdjustItemTypeResposeDTO>>>("ADJUSTITEMTYPE_DATA");
        if (res != null)
        {
            return res;
        }
        res = await _adjustItemTypeAPI.GetAdjustTypesAsync();
        HttpContext.Session.SetDataToSession("ADJUSTITEMTYPE_DATA", res);
        return res;
    }

    private async Task<BaseResponse<List<GetItemListResponseDTO>>> GetItemSessionDataAsync()
    {
        BaseResponse<List<GetItemListResponseDTO>> res = HttpContext.Session.GetDataFromSession<BaseResponse<List<GetItemListResponseDTO>>>("ITEM_DATA");
        if (res != null)
        {
            return res;
        }
        res = await _itemAPI.GetItemListAsync();
        HttpContext.Session.SetDataToSession("ITEM_DATA", res);
        return res;
    }

    private async Task<BaseResponse<List<GetBranchResponseDTO>>> GetBranchSessionDataAsync()
    {
        BaseResponse<List<GetBranchResponseDTO>> res = HttpContext.Session.GetDataFromSession<BaseResponse<List<GetBranchResponseDTO>>>("BRANCH_DATA");
        if (res != null)
        {
            return res;
        }
        res = await _branchAPI.GetBranchListAsync();
        HttpContext.Session.SetDataToSession("BRANCH_DATA", res);
        return res;
    }

    private CreateAdjustItemCommand MappingCreateAdjustItemCommand(CreateAdjustItemViewModel itemViewModel)
    {
        List<AdjustItemViewModel> tempAdjustItemList = HttpContext.Session.GetDataFromSession<List<AdjustItemViewModel>>("TEMP_ADJUST_ITEM_DATA");
        if (tempAdjustItemList.Count == 0 || tempAdjustItemList.Where(w => w.nqty < 0).Count() > 0)
        {
            throw new Exception("ข้อมูลปรับสต๊อกไม่ถูกต้อง กรุณาตรวจสอบข้อมูลใหม่อีกครั้ง");
        }
        return new CreateAdjustItemCommand
        {
            remark = itemViewModel.Remark,
            createdby = base.UserProfile.rolename,
            createddate = DateTime.Now,
            items = (from a in tempAdjustItemList
                     select new CreateAdjustItemDetailCommand
                     {
                         adjusttypeid = a.nadjusttypeid,
                         branchid = a.nbranchid,
                         itemid = a.nitemid,
                         qty = a.nqty
                     }).ToList()
        };
    }

    private void MappingAdjustItem(ref AdjustItemViewModel targetData, AdjustItemViewModel sourceData)
    {
        targetData.nadjusttypeid = sourceData.nadjusttypeid;
        targetData.nitemid = sourceData.nitemid;
        targetData.nqty = sourceData.nqty;
    }

    private void MappingAddAdjustItem(ref AdjustItemViewModel adjustItem)
    {
        BaseResponse<List<GetAdjustItemTypeResposeDTO>> resAdjustType = HttpContext.Session.GetDataFromSession<BaseResponse<List<GetAdjustItemTypeResposeDTO>>>("ADJUSTITEMTYPE_DATA");
        BaseResponse<List<GetItemListResponseDTO>> resItems = HttpContext.Session.GetDataFromSession<BaseResponse<List<GetItemListResponseDTO>>>("ITEM_DATA");
        BaseResponse<List<GetBranchResponseDTO>> resBranchs = HttpContext.Session.GetDataFromSession<BaseResponse<List<GetBranchResponseDTO>>>("BRANCH_DATA");

        var refAdjustTypeID = adjustItem.nadjusttypeid;
        var refItemID = adjustItem.nitemid;
        var refBranchID = adjustItem.nbranchid;
        adjustItem.sadjusttypename = resAdjustType.data.FirstOrDefault(w => w.adjusttypeid == refAdjustTypeID).adjusttypename;
        adjustItem.sitemname = resItems.data.FirstOrDefault(w => w.itemid == refItemID).name;
        adjustItem.sbranchname = resBranchs.data.FirstOrDefault(w => w.branchid == refBranchID).branchname;
    }
    #endregion

}
