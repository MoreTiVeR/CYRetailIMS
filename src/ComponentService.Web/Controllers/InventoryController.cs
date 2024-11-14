using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
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
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateDraftItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransferFromDraft.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.DeleteDraftItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.UpdateDraftItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetDraftItemTransferByBranchID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetDraftItemTransferByCriteria.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.ValidatePrintDraftItemTransferByDraftID.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.DeleteMoneyTransfer.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Infrastructure.Common.Extensions;
using CYRetailIMS.Infrastructure.ExternalService.ItemInBranchAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using static CYRetailIMS.Application.Common.Models.EnumModel;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Sale, RoleName.Stock)]
public class InventoryController : BaseController
{
    private readonly IItemTransferAPI _itemTransferAPI;
    private readonly IBranchAPI _branchAPI;
    private readonly IItemBrandAPI _itemBrandAPI;
    private readonly IItemInBranchAPI _itemInBranchAPI;
    public InventoryController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
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

    public async Task<IActionResult> Transfer()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        ViewBag.ItemBrandList = await PrepareSelectBrand();
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
    /// Search for Transfer page
    /// </summary>
    /// <param name="searchObj"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SearchInvenrotyTransferForTransfer([FromBody] SearchInvenrotyTransferViewModel searchObj)
    {
        int branchId = 0;
        try
        {
            if (searchObj == null)
            {
                return Json(new { result = false, message = $"เงื่อนไขการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง", data = new List<GetDraftItemTransferByBranchIDResponseDTO>() });
            }

            if (searchObj.branchid.HasValue)
            {
                branchId = searchObj.branchid.Value;
            }

            BaseResponse<List<GetItemInventoryTransferResposeDTO>> resItemInventoryTransfer = await _itemInBranchAPI.GetItemInventoryForTransferAsync(new GetItemInventoryTransferQuery
            {
                branchid = branchId,
                brandid = searchObj.brandid
            });
            if (!resItemInventoryTransfer.result)
            {
                return Json(new { result = false, message = $"ไม่พบข้อมูล", data = new List<GetItemInventoryTransferResposeDTO>() });
            }
            return Json(new { result = true, message = resItemInventoryTransfer.message, data = resItemInventoryTransfer.data });

        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย, เกิดข้อผิดพลาด {ex.Message}", data = new List<GetItemInventoryTransferResposeDTO>() });
        }
    }

