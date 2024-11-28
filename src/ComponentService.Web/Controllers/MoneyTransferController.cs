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
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransfer.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransferList.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.DeleteMoneyTransfer.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.UpdateMoneyTransfer.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Quiries.GetMoneyTransferByCriteria.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Quiries.GetMoneyTransferByID.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Packaging;
using NUglify.Helpers;
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

    public async Task<IActionResult> NewAsync()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        return View();
    }

    public async Task<IActionResult> EditAsync(int mTransferID)
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        ViewBag.ActiveList = PrepareSelectIsActive();
        BaseResponse<GetMoneyTransferByCriteriaResponseDTO> resData = await _moneyTransferAPI.GetMoeytransferByIDAsync(new GetMoneyTransferByIDQuery
        {
            moneytransferid = mTransferID
        });
        EditMoneyTransferViewModel editMoneyTransfer = _mapper.Map<EditMoneyTransferViewModel>(resData.data);
        return View(editMoneyTransfer);
    }

    #region Http method
    [HttpGet]
    public async Task<IActionResult> GetMoneyTransfer()
    {
        try
        {
            var inquiryObj = new GetMoneyTransferByCriteriaQuery
            {
                startdate = DateTime.Now,
                branchlist = base.UserProfile.roleid == (int)EnumModel.UserRole.Admin ? null : base.UserProfile.access_branch.Select(s => s.branchid).ToList(),
            };
            BaseResponse<List<GetMoneyTransferByCriteriaResponseDTO>> resMoneyTransfers = await _moneyTransferAPI.GetMoeytransferByCriteriaAsync(inquiryObj);
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
        DateTime? sDate = null;
        DateTime? eDate = null;
        try
        {
            if (searchData == null ||
                (!searchData.branchid.HasValue && string.IsNullOrEmpty(searchData.startdate) && string.IsNullOrEmpty(searchData.enddate)))
            {
                return Json(new { result = false, message = $"เงื่อนไขการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง", data = new List<GetMoneyTransferByCriteriaResponseDTO>() });
            }

            #region Search date validation
            if (!string.IsNullOrEmpty(searchData.startdate))
            {
                sDate = searchData.startdate.DatetimePickerToDate();
            }
            if (!string.IsNullOrEmpty(searchData.enddate))
            {
                eDate = searchData.enddate.DatetimePickerToDate();
            }
            if (sDate.HasValue && eDate.HasValue)
            {
                //StartDate > EndDate
                if (DateTime.Compare(sDate.Value, eDate.Value) == 1)
                {
                    throw new Exception("รูปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
                }
            }
            #endregion

            BaseResponse<List<GetMoneyTransferByCriteriaResponseDTO>> resSearch = await _moneyTransferAPI.GetMoeytransferByCriteriaAsync(new GetMoneyTransferByCriteriaQuery
            {
                startdate = sDate.HasValue ? sDate.Value : null,
                enddate = eDate.HasValue ? eDate.Value : null,
                branchlist = searchData.branchid.HasValue ? new List<int> { searchData.branchid.Value } : null
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
            mTransferData.SlipImagePath = $"../{_moneyTransferSlipSubPath}/{imgName}";
            //mTransferData.CreatedBy = base.UserProfile.username;
            //mTransferData.SaleDate = objData.SelectedDate.DCDateStringToDateTime();
            //var resCreate = await _saleSlipLogService.CreateSlipLog(objData);
            var resCreate = await _moneyTransferAPI.CreateAsync(PrepareCreateRequestData(mTransferData));
            #endregion

            return Json(new { result = resCreate.result, message = resCreate.result ? resCreate.message : resCreate.error.error.message, data = resCreate.result ? resCreate.data : null });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = ex.Message });
        }
    }


    /// <summary>
    /// Create Transaction with transfer slip version2
    /// </summary>
    /// <param name="mTransferData"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateTransactionV2(CreateMoneyTransferViewModel mTransferData)
    {
        try
        {
            List<MoneyTransferFileUploadModel> files = new List<MoneyTransferFileUploadModel>();
            List<CreateMoneyTransferCommand> moneyTransferCommands = new List<CreateMoneyTransferCommand>();

            #region Get form value
            List<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>> form = Request.Form.ToList();
            #endregion

            #region Prepare new from with not empty value
            form = form.Where(w => w.Key.Contains("outer-item-group")).Where(w => !string.IsNullOrEmpty(w.Value[0])).ToList();
            if (form.Count == 0)
            {
                return Json(new { result = false, msg = $"ขออภัย ข้อมูลการโอนไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!." });
            }
            #endregion

            #region PrePare ItemTransfer List
            List<CreateItemTransferDetailCommand> itemTransferList = new List<CreateItemTransferDetailCommand>();
            #endregion

            DateTime transferDate = mTransferData.TransferDate.ToDate();
            decimal totalAmt = 0;
            decimal totalProfitAmt = 0;
            int idx = form.Count / 2;
            for (int i = 0; i < idx; i++)
            {
                var transferAmount = form.Where(w => w.Key == $"outer-item-group[{i}][txtTransferAmount]").FirstOrDefault().Value[0];
                var transferTime = form.Where(w => w.Key == $"outer-item-group[{i}][txtTransferTime]").FirstOrDefault().Value[0];
                IFormFile postedFile = Request.Form?.Files[$"outer-item-group[{i}][fileUpload]"];
                string fileName = Path.GetFileName(postedFile?.FileName);
                if (!string.IsNullOrEmpty(transferAmount) && !string.IsNullOrEmpty(transferTime))
                {
                    string imgName = string.Empty;
                    string imgSavePath = string.Empty;
                    if (postedFile != null)
                    {
                        //Set Image Name
                        imgName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);

                        //Get url To Save
                        imgSavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", _moneyTransferSlipSubPath, imgName);
                        files.Add(new MoneyTransferFileUploadModel
                        {
                            filename = imgName,
                            filepath = imgSavePath,
                            filedata = postedFile
                        });
                    }

                    string[] time = transferTime.Split(":");
                    DateTime transferDateTime = new DateTime(transferDate.Year, transferDate.Month, transferDate.Day, time.First().ToInt32(), time.Last().ToInt32(), 00);
                    moneyTransferCommands.Add(new CreateMoneyTransferCommand
                    {
                        branchid = mTransferData.BranchID,
                        description = mTransferData.Description,
                        createdby = base.UserProfile.username,
                        slipimagepath = postedFile != null ? $"../{_moneyTransferSlipSubPath}/{imgName}" : null,
                        transferdate = transferDateTime,
                        amounttransfer = transferAmount.ToDecimal()
                    });
                }
            }

            #region Stream Image File

            files.Where(w => w.filedata != null).ForEach(e =>
            {
                #region File Path Check
                FileInfo fInfo = new FileInfo(e.filepath);
                if (!fInfo.Directory.Exists)
                {
                    fInfo.Directory.Create();
                }
                #endregion

                using (var stream = new FileStream(e.filepath, FileMode.Create))
                {
                    e.filedata.CopyTo(stream);
                }
            });

            #endregion

            #region Preparing Object to Create
            //mTransferData.SlipImagePath = $"../{_moneyTransferSlipSubPath}/{imgName}";
            //mTransferData.CreatedBy = base.UserProfile.username;
            //mTransferData.SaleDate = objData.SelectedDate.DCDateStringToDateTime();
            //var resCreate = await _saleSlipLogService.CreateSlipLog(objData);
            var resCreate = await _moneyTransferAPI.BulkCreateAsync(new CreateMoneyTransferListCommand
            {
                mtransferdata = moneyTransferCommands
            });
            #endregion

            return Json(new { result = resCreate.result, message = resCreate.result ? resCreate.message : resCreate.error.error.message, data = resCreate.result ? resCreate.data : null });

            //return Json(new { result = true, message = "บันทึกข้อมูลสำเร็จ." });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย มีบางอย่างผิดพลาด กรุณาลองใหม่อีกครั้ง!. {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> MoneyTransferDataValidation(CreateMoneyTransferViewModel transferItemObj)
    {
        try
        {
            #region Get form value
            List<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>> form = Request.Form.ToList();
            #endregion

            #region Prepare new from with not empty value
            form = form.Where(w => w.Key.Contains("data[outer-item-group]")).Where(w => !string.IsNullOrEmpty(w.Value[0])).ToList();
            if (form.Count == 0)
            {
                return Json(new { result = false, message = $"ขออภัย ข้อมูลขายสินค้าไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!." });
            }
            #endregion

            #region Validate Selling Item
            bool isValidData = form.Where(w => w.Key.Contains("data[outer-item-group]")).Any(w => !string.IsNullOrEmpty(w.Value[0]));
            if (!isValidData)
            {
                return Json(new { result = false, message = $"ขออภัย ข้อมูลขายสินค้าไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!." });
            }
            #endregion

            return Json(new { result = true, message = "ตรวจสอบข้อมูลถูกต้อง." });
        }
        catch (Exception ex)
        {
            return Json(new { result = true, message = $"ขออภัย รูปแบบข้อมูลไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!. {ex.Message}" });
        }
    }

    /// <summary>
    /// Update Transaction with transfer slip
    /// </summary>
    /// <param name="mTransferData"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> UpdateTransaction(EditMoneyTransferViewModel mTransferData)
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
            mTransferData.SlipImagePath = $"../{_moneyTransferSlipSubPath}/{imgName}";
            //mTransferData.CreatedBy = base.UserProfile.username;
            //mTransferData.SaleDate = objData.SelectedDate.DCDateStringToDateTime();
            //var resCreate = await _saleSlipLogService.CreateSlipLog(objData);
            var resUpdate = await _moneyTransferAPI.UpdateAsync(PrepareUpdateObjectData(mTransferData));
            #endregion

            return Json(new { result = resUpdate.result, message = resUpdate.result ? resUpdate.message : resUpdate.error.error.message, data = resUpdate.result ? resUpdate.data : null });
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
            var resDelete = await _moneyTransferAPI.DeleteAsync(new DeleteMoneyTransferCommand
            {
                moneytransferid = deleteMoneyTranfer.moneytransferid,
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

    private List<SelectListItem> PrepareSelectIsActive()
    {
        List<SelectListItem> selectListItems = new List<SelectListItem> 
        {
            new SelectListItem { Text = "เปิดใช้งาน", Value = "true" },
            new SelectListItem { Text = "ยกเลิก", Value = "false" }
        };
        return selectListItems;
    }

    private CreateMoneyTransferCommand PrepareCreateRequestData(CreateMoneyTransferViewModel reqData)
    {
        return new CreateMoneyTransferCommand
        {
            branchid = reqData.BranchID,
            transferdate = reqData.TransferDate.ToDate(),
            amounttransfer = reqData.AmountTransfer,
            description = reqData.Description,
            slipimagepath = reqData.SlipImagePath,
            createdby = base.UserProfile.username
        };
    }

    private UpdateMoneyTransferCommand PrepareUpdateObjectData(EditMoneyTransferViewModel reqData)
    {
        return new UpdateMoneyTransferCommand
        {
            moneytransferid = reqData.MoneyTransferID,
            branchid = reqData.BranchID,
            transferdate = reqData.TransferDate.ToDate(),
            amounttransfer = reqData.AmountTransfer,
            description = reqData.Description,
            slipimagepath = reqData.SlipImagePath,
            updatedby = base.UserProfile.username,
            isactive = reqData.IsActive
        };
    }
    #endregion
}
