using System.Collections.Generic;
using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.ExternalService.MoneyTransferAPI;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.ItemService.Commands.DeleteItem;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransfer.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.DeleteMoneyTransfer.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.UpdateMoneyTransfer.v1;
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
    private string _moneyTransferSlipSubPath = "money_transfer_slip";
    private readonly IBranchAPI _branchAPI;
    private readonly IMoneyTransferAPI _moneyTransferAPI;
    public MoneyTransferController(IHttpClientRequest httpClientRequest, 
        IMapper mapper, 
        ILog4NetLogger log,
        IBranchAPI branchAPI,
        IMoneyTransferAPI moneyTransferAPI) : base(httpClientRequest, mapper, log)
    {
        _branchAPI = branchAPI;
        _moneyTransferAPI = moneyTransferAPI;
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
    public async Task<IActionResult> GetMoneyTransfer()
    {
        try
        {
            var reqAPI = new GetMoneyTransferByCriteriaQuery
            {
                startdate = DateTime.Now,
                branchlist = base.UserProfile.roleid == (int)EnumModel.UserRole.Admin ? null : base.UserProfile.access_branch.Select(s => s.branchid).ToList(),
            };
            BaseResponse<List<GetMoneyTransferByCriteriaResponseDTO>> resMoneyTransfers = await _moneyTransferAPI.GetMoeytransferByCriteriaAsync(new GetMoneyTransferByCriteriaQuery
            {
                startdate = DateTime.Now
            });
            if (!resMoneyTransfers.result)
            {
                return Json(new { result = false, message = resMoneyTransfers.message, data = new List<GetMoneyTransferByCriteriaResponseDTO>() });
            }
            return Json(new { result = true, message = "สำเร็จ", data = resMoneyTransfers.data });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = ex.Message, data = new List<GetMoneyTransferByCriteriaResponseDTO>() });
        }
    }

    [HttpPost]
    public async Task<IActionResult> SearchMoneyTransfer([FromBody] SearchMoneyTransferViewModel searchData)
    {
        try
        {
            BaseResponse<List<GetMoneyTransferByCriteriaResponseDTO>> resSearch = await _moneyTransferAPI.GetMoeytransferByCriteriaAsync(new GetMoneyTransferByCriteriaQuery
            {
                startdate = searchData.startdate.ToDateTime(),
                enddate = searchData.enddate.ToDateTime(),
                branchlist = new List<int> { searchData.branchid }
            });
            if (!resSearch.result)
            {
                return Json(new { result = false, message = resSearch.error?.error?.message, data = new List<GetMoneyTransferByCriteriaResponseDTO>() });
            }
            return Json(new { result = true, message = "สำเร็จ", data = resSearch.data });
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
    public async Task<IActionResult> CreateTransaction(CreateMoneyTransferViewModel mTransferData)
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
                imgName = Guid.NewGuid().ToString() + Path.GetExtension(mTransferData.ImageFile.FirstOrDefault().FileName);

                //Get url To Save
                imgSavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", _moneyTransferSlipSubPath, imgName);

                #region File Path Check
                FileInfo fInfo = new FileInfo(imgSavePath);
                if (!fInfo.Directory.Exists)
                {
                    fInfo.Directory.Create();
                }
                #endregion

                using (var stream = new FileStream(imgSavePath, FileMode.Create))
                {
                    mTransferData.ImageFile.FirstOrDefault().CopyTo(stream);
                }
                #endregion

            }
            #endregion

            #region Preparing Object to Create
            //mTransferData.ImagePath = $"wwwroot/sale_slip/{ImageName}";
            //mTransferData.CreatedBy = base.UserProfile.username;
            //mTransferData.SaleDate = objData.SelectedDate.DCDateStringToDateTime();
            //var resCreate = await _saleSlipLogService.CreateSlipLog(objData);
            var resCreate = await _moneyTransferAPI.CreateAsync(PrepareCreateRequestData(mTransferData, imgSavePath));
            #endregion

            return Json(new { result = resCreate.result, message = resCreate.result ? resCreate.message : resCreate.error.error.message, data = resCreate.result ? resCreate.data : null });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTransaction([FromBody] EditMoneyTransferViewModel updateMoneyTranfer)
    {
        try
        {
            var resDelete = await _moneyTransferAPI.UpdateAsync(PrepareUpdateObjectData(updateMoneyTranfer));
            return Json(new JsonViewModel { result = resDelete.result, message = resDelete.result ? resDelete.message : resDelete.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new JsonViewModel { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTransaction([FromBody] DeleteMoneyTransferViewModel deleteMoneyTranfer)
    {
        try
        {
            var resDelete = await _moneyTransferAPI.DeleteAsync(new DeleteMoneyTransferCommand
            {
                moeytransferid = deleteMoneyTranfer.moneytransferid,
                updatedby = base.UserProfile.username
            });
            return Json(new JsonViewModel { result = resDelete.result, message = resDelete.result ? resDelete.message : resDelete.error.error.message });
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

    private CreateMoneyTransferCommand PrepareCreateRequestData(CreateMoneyTransferViewModel reqData, string slipImagePath = default)
    {
        return new CreateMoneyTransferCommand
        {
            branchid = reqData.BranchID,
            transferdate = reqData.TransferDate.ToDateTime(),
            //description = reqData
            amounttransfer =reqData.AmountTransfer,
            createdby = base.UserProfile.username,
            slipimagepath = !string.IsNullOrEmpty(slipImagePath) ? slipImagePath : null
        };
    }

    private UpdateMoneyTransferCommand PrepareUpdateObjectData(EditMoneyTransferViewModel reqData, string slipImagePath = default)
    {
        return new UpdateMoneyTransferCommand
        {
            moneytransferid = reqData.MoneyTransferID,
            branchid = reqData.BranchID,
            transferdate = reqData.TransferDate.ToDateTime(),
            amounttransfer = reqData.AmountTransfer,
            //description = reqData
            slipimagepath = !string.IsNullOrEmpty(slipImagePath) ? slipImagePath : null,
            updatedby = base.UserProfile.username,
            isactive = reqData.IsActive
        };
    }
    #endregion
}