    /// <summary>
    /// Search for Index page
    /// </summary>
    /// <param name="searchObj"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SearchInvenrotyTransferForIndex([FromBody] SearchInvenrotyTransferViewModel searchData)
    {
        DateTime? sDate = null;
        DateTime? eDate = null;
        try
        {
            if (searchData == null ||
                (!searchData.branchid.HasValue && string.IsNullOrEmpty(searchData.startdate) && string.IsNullOrEmpty(searchData.enddate)))
            {
                return Json(new { result = false, message = $"เงื่อนไขการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง", data = new List<GetDraftItemTransferByBranchIDResponseDTO>() });
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
            BaseResponse<List<GetDraftItemTransferByBranchIDResponseDTO>> resDraftItemTransfer = await _itemTransferAPI.GetDraftItemTransferByCriteriaAsync(new GetDraftItemTransferByCriteriaQuery
            {
                transferdate = sDate,
                transferenddate = eDate,
                branchid = searchData.branchid
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

    [HttpGet]
    public async Task<IActionResult> GetItemInventoryTransfer()
    {
        try
        {
            BaseResponse<List<GetItemInventoryTransferResposeDTO>> resItemInventoryTransfer = await _itemInBranchAPI.GetItemInventoryForTransferAsync(new GetItemInventoryTransferQuery
            {
                branchid = 1
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
    /// ตรวจสอบรายการสินค้าที่ทำโอน
    /// </summary>
    /// <param name="inventoryTransferRequest"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult ItemInvenrotyTransferValidation([FromBody] CreateInvenrotyTransferViewModel inventoryTransferRequest)
    {
        try
        {
            if (inventoryTransferRequest.detail == null)
            {
                return Json(new { result = false, message = $"ไม่สามารถทำรายการได้ เนื่องจากข้อมูลไม่ถูกต้อง" });
            }

            var refillItem = inventoryTransferRequest.detail.Where(w => w.ischeck);
            if (!refillItem.Any())
            {
                return Json(new { result = false, message = $"ไม่สามารถทำรายการได้ กรุณาติ๊กเลือกเลือกสินค้าโอนก่อนทำรายการ" });
            }

            return Json(new { result = true, message = "สำเร็จ" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย, เกิดข้อผิดพลาด {ex.Message}" });

        }
    }

    /// <summary>
    /// Create Item Transfer : สร้างรายการ โอนสินค้า
    /// </summary>
    /// <param name="inventoryTransferRequest"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateItemInvenrotyTransfer([FromBody] CreateInvenrotyTransferViewModel inventoryTransferRequest)
    {
        try
        {
            #region Validate item detail is checked
            if (inventoryTransferRequest.detail.Where(w => w.ischeck == true).Count() == 0)
            {
                return Json(new { result = false, message = $"ไม่สามารถทำรายการได้ กรุณาติ๊กเลือกเลือกสินค้าโอนก่อนบันทึกรายการ" });
            }
            #endregion

            #region Prepare & Create Transaction
            CreateItemTransferCommand createItemTransferCommand = CreateItemTransferCommand(inventoryTransferRequest);
            BaseResponse<CommandResponse> resCreateTrn = await _itemTransferAPI.CreateItemTransferAsync(createItemTransferCommand);
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

    /// <summary>
    /// Create Item Transfer from Draft Data : สร้างรายการ โอนสินค้า ใหม่ จากบันทึกร่าง-ดราฟ
    /// </summary>
    /// <param name="inventoryTransferRequest"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateItemInvenrotyTransferFromDraft([FromBody] CreateInvenrotyTransferViewModel inventoryTransferRequest)
    {
        try
        {
            #region Validate item detail is checked
            if (inventoryTransferRequest.detail.Where(w => w.ischeck == true).Count() == 0)
            {
                return Json(new { result = false, message = $"ไม่สามารถทำรายการได้ กรุณาติ๊กเลือกเลือกสินค้าโอนก่อนบันทึกรายการ" });
            }
            #endregion

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

    /// <summary>
    /// Create Draft Item Transfer : สร้างรายการ ดราฟ โอนสินค้า
    /// </summary>
    /// <param name="inventoryTransferRequest"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateDraftItemInvenrotyTransfer([FromBody] CreateInvenrotyTransferViewModel inventoryTransferRequest)
    {
        try
        {
            #region Validate item detail is checked
            if (inventoryTransferRequest.detail.Where(w => w.ischeck == true).Count() == 0)
            {
                return Json(new { result = false, message = $"ไม่สามารถทำรายการได้ กรุณาติ๊กเลือกเลือกสินค้าโอนก่อนบันทึกรายการ" });
            }
            #endregion

            #region Prepare & Create, Update Draft Transaction
            BaseResponse<CommandResponse> resCreateTrn;
            if (inventoryTransferRequest.draftid <= 0)
            {
                #region Create
                CreateDraftItemTransferCommand createDraftItemTransferCmd = CreateDraftItemTransferCommand(inventoryTransferRequest);
                resCreateTrn = await _itemTransferAPI.CreateDraftItemTransferAsync(createDraftItemTransferCmd);
                #endregion
            }
            else
            {
                #region Update
                UpdateDraftItemTransferCommand updateDraftItemTransferCmd = PrepareUpdateDraftItemTransferCommand(inventoryTransferRequest);
                resCreateTrn = await _itemTransferAPI.UpdateDraftItemTransferAsync(updateDraftItemTransferCmd);
                #endregion
            }

            if (!resCreateTrn.result)
            {
                return Json(new { result = false, message = resCreateTrn.error.error.message });
            }
            #endregion
            return Json(new { result = true, message = "บันทึกข้อมูลฉบับร่างสำเร็จ." });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย, เกิดข้อผิดพลาด {ex.Message}", data = new List<GetItemInventoryTransferResposeDTO>() });

        }
    }

    /// <summary>
    /// If transferstatus equal 1 then can't delete
    /// </summary>
    /// <param name="deleteDraftTransferItem"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> DeleteDraft([FromBody] DeleteDraftTransferItemViewModel deleteDraftTransferItem)
    {
        try
        {
            var resDelete = await _itemTransferAPI.DeleteDraftItemTransferAsync(new DeleteDraftItemTransferCommand
            {
                draftid = deleteDraftTransferItem.transferheaderid,
                updatedby = base.UserProfile.username
            });
            return Json(new JsonViewModel { result = resDelete.result, message = resDelete.result ? resDelete.message : resDelete.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new JsonViewModel { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    /// <summary>
    /// If transferstatus not equal 1 then can't print/download excel
    /// </summary>
    /// <param name="draftid"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<JsonResult> PrepareGenerateExcelExport(int draftid)
    {
        if (draftid == 0)
        {
            return new JsonResult(new { result = false, message = "ข้อมูลไม่ถูกต้อง กรุณาทำรายการใหม่อีกครั้ง" });
        }

        var resValidation = await _itemTransferAPI.ValidatePrintDraftItemTransferByDraftIDAsync(new ValidatePrintDraftItemTransferQuery
        {
            draftid = draftid
        });

        if (!resValidation.result || !resValidation.data.ispass)
        {
            return Json(new JsonViewModel { result = false, message = resValidation.result ? resValidation.message : resValidation.error.error.message });
        }

        string sheetName = DateTime.Now.ToString("dd-MM-yyyy");
        string fName = $"รายงานโอนสินค้า_{sheetName}.xlsx";
        return new JsonResult(new { result = true, fileName = fName, message = "Success." });
    }

    [HttpPost]
    public async Task<IActionResult> DownloadInventoryTransferExcel(int draftid)
    {
        try
        {
            var resInvItemTransfer = await _itemTransferAPI.InquiryDraftItemTransferByDraftIDAsync(new GetItemInventoryForTransferByDraftIDQuery
            {
                draftid = draftid
            });

            #region Generate Excel
            System.Drawing.Color orangeColor = System.Drawing.ColorTranslator.FromHtml("#ffc336");
            string sheetName = DateTime.Now.ToString("dd-MM-yyyy");
            string fName = $"รายงานโอนสินค้า_{sheetName}.xlsx";
            byte[] result;
            using (var package = new ExcelPackage())
            {
                // add a new worksheet to the empty workbook
                var worksheet = package.Workbook.Worksheets.Add(sheetName);

                //Header
                worksheet.Cells[1, 1].Value = "ลำดับ";
                worksheet.Cells[1, 2].Value = "รหัสสินค้า";
                worksheet.Cells[1, 3].Value = "ชื่อสินค้า";
                worksheet.Cells[1, 4].Value = "จำนวนที่เติม";
                worksheet.Cells[1, 5].Value = "จำนวนรับสินค้า";
                worksheet.Cells[1, 6].Value = "ขาด/เกิน";

                //worksheet.Cells[1, 1, 1, 6].Merge = true;
                //worksheet.Cells[1, 1, 1, 6].Value = $"Time : {DateTime.Now.ToString("dd/MM/yyyy HH:mm", new System.Globalization.CultureInfo("en-US"))}";
                //worksheet.Cells[1, 1, 1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(orangeColor);
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 11;
                }

                result = package.GetAsByteArray();
            }
            #endregion

            return File(result, "application/ms-excel", $"{fName}");
        }
        catch (Exception ex)
        {
            return Json(new JsonViewModel { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
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

    /// <summary>
    /// Only transfer from Warehouse(id=1) to Branch
    /// </summary>
    /// <param name="reqObj"></param>
    /// <returns></returns>
    private CreateItemTransferCommand CreateItemTransferCommand(CreateInvenrotyTransferViewModel reqObj)
    {
        return new CreateItemTransferCommand
        {
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

    private CreateDraftItemTransferCommand CreateDraftItemTransferCommand(CreateInvenrotyTransferViewModel reqObj)
    {
        return new CreateDraftItemTransferCommand
        {
            transfertypeid = (int)TransferType.WTB,
            sourceid = 1,
            destinationid = reqObj.detail.FirstOrDefault().branchid,
            //description = "",
            createdby = base.UserProfile.username,
            createddate = DateTime.Now,
            transferstatus = (int)TransferStatus.Draft,
            isactive = true,
            items = reqObj.detail.Where(w => w.ischeck == true).Select(s => new CreateItemTransferDetailCommand
            {
                itemid = s.itemid,
                qty = s.refillqty
            }).ToList()
        };
    }

    private UpdateDraftItemTransferCommand PrepareUpdateDraftItemTransferCommand(CreateInvenrotyTransferViewModel reqObj)
    {
        return new UpdateDraftItemTransferCommand
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

    #endregion
}
