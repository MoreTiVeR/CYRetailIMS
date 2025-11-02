using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.ExternalService.ReceiptTempAPI;
using CYRetailIMS.Application.Services.ItemService.Commands.UpdateItem;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.CreateReceiveTemplate.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.DeleteReceiveTemplate.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.GenerateReceiptNo.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.UpdateReceiveTemplate.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempByBranchID.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempByCriteria.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempByID.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempList.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using CYRetailIMS.Infrastructure.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NetTopologySuite.Index.HPRtree;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin)]
public class ReceiptController : BaseController
{
    private readonly IReceiptTempAPI _receiptTempAPI;
    private readonly IBranchAPI _branchAPI;
    private readonly ICompositeViewEngine _viewEngine;
    public ReceiptController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IReceiptTempAPI receiptTempAPI,
        IBranchAPI branchAPI, ICompositeViewEngine viewEngine) : base(httpClientRequest, mapper, log)
    {
        _receiptTempAPI = receiptTempAPI;
        _branchAPI = branchAPI;
        _viewEngine = viewEngine;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        return View();
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        return View();
    }

    public async Task<IActionResult> Edit(int receivetempid)
    {
        BaseResponse<GetReceiveTempResponseDTO> rTemplate = await _receiptTempAPI.GetReceiptTemplateByIDAsync(new GetReceiveTempByIDQuery
        {
            tempreceiveid = receivetempid
        });
        EditReceiptTemplateViewModel vModel = MappingEditModel(rTemplate.data);
        return View(vModel);
    }

    [HttpPost]
    public async Task<IActionResult> SearchReceipt([FromBody] SearchReceiptTemplateViewModel searchItem)
    {
        List<GetReceiveTempResponseDTO> resReceiptList = new List<GetReceiveTempResponseDTO>();
        try
        {
            int? branchID = null;
            branchID = searchItem.branchid == 999 || searchItem.branchid == 0 ? null : searchItem.branchid;
            BaseResponse<GetReceiveTempByCriteriaResponseDTO> resReport = await _receiptTempAPI.GetReceiptTemplateByCriteriaAsync(new GetReceiveTempByCriteriaQuery
            {
                branchid = branchID,
                startrow = searchItem.start,
                pagesize = searchItem.length,
                searchvalue = searchItem.searchValue.Replace("\t", "").Replace("\n", ""),
                isexportalldata = searchItem.isexportalldata,
            });

            if (!resReport.result)
            {
                return Json(new { data = new List<GetReceiveTempResponseDTO>(), recordsTotal = 0, recordsFiltered = 0 });
            }

            #region Search Filter
            //if (!string.IsNullOrEmpty(searchItem.searchValue))
            //{
            //    string searchValue = searchItem.searchValue.Replace("\t", "").Replace("\n", "");

            //    resReport.data.transactiondata = resReport.data.transactiondata.Where(w => w.itemname.Contains(searchValue)
            //    || w.itemcode.Contains(searchValue)
            //    || w.branchname.Contains(searchValue)
            //    || w.brandname.Contains(searchValue)
            //    || w.createdby.Contains(searchValue)).ToList();
            //}
            #endregion

            var totalItems = resReport.data.totalrow; // Get total item count for pagination

            // Filter based on searchValue if necessary
            var query = resReport.data.receipttemplates;

            // Prepare response for DataTables
            return Json(new
            {
                draw = searchItem.draw, // Echo the draw parameter
                recordsTotal = totalItems, // Total records before filtering
                recordsFiltered = totalItems, // Total records after applying filtering
                data = resReport.data.receipttemplates // The actual data to be displayed
            });
        }
        catch
        {
            return Json(new { data = new List<GetReceiveTempResponseDTO>(), recordsTotal = 0, recordsFiltered = 0 });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateReceiptTemplate([FromBody] CreateReceiptTemplateViewModel createReceipt)
    {
        try
        {
            CreateReceiveTemplateCommand createReceiveCmd = PrepareCreateReceiptCommand(createReceipt);
            BaseResponse<CommandResponse> resCreate = await _receiptTempAPI.CreateBranchAsync(createReceiveCmd);
            return Json(new JsonViewModel { result = resCreate.result, message = resCreate.result ? resCreate.message : resCreate.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditReceiptTemplate([FromBody] EditReceiptTemplateViewModel editReceipt)
    {
        try
        {
            UpdateReceiveTemplateCommand updateReceiptTempCommand = PrepareUpdateReceiptCommand(editReceipt);
            BaseResponse<CommandResponse> resUpdateReceiptTemp = await _receiptTempAPI.UpdateBranchAsync(updateReceiptTempCommand);
            return Json(new JsonViewModel { result = resUpdateReceiptTemp.result, message = resUpdateReceiptTemp.result ? resUpdateReceiptTemp.message : resUpdateReceiptTemp.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }


    [HttpPost]
    public async Task<IActionResult> DeleteReceiptByID([FromBody] DeleteReceiptTemplateViewModel deleteReceiptTemp)
    {
        try
        {
            DeleteReceiveTemplateCommand delItemCommand = new DeleteReceiveTemplateCommand { receivetemplateid = deleteReceiptTemp.receipttempid, updatedby = base.UserProfile.username };
            BaseResponse<CommandResponse> resDelete = await _receiptTempAPI.DeleteBranchAsync(delItemCommand);
            if (resDelete.result)
            {
                return Json(new JsonViewModel { result = resDelete.result, message = resDelete.message });
            }
            return Json(new JsonViewModel { result = resDelete.result, message = resDelete.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }


    [HttpPost]
    public IActionResult GenerateReceiptText([FromBody] CreateReceiptTemplateViewModel model)
    {
        try
        {
            int width = 56;
            var sb = new StringBuilder();

            // Header
            sb.AppendLine(CenterText(model.shopheadernametext, width));
            sb.AppendLine(CenterText(model.shopheaderaddresstext, width));
            if (!string.IsNullOrEmpty(model.telephoneno))
                sb.AppendLine(CenterText(model.telephoneno, width));
            sb.AppendLine(new string('-', width));
            sb.AppendLine($"Invoice: CY001-001-{DateTime.Now:ddMMyyyy}");
            sb.AppendLine($"Date: {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine(new string('-', width));

            // Items
            sb.AppendLine(string.Format("{0,-28}{1,4}{2,12}{3,12}", "Item", "Qty", "Price", "Total"));
            sb.AppendLine(new string('-', width));
            List<SellingBarcodeItemViewModel> tempSellingBarcodeItemList = new List<SellingBarcodeItemViewModel>();
            tempSellingBarcodeItemList.Add(new SellingBarcodeItemViewModel
            {
                itemname = "CASE IP 6/I6S",
                qty = 5,
                itemprice = 100.00m,
                totalprice = 3 * 100.00m
            });
            tempSellingBarcodeItemList.Add(new SellingBarcodeItemViewModel
            {
                itemname = "VI Y17 / Y11 / Y12 / Y15 / Y12i / Y3S ฟิล์มเต็มจอ",
                qty = 2,
                itemprice = 150.00m,
                totalprice = 2 * 150.00m
            });
            foreach (var item in tempSellingBarcodeItemList)
            {
                string name = TruncateItemName(item.itemname, 28);
                sb.AppendLine(string.Format("{0,-28}{1,4}{2,12:N2}{3,12:N2}", name, item.qty, item.itemprice, item.totalprice));
            }
            sb.AppendLine(new string('-', width));

            // Totals
            // คำนวณ
            bool isVatInclusive = true; // หรือ true ถ้าราคาในระบบรวม VAT มาแล้ว

            decimal subTotal = tempSellingBarcodeItemList.Sum(s => s.totalprice);
            decimal discount = 0.0m;
            decimal shipping = 0.0m;
            //decimal vat = subTotal.ToVat();
            //decimal totalBill = subTotal - discount + shipping + vat;
            //inc vat VAT รวมในสินค้าแล้ว ไม่ต้องตำนวนบวกเพิ่ม
            //decimal totalBill = subTotal - discount + shipping;
            decimal due = 0.0m;

            ///subTotalExcludeVat ยอดก่อน vat
            ///vat ภาษี
            ///totalBill หลัง vat
            var (subTotalExcludeVat, vat, totalBill) = VatHelper.CalculateTotal(subTotal, discount, shipping, isVatInclusive);
            sb.AppendLine(string.Format("{0,-43}{1,13:N2}", "ราคารวม :", subTotalExcludeVat));
            //sb.AppendLine(string.Format("{0,-44}{1,12:N2}", "ส่วนลด :", discount));
            sb.AppendLine(string.Format("{0,-43}{1,12:N2}", "VAT(7%) :", vat));
            sb.AppendLine(string.Format("{0,-43}{1,14:N2}", "ยอดรวมทั้งหมด :", totalBill));
            sb.AppendLine(new string('-', width));

            // Footer
            if (!string.IsNullOrEmpty(model.shopfootertext))
                sb.AppendLine(CenterText(model.shopfootertext, width));
            if (!string.IsNullOrEmpty(model.additionalfootertext))
                sb.AppendLine(CenterText(model.additionalfootertext, width));
            sb.AppendLine(CenterText("THANK YOU!", width));

            return Json(new { result = true, text = sb.ToString() });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }

    }

    [HttpPost]
    public IActionResult GenerateReceiveSlipText([FromBody] CreateReceiptTemplateViewModel receiptTemplateViewModel)
    {

        #region Generate Receipt Number by branch code
        string currentReceiptNo = $"CY000-001-{DateTime.Now:ddMMyyyyHHmm}";
        #endregion

        #region Mock Item
        List<string> names = new List<string> { "สินค้า 1", "สินค้า 2" };
        List<int> qtys = new List<int> { 2, 3 };
        List<decimal> prices = new List<decimal> { 150.00m, 75.50m };
        List<SellingBarcodeItemViewModel> tempSellingBarcodeItemList = new List<SellingBarcodeItemViewModel>();
        tempSellingBarcodeItemList.Add(new SellingBarcodeItemViewModel
        {
            itemname = "CASE IP 6/I6S",
            qty = 3,
            itemprice = 100.00m,
            totalprice = 3 * 100.00m
        });
        tempSellingBarcodeItemList.Add(new SellingBarcodeItemViewModel
        {
            itemname = "VI Y17 / Y11 / Y12 / Y15 / Y12i / Y3S ฟิล์มเต็มจอ",
            qty = 2,
            itemprice = 150.00m,
            totalprice = 2 * 150.00m
        });
        #endregion
        // คำนวณ
        bool isVatInclusive = true; // หรือ true ถ้าราคาในระบบรวม VAT มาแล้ว

        decimal subTotal = tempSellingBarcodeItemList.Sum(s => s.totalprice);
        decimal discount = 0.0m;
        decimal shipping = 0.0m;
        //decimal vat = subTotal.ToVat();
        //decimal totalBill = subTotal - discount + shipping + vat;
        //inc vat VAT รวมในสินค้าแล้ว ไม่ต้องตำนวนบวกเพิ่ม
        //decimal totalBill = subTotal - discount + shipping;
        decimal due = 0.0m;

        ///subTotalExcludeVat ยอดก่อน vat
        ///vat ภาษี
        ///totalBill หลัง vat
        var (subTotalExcludeVat, vat, totalBill) = VatHelper.CalculateTotal(subTotal, discount, shipping, isVatInclusive);
        var model = new ReceiptViewModel
        {
            InvoiceNo = currentReceiptNo,
            CompanyName = receiptTemplateViewModel.shopheadernametext,
            CompanyAddress = receiptTemplateViewModel.shopheaderaddresstext,
            AdditionalHeaderText = receiptTemplateViewModel.additionalheadertext,
            TelephoneNo = receiptTemplateViewModel.telephoneno,
            ShopFooterText = receiptTemplateViewModel.shopfootertext,
            AdditionalFooterText = receiptTemplateViewModel.additionalfootertext,
            Items = tempSellingBarcodeItemList,
            SubTotal = subTotalExcludeVat,
            Discount = discount,
            Shipping = shipping,
            Vat = vat,
            TotalBill = totalBill,
            Due = due,
            Date = DateTime.Now
        };

        // Text Mode
        string recetiveText = GenerateReceiptEscPosV2(model);

        // return PartialView เป็น string
        //string html = RenderPartialViewToString("_ReceiptModal", model);
        return Json(new { result = true, text = recetiveText });
    }

    #region Private
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
        //sb.Append("\x1B\x4D\x00"); // Initialize Font A (default)

        // ✅ เลือก Code Page ไทย (กรณีเครื่อง map CP874 ไว้ที่ 18)
        //sb.Append("\x1B\x74\x12");

        // ✅ เลือก Code Page ไทย (กรณีเครื่อง map CP874 ไว้ที่ 255) not working BUG? use .setCharacterCodeTable(255) in js script
        //sb.Append("\x1B\x74\xFF");
        //sb.Append("\x1B\x74\xFF");

        //sb.Append("\x1B\x4D\x01"); // Initialize Font B
        sb.AppendLine();
        //sb.Append("\x1B\x61\x01"); // Center align

        // ===== Header =====
        sb.AppendLine(model.CompanyName);
        sb.AppendLine(model.CompanyAddress);
        if (!string.IsNullOrEmpty(model.TelephoneNo))
            sb.AppendLine(model.TelephoneNo);
        sb.AppendLine(new string('-', 48));

        // ===== Invoice & Date =====
        //sb.Append("\x1B\x61\x00"); // Left align
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

        sb.AppendLine($"{PadTextRightEscPos("ราคารวม :", 40 - 9)}{PadTextLeftEscPos(model.SubTotal.ToString("N2"), (48 - (40 + model.SubTotal.ToString("N2").Length)))}");
        //sb.AppendLine($"{PadTextRightEscPos("ส่วนลด :", 40-8)}{PadTextLeftEscPos(model.Discount.ToString("N2"), (48 - (40 + model.Discount.ToString("N2").Length)) + 1)}");
        sb.AppendLine($"{PadTextRightEscPos("VAT(7%) :", 40 - 9)}{PadTextLeftEscPos(model.Vat.ToString("N2"), (48 - (40 + model.Vat.ToString("N2").Length)))}");
        sb.AppendLine($"{PadTextRightEscPos("ยอดรวมทั้งหมด :", 40 - 15)}{PadTextLeftEscPos(model.TotalBill.ToString("N2"), (48 - (40 + model.TotalBill.ToString("N2").Length)) + 2)}");


        //sb.AppendLine($"{PadRightEscPos("ราคารวม :", GetDisplayWidth("ราคารวม :"))}{PadLeftEscPos(model.SubTotal.ToString("N2"), model.SubTotal.ToString("N2").Length)}");
        //sb.AppendLine($"{PadRightEscPos("ส่วนลด :", GetDisplayWidth("ส่วนลด :"))}{PadLeftEscPos(model.Discount.ToString("N2"), model.Discount.ToString("N2").Length)}");
        //sb.AppendLine($"{PadRightEscPos("VAT(7%) :", GetDisplayWidth("VAT(7%) :"))}{PadLeftEscPos(model.Vat.ToString("N2"), model.Vat.ToString("N2").Length)}");
        //sb.AppendLine($"{PadRightEscPos("ยอดรวมทั้งหมด :", GetDisplayWidth("ยอดรวมทั้งหมด :"))}{PadLeftEscPos(model.TotalBill.ToString("N2"), model.TotalBill.ToString("N2").Length)}");


        sb.AppendLine(new string('-', 48));

        // ===== Footer =====
        //sb.Append("\x1B\x61\x01"); // Center align
        if (!string.IsNullOrEmpty(model.ShopFooterText))
            sb.AppendLine(model.ShopFooterText);
        if (!string.IsNullOrEmpty(model.AdditionalFooterText))
            sb.AppendLine(model.AdditionalFooterText);
        sb.AppendLine("THANK YOU!");
        sb.AppendLine();

        // Cut paper
        //sb.Append("\x1D\x56\x41"); // Partial cut
        return sb.ToString();
    }

    private bool IsAscii(string text)
    {
        foreach (char c in text)
        {
            if (c > 127) return false; // not ASCII
        }
        return true;
    }

    private string PadRightEscPos(string text, int width) => PadEscPos(text, width, Encoding.GetEncoding("TIS-620"), right: false);

    private string PadLeftEscPos(string text, int width) => PadEscPos(text, width, Encoding.GetEncoding("TIS-620"), right: true);

    private string PadTextRightEscPos(string text, int width) => PadTextEscPos(text, width, Encoding.GetEncoding("TIS-620"), right: false);

    private string PadTextLeftEscPos(string text, int width) => PadTextEscPos(text, width, Encoding.GetEncoding("TIS-620"), right: true);

    private string PadItemNameRightEscPos(string text, int width) => PadItemTextEscPos(text, width, Encoding.GetEncoding("TIS-620"), right: false);

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
    private int GetDisplayWidth(string text)
    {
        int width = 0;

        foreach (char c in text)
        {
            width += (c < 128) ? 1 : 2;
        }

        return width;
    }

    private async Task<List<SelectListItem>> PrepareSelectBranch()
    {
        var resBranch = await _branchAPI.GetBranchListAsync();
        return resBranch.data.Select(s => new SelectListItem { Text = s.branchname, Value = s.branchid.ToString() }).ToList();
    }

    private EditReceiptTemplateViewModel MappingEditModel(GetReceiveTempResponseDTO getReceiveTempResponseDTO)
    {
        return new EditReceiptTemplateViewModel
        {
            receivetempid = getReceiveTempResponseDTO.receivetempid,
            branchid = getReceiveTempResponseDTO.branchid,
            //branchcode = getReceiveTempResponseDTO.branchcode,
            //branchname = getReceiveTempResponseDTO.branchname,
            shopheadernametext = getReceiveTempResponseDTO.shopheadernametext,
            shopheaderaddresstext = getReceiveTempResponseDTO.shopheaderaddresstext,
            shopfootertext = getReceiveTempResponseDTO.shopfootertext,
            additionalfootertext = getReceiveTempResponseDTO.additionalfootertext,
            telephoneno = getReceiveTempResponseDTO.telephoneno,
            printername = getReceiveTempResponseDTO.printername,
            updatedby = base.UserProfile.username,
            isactive = getReceiveTempResponseDTO.isactive.ToString()
        };
    }

    private CreateReceiveTemplateCommand PrepareCreateReceiptCommand(CreateReceiptTemplateViewModel createReceipt)
    {
        return new CreateReceiveTemplateCommand
        {
            branchid = createReceipt.branchid.ToInt32(),
            shopheadernametext = createReceipt.shopheadernametext,
            shopheaderaddresstext = createReceipt.shopheaderaddresstext,
            shopfootertext = createReceipt.shopfootertext,
            additionalfootertext = createReceipt.additionalfootertext,
            telephoneno = createReceipt.telephoneno,
            printername = createReceipt.printername,
            createdby = base.UserProfile.username
        };
    }

    private UpdateReceiveTemplateCommand PrepareUpdateReceiptCommand(EditReceiptTemplateViewModel editReceipt)
    {
        return new UpdateReceiveTemplateCommand
        {
            receivetemplateid = editReceipt.receivetempid,
            branchid = editReceipt.branchid,
            shopheadernametext = editReceipt.shopheadernametext,
            shopheaderaddresstext = editReceipt.shopheaderaddresstext,
            shopfootertext = editReceipt.shopfootertext,
            additionalfootertext = editReceipt.additionalfootertext,
            telephoneno = editReceipt.telephoneno,
            printername = editReceipt.printername,
            updatedby = base.UserProfile.username,
            isactive = editReceipt.isactive.ToBoolFromIntString()
        };
    }

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

    private string CenterText(string text, int width = 56)
    {
        if (string.IsNullOrEmpty(text)) return "";
        if (text.Length >= width) return text.Substring(0, width);
        int pad = (width - text.Length) / 2;
        return new string(' ', pad) + text;
    }

    private string TruncateItemName(string name, int maxLength = 28)
    {
        if (string.IsNullOrEmpty(name)) return "";
        if (name.Length <= maxLength) return name;
        return name.Substring(0, maxLength - 3) + "...";
    }
    #endregion
}
