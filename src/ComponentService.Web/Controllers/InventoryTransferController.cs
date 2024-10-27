using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.ExternalService.ItemBrandAPI;
using CYRetailIMS.Application.ExternalService.ItemInBranchAPI;
using CYRetailIMS.Application.ExternalService.ItemTransferAPI;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandList.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInventoryForTransferByBranchID.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInventoryForTransferByDraftID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransferFromDraft.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetDraftItemTransferByBranchID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetDraftItemTransferByCriteria.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using CYRetailIMS.Infrastructure.Common.Extensions;
using CYRetailIMS.Infrastructure.ExternalService.ItemInBranchAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static CYRetailIMS.Application.Common.Models.EnumModel;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Sale, RoleName.Stock)]
public class InventoryTransferController : BaseController
{
    private readonly IItemTransferAPI _itemTransferAPI;
    private readonly IBranchAPI _branchAPI;
    private readonly IItemBrandAPI _itemBrandAPI;
    private readonly IItemInBranchAPI _itemInBranchAPI;
    public InventoryTransferController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IBranchAPI branchAPI,
        IItemBrandAPI itemBrandAPI, 
        IItemTransferAPI itemTransferAPI, 
        IItemInBranchAPI itemInBranchAPI) : base(httpClientRequest, mapper, log)
    {
        _branchAPI = branchAPI;
        _itemBrandAPI = itemBrandAPI;
        _itemTransferAPI = itemTransferAPI;
        _itemInBranchAPI = itemInBranchAPI;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        ViewBag.ItemBrandList = await PrepareSelectBrand();
        return View();
    }

    public IActionResult Draft(int draftid)
    {
        InventoryTransferInquiryViewModel inquiryObj = new InventoryTransferInquiryViewModel
        {
            draftid = draftid
        };
        HttpContext.Session.SetDataToSession("INQUIRY_ITEM_TRANSFER", inquiryObj);
        ViewBag.DraftID = draftid;
        return View();
    }


    [HttpGet]
    public async Task<IActionResult> GetDrafItemTransferOfMonth()
    {
        try
        {
            BaseResponse<List<GetDraftItemTransferByBranchIDResponseDTO>> resDraftItemTransfer = await _itemTransferAPI.GetDraftItemTransferByCriteriaAsync(new GetDraftItemTransferByCriteriaQuery
            {
                transferdate = DateTime.Now
            });
            if (!resDraftItemTransfer.result)
            {
                throw new Exception(resDraftItemTransfer.error.error.message);
            }
            return Json(new { data = resDraftItemTransfer.data });
        }
        catch
        {
            return Json(new { data = new List<GetItemInventoryTransferResposeDTO>() });
        }
    }

    /// <summary>
    /// โอนสินค้า ใหม่
    /// </summary>
    /// <param name="searchObj"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SearchInvenrotyTransfer([FromBody] SearchInvenrotyTransferViewModel searchObj)
    {
        try
        {
            if (searchObj == null)
            {
                return Json(new { result = false, message = $"เงื่อนไขการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง", data = new List<GetDraftItemTransferByBranchIDResponseDTO>() });
            }
            BaseResponse<List<GetDraftItemTransferByBranchIDResponseDTO>> resDraftItemTransfer = await _itemTransferAPI.GetDraftItemTransferByCriteriaAsync(new GetDraftItemTransferByCriteriaQuery
            {
                transferdate = DateTime.Now,
                branchid = searchObj.branchid
            });
            if (!resDraftItemTransfer.result)
            {
                return Json(new { result = false, message = $"ไม่พบข้อมูล", data = new List<GetDraftItemTransferByBranchIDResponseDTO>() });
            }
            return Json(new { result = true, message = resDraftItemTransfer.message, data = resDraftItemTransfer.data });

        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย, เกิดข้อผิดพลาด {ex.Message}", data = new List<GetDraftItemTransferByBranchIDResponseDTO>() });
        }
    }

    [HttpGet]
    public async Task<IActionResult> InquiryDrafItemTransfer()
    {
        try
        {
            InventoryTransferInquiryViewModel inquiryObj = HttpContext.Session.GetDataFromSession<InventoryTransferInquiryViewModel>("INQUIRY_ITEM_TRANSFER");

            BaseResponse<List<GetItemInventoryTransferResposeDTO>> resItemInventoryTransfer = await _itemTransferAPI.InquiryDraftItemTransferByDraftIDAsync(new GetItemInventoryForTransferByDraftIDQuery
            {
                draftid = inquiryObj.draftid
            });
            if (!resItemInventoryTransfer.result)
            {
                throw new Exception(resItemInventoryTransfer.error.error.message);
            }
            return Json(new { data = resItemInventoryTransfer.data });
        }
        catch
        {
            return Json(new { data = new List<GetItemInventoryTransferResposeDTO>() });
        }
    }

    /// <summary>
    /// โอนสินค้า ใหม่
    /// </summary>
    /// <param name="inventoryTransferRequest"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateItemInvenrotyTransferFromDraft([FromBody] CreateInvenrotyTransferViewModel inventoryTransferRequest)
    {
        try
        {
            #region Prepare & Create Transaction
            CreateItemTransferFromDraftCommand createItemTransferByDraftCommand = CreateItemTransferByDraftCommand(inventoryTransferRequest);
            BaseResponse<CommandResponse> resCreateTrn = await _itemTransferAPI.CreateItemTransferFromDrafAsyc(createItemTransferByDraftCommand);
            if (!resCreateTrn.result)
            {
                return Json(new { result = false, message = resCreateTrn.error.error.message });
            }
            #endregion
            return Json(new { result = true, message = "บันทึกข้อมูลสำเร็จ." });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย, เกิดข้อผิดพลาด {ex.Message}", data = new List<GetItemInventoryTransferResposeDTO>() });

        }
    }

    #region Private
    /// <summary>
    /// Only transfer from Warehouse(id=1) to Branch
    /// </summary>
    /// <param name="reqObj"></param>
    /// <returns></returns>
    private CreateItemTransferFromDraftCommand CreateItemTransferByDraftCommand(CreateInvenrotyTransferViewModel reqObj)
    {
        return new CreateItemTransferFromDraftCommand
        {
            draftid = reqObj.draftid,
            transfertypeid = (int)TransferType.WTB,
            sourceid = 1,
            destinationid = reqObj.detail.FirstOrDefault().branchid,
            //description = "",
            createdby = base.UserProfile.username,
            createddate = DateTime.Now,
            transferstatus = (int)TransferStatus.Pending,
            isactive = true,
            items = reqObj.detail.Where(w => w.ischeck == true).Select(s => new CreateItemTransferDetailCommand
            {
                itemid = s.itemid,
                qty = s.refillqty
            }).ToList()
        };
    }

    private async Task<List<SelectListItem>> PrepareSelectBranch()
    {

        BaseResponse<List<GetBranchResponseDTO>> resBranch = await _branchAPI.GetBranchListAsync();
        resBranch.data = base.UserProfile.roleid == (int)EnumModel.UserRole.Sale
            ? resBranch.data.Where(w => base.UserProfile.access_branch.Select(s => s.branchid).Contains(w.branchid)).ToList()
            : resBranch.data;
        return resBranch.data.Select(s => new SelectListItem { Text = s.branchname, Value = s.branchid.ToString() }).ToList();
    }

    private async Task<List<SelectListItem>> PrepareSelectBrand()
    {
        BaseResponse<List<GetItemBrandListResponseDTO>> resBranch = await _itemBrandAPI.GetItemBrandListAsync();
        return resBranch.data.Select(s => new SelectListItem { Text = s.brandname, Value = s.brandid.ToString() }).ToList();
    }
    #endregion
}
