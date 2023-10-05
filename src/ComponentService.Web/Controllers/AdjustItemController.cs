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
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

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
        BaseResponse<List<GetAdjustItemTransactionsResponseDTO>> resAdjustItem = await _adjustItemAPI.GetAdjustItemTransactionAsync();
        BaseResponse<List<GetBranchListResponseDTO>> resBranchList = await _branchAPI.GetBranchListAsync();
        ViewBag.AdjustItemTransactions = resAdjustItem;
        ViewBag.BranchList = resBranchList;
        return View();
    }

    public async Task<IActionResult> Adjust()
    {
        BaseResponse<List<GetAdjustItemTypeResposeDTO>> resAdjustType = await _adjustItemTypeAPI.GetAdjustTypesAsync();
        BaseResponse<List<GetItemListResponseDTO>> resItem = await _itemAPI.GetItemListAsync();

        ViewBag.AdjustItemType = resAdjustType;
        ViewBag.ItemList = resItem;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAdjustItem([FromBody] CreateAdjustItemViewModel adjustItemData)
    {
        try
        {
            if(adjustItemData.Qty < 0)
            {
                throw new Exception("กรุณาระบุจำนวนไม่น้อยกว่า 0");
            }
            CreateAdjustItemCommand CreateAdjustItemCommand = MappingCreateAdjustItemCommand(adjustItemData);
            BaseResponse<CommandResponse> res = await _adjustItemAPI.CreateAdjustItemAsync(CreateAdjustItemCommand);
            return Json(new { result = res.result, message = res.result ? "ปรับสต๊อกสินค้าสำเร็จ" : $"ไม่สามารถทำรายการได้, {res.error.error.message}" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    private CreateAdjustItemCommand MappingCreateAdjustItemCommand(CreateAdjustItemViewModel itemViewModel)
    {
        return new CreateAdjustItemCommand
        {
            adjusttypeid = itemViewModel.AdjustTypeID,
            itemid = itemViewModel.ItemID,
            qty = itemViewModel.Qty,
            remark = itemViewModel.Remark,
            createdby = base.UserProfile.rolename,
            createddate = DateTime.Now
        };
    }
}
