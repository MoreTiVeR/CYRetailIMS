using System.Drawing;
using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.ExternalService.ExcelAPI;
using CYRetailIMS.Application.ExternalService.ItemBrandAPI;
using CYRetailIMS.Application.ExternalService.ItemInBranchAPI;
using CYRetailIMS.Application.ExternalService.ItemTransferAPI;
using CYRetailIMS.Application.ExternalService.ReportAPI;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.ExcelService.Queries.GenerateStockTransferExcelReport.v1;
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandList.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInventoryForTransferByBranchID.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInventoryForTransferByDraftID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateDraftItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer.v2;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransferFromDraft.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.DeleteDraftItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.UpdateDraftItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetDraftItemTransferByBranchID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetDraftItemTransferByCriteria.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.ValidatePrintDraftItemTransferByDraftID.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.DeleteMoneyTransfer.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.InventoryTransferByDraftID.v1;
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
    private readonly IReportAPI _reportAPI;
    private readonly IExcelAPI _excelAPI;
    public InventoryController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IBranchAPI branchAPI,
        IItemBrandAPI itemBrandAPI,
        IItemTransferAPI itemTransferAPI,
        IItemInBranchAPI itemInBranchAPI,
        IReportAPI reportAPI,
        IExcelAPI excelAPI) : base(httpClientRequest, mapper, log)
    {
        _branchAPI = branchAPI;
        _itemBrandAPI = itemBrandAPI;
        _itemTransferAPI = itemTransferAPI;
        _itemInBranchAPI = itemInBranchAPI;
        _reportAPI = reportAPI;
        _excelAPI = excelAPI;
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
            CreateItemTransferWithDraftCommand createItemTransferCommand = CreateItemTransferCommand(inventoryTransferRequest);
            BaseResponse<CommandResponse> resCreateTrn = await _itemTransferAPI.CreateItemTransferV2Async(createItemTransferCommand);
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
            return Json(new JsonViewModel { result = false, message = resValidation.data.remark });
        }

        string sheetName = DateTime.Now.ToString("dd-MM-yyyy");
        string fName = $"รายงานโอนสินค้า_{sheetName}.xlsx";
        return new JsonResult(new { result = true, fileName = fName, message = "Success." });
    }

    [HttpPost]
    public async Task<IActionResult> DownloadInventoryTransferExcel(int draftID, string fName)
    {
        try
        {
            BaseResponse<List<GetItemInventoryTransferResposeDTO>> resInvItemTransfer = await _itemTransferAPI.InquiryDraftItemTransferByDraftIDAsync(new GetItemInventoryForTransferByDraftIDQuery
            {
                draftid = draftID
            });
            return await GenerateStockTransferReport(fName, draftID, resInvItemTransfer.data);
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
    private CreateItemTransferWithDraftCommand CreateItemTransferCommand(CreateInvenrotyTransferViewModel reqObj)
    {
        return new CreateItemTransferWithDraftCommand
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

    #region Generate Excel
    private async Task<FileContentResult> GenerateStockTransferReport(string fName, int draftID, List<GetItemInventoryTransferResposeDTO> invReportData)
    {
        //int dRow = 1;
        //int dCol = 1;

        ////Autofit with minimum size for the column.
        //double autofitMinimumSize = 10;

        ////Autofit with minimum and maximum size for the column.
        //double autofitMaximumSize = 50;

        //#region Get Report Data
        //var resReportData = await _reportAPI.GetInventoryTransferByDraftReportAsync(new InventoryTransferReportByDraftIDQuery
        //{
        //    transferid = draftID
        //});
        //int branchID = resReportData.data.destinationbranchid;
        //string branchName = resReportData.data.destinationbranchname;
        //string createdByName = resReportData.data.createdbyname;
        //string refNo = resReportData.data.refno;
        //string createdDate = $"{resReportData.data.createddate.ToString("dd/MM/yyyy HH:mm", new System.Globalization.CultureInfo("en-US"))}";
        //int totalTransferQty = resReportData.data.totaltransferqty;
        //#endregion

        //#region Generate Excel
        //System.Drawing.Color orangeColor = System.Drawing.ColorTranslator.FromHtml("#ffc336");
        //System.Drawing.Color blueColor = System.Drawing.ColorTranslator.FromHtml("#66a2fb");
        //System.Drawing.Color lightBlueColor = System.Drawing.ColorTranslator.FromHtml("#c0d7f9");
        //System.Drawing.Color yellowColor = System.Drawing.ColorTranslator.FromHtml("#ebf916");
        //System.Drawing.Color grayColor = System.Drawing.ColorTranslator.FromHtml("#b9b4b4");
        //System.Drawing.Color lightGrayColor = System.Drawing.ColorTranslator.FromHtml("#e7e8ea");
        //byte[] result;
        //using (var package = new ExcelPackage())
        //{
        //    //Add a new worksheet to the empty workbook
        //    var worksheet = package.Workbook.Worksheets.Add(fName);

        //    #region Branch/Employee Header
        //    worksheet.Cells[dRow, 3, dRow + 1, 3].Value = "ใบโอนสินค้า";
        //    worksheet.Cells[dRow, 3, dRow + 1, 3].Merge = true;
        //    worksheet.Cells[dRow, 3, dRow + 1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    using (var range = worksheet.Cells[dRow, 2, dRow + 1, 3])
        //    {
        //        range.Style.Font.Bold = true;
        //        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //        range.Style.Fill.BackgroundColor.SetColor(blueColor);
        //        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //        range.Style.Font.Size = 11;
        //    }

        //    worksheet.Cells[dRow, 4, dRow + 1, 4].Value = "เลขอ้างอิง2";
        //    worksheet.Cells[dRow, 4, dRow + 1, 4].Merge = true;
        //    worksheet.Cells[dRow, 4, dRow + 1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    using (var range = worksheet.Cells[dRow, 4, dRow + 1, 4])
        //    {
        //        range.Style.Font.Bold = true;
        //        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //        range.Style.Fill.BackgroundColor.SetColor(yellowColor);
        //        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //        range.Style.Font.Size = 11;
        //    }

        //    worksheet.Cells[dRow, 5, dRow + 1, 5].Value = "พนักงานยิง";
        //    worksheet.Cells[dRow, 5, dRow + 1, 5].Merge = true;
        //    worksheet.Cells[dRow, 5, dRow + 1, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    using (var range = worksheet.Cells[dRow, 5, dRow + 1, 5])
        //    {
        //        range.Style.Font.Bold = true;
        //        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //        range.Style.Fill.BackgroundColor.SetColor(yellowColor);
        //        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //        range.Style.Font.Size = 11;
        //    }

        //    worksheet.Cells[dRow, 6, dRow + 1, 6].Value = "พนักงานแพ็ค";
        //    worksheet.Cells[dRow, 6, dRow + 1, 6].Merge = true;
        //    worksheet.Cells[dRow, 6, dRow + 1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    using (var range = worksheet.Cells[dRow, 6, dRow + 1, 6])
        //    {
        //        range.Style.Font.Bold = true;
        //        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //        range.Style.Fill.BackgroundColor.SetColor(yellowColor);
        //        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //        range.Style.Font.Size = 11;
        //    }

        //    dRow = dRow + 2;
        //    #endregion

        //    #region Branch/Employee detail
        //    //รหัสสาขา, วันที่สร้าง
        //    worksheet.Cells[dRow, 2].Value = branchID;
        //    worksheet.Cells[dRow, 2].Style.Font.Size = 14;
        //    worksheet.Cells[dRow + 1, 2].Value = createdDate;
        //    using (var range = worksheet.Cells[dRow, 2, dRow + 1, 2])
        //    {
        //        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //        range.Style.Fill.BackgroundColor.SetColor(lightBlueColor);
        //        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    }

        //    //ชื่อสาขา, รวมส่งออก text
        //    worksheet.Cells[dRow, 3].Value = branchName;
        //    worksheet.Cells[dRow, 3].Style.Font.Size = 14;
        //    worksheet.Cells[dRow, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    worksheet.Cells[dRow + 1, 3].Value = "รวมส่งออก";
        //    worksheet.Cells[dRow + 1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
        //    using (var range = worksheet.Cells[dRow, 3, dRow + 1, 3])
        //    {
        //        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //        range.Style.Fill.BackgroundColor.SetColor(lightBlueColor);
        //    }

        //    //เลขอ้างอิง2, จำนวนรวมส่งออก
        //    worksheet.Cells[dRow, 4].Value = refNo;
        //    //text format
        //    worksheet.Cells[dRow, 4].Style.Numberformat.Format = "@";
        //    worksheet.Cells[dRow, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    worksheet.Cells[dRow + 1, 4].Value = totalTransferQty;
        //    worksheet.Cells[dRow + 1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

        //    //พนักงานยิง
        //    worksheet.Cells[dRow, 5, dRow + 1, 5].Value = createdByName;
        //    worksheet.Cells[dRow, 5, dRow + 1, 5].Merge = true;
        //    worksheet.Cells[dRow, 5, dRow + 1, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    worksheet.Cells[dRow, 5, dRow + 1, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

        //    //พนักงานแพ็ค
        //    worksheet.Cells[dRow, 6, dRow + 1, 6].Value = string.Empty;
        //    worksheet.Cells[dRow, 6, dRow + 1, 6].Merge = true;
        //    worksheet.Cells[dRow, 6, dRow + 1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    worksheet.Cells[dRow, 6, dRow + 1, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

        //    dRow = dRow + 2;
        //    #endregion

        //    #region Transfer Item
        //    //Item Transfer Header
        //    worksheet.Cells[dRow, 1].Value = "ลำดับ";
        //    worksheet.Cells[dRow, 2].Value = "รหัสสินค้า";
        //    worksheet.Cells[dRow, 3].Value = "ชื่อสินค้า";
        //    worksheet.Cells[dRow, 4].Value = "จำนวนที่เติม";
        //    worksheet.Cells[dRow, 5].Value = "จำนวนรับสินค้า";
        //    worksheet.Cells[dRow, 6].Value = "ขาด/เกิน";

        //    //Set Item Transfer Header Style
        //    using (var range = worksheet.Cells[dRow, 1, dRow, 6])
        //    {
        //        range.Style.Font.Bold = true;
        //        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //        range.Style.Fill.BackgroundColor.SetColor(grayColor);
        //        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //        range.Style.Font.Size = 10;
        //    }
        //    dRow++;

        //    int startHeaderItemRow = dRow;
        //    int endHeaderItemRow = 0;
        //    foreach (var item in resReportData.data.detail)
        //    {
        //        worksheet.Cells[dRow, 1].Value = item.seq;
        //        worksheet.Cells[dRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        worksheet.Cells[dRow, 2].Value = item.itemcode;
        //        worksheet.Cells[dRow, 3].Value = item.itemname;
        //        worksheet.Cells[dRow, 4].Value = item.transferqty;
        //        worksheet.Cells[dRow, 5].Value = item.receiveqty <= 0 ? null : item.receiveqty;
        //        worksheet.Cells[dRow, 6].Value = item.excessqty <= 0 ? null : item.excessqty;
        //        using (var range = worksheet.Cells[dRow, 4, dRow, 6])
        //        {
        //            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        }
        //        dRow++;
        //    }

        //    //Row รวมทั้งหมด
        //    endHeaderItemRow = dRow;
        //    worksheet.Cells[dRow, 3].Value = "รวมทั้งหมด";
        //    worksheet.Cells[dRow, 3].Style.Font.Bold = true;
        //    worksheet.Cells[dRow, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
        //    //worksheet.Cells[dRow, 4].Value = resReportData.data.totaltransferqty;
        //    worksheet.Cells[dRow, 4].Formula = $"SUM({worksheet.Cells[startHeaderItemRow, 4].Address}:{worksheet.Cells[endHeaderItemRow - 1, 4].Address})";
        //    worksheet.Cells[dRow, 4].Calculate();
        //    worksheet.Cells[dRow, 4].Style.Font.Bold = true;
        //    worksheet.Cells[dRow, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    dRow++;
        //    #endregion

        //    #region SubItem
        //    //Header SubItem
        //    worksheet.Cells[dRow, 1].Value = "ลำดับ";
        //    worksheet.Cells[dRow, 2].Value = "ประเภทฟิล์ม";
        //    worksheet.Cells[dRow, 3].Value = "จำนวนทำออก";

        //    //Set Header SubItem Style
        //    using (var range = worksheet.Cells[dRow, 1, dRow, 3])
        //    {
        //        range.Style.Font.Bold = true;
        //        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //        range.Style.Fill.BackgroundColor.SetColor(grayColor);
        //        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //        range.Style.Font.Size = 10;
        //    }
        //    dRow++;

        //    int startSubItemRow = dRow;
        //    int endSubItemRow = 0;
        //    foreach (var item in resReportData.data.subitemdetail)
        //    {
        //        worksheet.Cells[dRow, 1].Value = item.seq;
        //        worksheet.Cells[dRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        worksheet.Cells[dRow, 2].Value = item.subitemtypename;
        //        worksheet.Cells[dRow, 3].Value = item.transferqty;
        //        worksheet.Cells[dRow, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        dRow++;
        //    }

        //    endSubItemRow = dRow;
        //    //Row รวมทั้งหมด
        //    worksheet.Cells[dRow, 2].Value = "จำนวนรวม";
        //    worksheet.Cells[dRow, 2].Style.Font.Bold = true;
        //    worksheet.Cells[dRow, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
        //    worksheet.Cells[dRow, 3].Value = resReportData.data.totalsubitemtransferqty;
        //    worksheet.Cells[dRow, 3].Style.Font.Bold = true;
        //    worksheet.Cells[dRow, 3].Formula = $"SUM({worksheet.Cells[startSubItemRow, 3].Address}:{worksheet.Cells[endSubItemRow - 1, 3].Address})";
        //    worksheet.Cells[dRow, 3].Calculate();
        //    worksheet.Cells[dRow, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

        //    #region Set fonesize for all item row
        //    using (var range = worksheet.Cells[startHeaderItemRow, 1, endSubItemRow, 6])
        //    {
        //        range.Style.Font.Size = 10;
        //    }
        //    #endregion

        //    dRow++;
        //    #endregion

        //    #region ท้ายตาราง ชื่อพนักงาน/ วันที่รับสินค้า/ วันที่นับสินค้า
        //    dRow++;
        //    //ชื่อพนักงาน, วันที่รับสินค้า, วันที่นับสินค้า
        //    worksheet.Cells[dRow, 1, dRow, 2].Value = "ชื่อพนักงาน";
        //    using (var range = worksheet.Cells[dRow, 1, dRow, 2])
        //    {
        //        range.Merge = true;
        //        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    }

        //    worksheet.Cells[dRow, 3, dRow, 4].Value = "วันที่รับสินค้า";
        //    using (var range = worksheet.Cells[dRow, 3, dRow, 4])
        //    {
        //        range.Merge = true;
        //        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    }

        //    worksheet.Cells[dRow, 5, dRow, 6].Value = "วันที่นับสินค้า";
        //    using (var range = worksheet.Cells[dRow, 5, dRow, 6])
        //    {
        //        range.Merge = true;
        //        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    }
        //    dRow++;

        //    worksheet.Cells[dRow, 1, dRow, 2].Value = "(………………………………………)";
        //    using (var range = worksheet.Cells[dRow, 1, dRow, 2])
        //    {
        //        range.Merge = true;
        //        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    }

        //    worksheet.Cells[dRow, 3, dRow, 4].Value = "(……………………………………………………………)";
        //    using (var range = worksheet.Cells[dRow, 3, dRow, 4])
        //    {
        //        range.Merge = true;
        //        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    }

        //    worksheet.Cells[dRow, 5, dRow, 6].Value = "(………………………………………)";
        //    using (var range = worksheet.Cells[dRow, 5, dRow, 6])
        //    {
        //        range.Merge = true;
        //        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    }
        //    dRow++;
        //    dRow++;

        //    //ตำแหน่ง, เวลา
        //    worksheet.Cells[dRow, 1, dRow, 2].Value = "ตำแหน่ง";
        //    using (var range = worksheet.Cells[dRow, 1, dRow, 2])
        //    {
        //        range.Merge = true;
        //        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    }

        //    worksheet.Cells[dRow, 3, dRow, 4].Value = "เวลา";
        //    using (var range = worksheet.Cells[dRow, 3, dRow, 4])
        //    {
        //        range.Merge = true;
        //        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    }
        //    dRow++;

        //    worksheet.Cells[dRow, 1, dRow, 2].Value = "(………………………………………)";
        //    using (var range = worksheet.Cells[dRow, 1, dRow, 2])
        //    {
        //        range.Merge = true;
        //        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    }

        //    worksheet.Cells[dRow, 3, dRow, 4].Value = "(……………………………………………………………)";
        //    using (var range = worksheet.Cells[dRow, 3, dRow, 4])
        //    {
        //        range.Merge = true;
        //        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    }
        //    #endregion

        //    #region Set All Font
        //    using (var rage = worksheet.Cells[worksheet.Dimension.Start.Row,
        //        worksheet.Dimension.Start.Column,
        //        worksheet.Dimension.End.Row,
        //        worksheet.Dimension.End.Column])
        //    {
        //        rage.Style.Font.Name = "Tahoma";
        //    }
        //    #endregion

        //    #region Autofit all Column
        //    //Make all text fit the cells
        //    //worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        //    //worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns(autofitMinimumSize);
        //    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns(autofitMinimumSize, autofitMaximumSize);

        //    //for (int col = 1; col < worksheet.Dimension.End.Column; col++)
        //    //{
        //    //    worksheet.Column(col).AutoFit();
        //    //    //worksheet.Column(col).Width = worksheet.Column(col).Width + 2;

        //    //    //wrap text in the cells
        //    //    //if (col == 13)
        //    //    //{
        //    //    //    worksheet.Column(13).Style.WrapText = true;
        //    //    //}
        //    //}

        //    //worksheet.Cells[1, 6].AutoFitColumns();
        //    //worksheet.Cells[2, 6].AutoFitColumns();
        //    //worksheet.Cells[1, 6, 2, 6].AutoFitColumns();

        //    //Adjust Column พนักงานแพ็ค Width Manually
        //    worksheet.Column(6).Width = 15;
        //    #endregion

        //    #region Assign Excel
        //    result = package.GetAsByteArray();
        //    #endregion

        //}
        //#endregion

        //return File(result, "application/ms-excel", $"{fName}");

        var resReportData = await _reportAPI.GetInventoryTransferByDraftReportAsync(new InventoryTransferReportByDraftIDQuery
        {
            transferid = draftID
        });
        if (!resReportData.result)
        {
            throw new Exception(resReportData.error.error.message);
        }

        var resExcepReport = await _excelAPI.GenerateInventoryTransferExcelAsync(new GenerateStockTransferExcelReportQuery
        {
            draftid = draftID,
            reportdata = resReportData.data
        });
        if (!resExcepReport.result)
        {
            throw new Exception(resExcepReport.error.error.message);
        }
        return File(resExcepReport.data.filebyte, "application/ms-excel", $"{resExcepReport.data.filename}");

    }
    #endregion
}
