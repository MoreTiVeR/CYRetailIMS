using System.Collections.Generic;
using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.ItemService.Commands.DeleteItem;
using CYRetailIMS.Application.Services.MoneyTransferService.Quiries.GetMoneyTransferByCriteria.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SixLabors.ImageSharp;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Sale, RoleName.AreaSale)]
public class MoneyTransferController : BaseController
{
    private readonly IBranchAPI _branchAPI;
    public MoneyTransferController(IHttpClientRequest httpClientRequest, 
        IMapper mapper, 
        ILog4NetLogger log,
        IBranchAPI branchAPI) : base(httpClientRequest, mapper, log)
    {
        _branchAPI = branchAPI;
    }

    public async Task<IActionResult> IndexAsync()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        return View();
    }

    public async Task<IActionResult> CreateAsync()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        return View();
    }

    public async Task<IActionResult> EditAsync(int tranferID)
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        return View();
    }

    #region Http method
    [HttpGet]
    public IActionResult GetMoneyTransfer()
    {
        try
        {
            List<GetMoneyTransferByCriteriaResponseDTO> resMoneyTransfer = new List<GetMoneyTransferByCriteriaResponseDTO>();
            resMoneyTransfer.Add(new GetMoneyTransferByCriteriaResponseDTO
            {
                moneytransferid = 1,
                branchid = 3,
                branchname = "บางพลี",
                amounttransfer = 5200,
                description = "เงินโอนสาขาบางพลี",
                createdby = "admin",
                createddate = DateTime.Now.AddDays(-1),
                transferdate = DateTime.Now.AddDays(-1)
            });
            resMoneyTransfer.Add(new GetMoneyTransferByCriteriaResponseDTO
            {
                moneytransferid = 2,
                branchid = 4,
                branchname = "บางนา",
                amounttransfer = 7300,
                description = "เงินโอนสาขาบางนา",
                createdby = "admin",
                createddate = DateTime.Now,
                transferdate = DateTime.Now
            });
            return Json(new { result = false, message = "สำเร็จ", data = resMoneyTransfer });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = ex.Message, data = new List<GetMoneyTransferByCriteriaResponseDTO>() });
        }
    }

    [HttpPost]
    public IActionResult SearchMoneyTransfer([FromBody] SearchMoneyTransferViewModel searchData)
    {
        try
        {
            return Json(new { result = false, message = "สำเร็จ", data = new List<GetMoneyTransferByCriteriaResponseDTO>() });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = ex.Message, data = new List<GetMoneyTransferByCriteriaResponseDTO>() });
        }
    }

    /// <summary>
    /// Create Transaction with transfer slip
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CreateTransaction(CreateMoneyTransferViewModel mTransferData)
    {
        string imgName = string.Empty;
        string imgSavePath = string.Empty;
        try
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, msg = $"ขออภัย รูปแบบข้อมูลไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!." });
            }

            #region Image File is not null then stream image
            if (mTransferData.ImageFile != null)
            {
                #region Image File
                //Set Key Name
                imgName = Guid.NewGuid().ToString() + Path.GetExtension(mTransferData.ImageFile.FileName);

                //Get url To Save
                imgSavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/money_transfer_slip", imgName);

                #region File Path Check
                FileInfo fInfo = new FileInfo(imgSavePath);
                if (!fInfo.Directory.Exists)
                {
                    fInfo.Directory.Create();
                }
                #endregion

                using (var stream = new FileStream(imgSavePath, FileMode.Create))
                {
                    mTransferData.ImageFile.CopyTo(stream);
                }
                #endregion

            }
            #endregion

            #region Preparing Object to Create
            DateTime transferDate = mTransferData.TransferDate.ToDate();
            string ImagePath = $"wwwroot/sale_slip/{imgName}";
            string CreatedBy = base.UserProfile.username;
            //mTransferData.ImagePath = $"wwwroot/sale_slip/{ImageName}";
            //mTransferData.CreatedBy = base.UserProfile.username;
            //mTransferData.SaleDate = objData.SelectedDate.DCDateStringToDateTime();
            //var resCreate = await _saleSlipLogService.CreateSlipLog(objData);
            #endregion

            return Json(new { result = false, message = "สำเร็จ" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTransaction([FromBody] DeleteMoneyTransferViewModel deleteMoneyTranfer)
    {
        try
        {
            await Task.Run(() =>
            {
                Thread.Sleep(100);
            });
            //DeleteItemCommand delItemCommand = new DeleteItemCommand { itemid = delItemObj.itemid, deletedby = base.UserProfile.username };
            //BaseResponse<CommandResponse> resDelItem = await _itemAPI.DeleteItemAsync(delItemCommand);
            //if (resDelItem.result)
            //{
            //    return Json(new JsonViewModel { result = resDelItem.result, message = resDelItem.message });
            //}
            return Json(new JsonViewModel { result = true, message = "สำเร็จ" });
        }
        catch (Exception ex)
        {
            return Json(new JsonViewModel { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }
    #endregion

    #region Private method
    private async Task<List<SelectListItem>> PrepareSelectBranch()
    {

        BaseResponse<List<GetBranchResponseDTO>> resBranch = await _branchAPI.GetBranchListAsync();
        resBranch.data = base.UserProfile.roleid == (int)EnumModel.UserRole.Sale
            ? resBranch.data.Where(w => base.UserProfile.access_branch.Select(s => s.branchid).Contains(w.branchid)).ToList()
            : resBranch.data;
        return resBranch.data.Select(s => new SelectListItem { Text = s.branchname, Value = s.branchid.ToString() }).ToList();
    }
    #endregion
}
