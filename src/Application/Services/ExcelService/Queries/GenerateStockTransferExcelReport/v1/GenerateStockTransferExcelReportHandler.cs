using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.ReportAPI;
using CYRetailIMS.Application.Services.ReportService.Queries.InventoryTransferByDraftID.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.InventoryTransferReportByDraftID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace CYRetailIMS.Application.Services.ExcelService.Queries.GenerateStockTransferExcelReport.v1;
public class GenerateStockTransferExcelReportHandler : BaseService,  IRequestHandler<GenerateStockTransferExcelReportQuery, BaseResponse<GenerateStockTransferExcelReportResponseDTO>>
{
    public GenerateStockTransferExcelReportHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GenerateStockTransferExcelReportResponseDTO>> Handle(GenerateStockTransferExcelReportQuery request, CancellationToken cancellationToken)
    {
        var resInvTransfer = await _unitOfWork.Repository<TTDraftItemTransfer>().FirstOrDefaultAsync(w => w.TransferHeaderID == request.draftid);
        if(resInvTransfer == null)
        {
            throw new Exception("ไม่พบข้อมูลการโอน");
        }
        int dRow = 1;
        int dCol = 1;
        string sheetName = DateTime.Now.ToString("dd-MM-yyyy");
        //Autofit with minimum size for the column.
        double autofitMinimumSize = 10;

        //Autofit with minimum and maximum size for the column.
        double autofitMaximumSize = 50;

        #region Get Report Data
        //var resReportData = await _reportAPI.GetInventoryTransferByDraftReportAsync(new InventoryTransferReportByDraftIDQuery
        //{
        //    transferid = request.draftid
        //});
        InventoryTransferReportByDraftIDResponseDTO resReportData = request.reportdata;
        string fName = $"รายงานโอนสินค้า_{resReportData.destinationbranchname}_{sheetName}.xlsx";
        int branchID = resReportData.destinationbranchid;
        string branchName = resReportData.destinationbranchname;
        string createdByName = resReportData.createdbyname;
        string refNo = resReportData.refno;
        string createdDate = $"{resReportData.createddate.ToString("dd/MM/yyyy HH:mm", new System.Globalization.CultureInfo("en-US"))}";
        int totalTransferQty = resReportData.totaltransferqty;
        #endregion

        #region Generate Excel
        System.Drawing.Color orangeColord = System.Drawing.ColorTranslator.FromHtml("");
        System.Drawing.Color orangeColor = System.Drawing.ColorTranslator.FromHtml("#ffc336");
        System.Drawing.Color blueColor = ColorTranslator.FromHtml("#66a2fb");
        System.Drawing.Color lightBlueColor = System.Drawing.ColorTranslator.FromHtml("#c0d7f9");
        System.Drawing.Color yellowColor = System.Drawing.ColorTranslator.FromHtml("#ebf916");
        System.Drawing.Color grayColor = System.Drawing.ColorTranslator.FromHtml("#b9b4b4");
        System.Drawing.Color lightGrayColor = System.Drawing.ColorTranslator.FromHtml("#e7e8ea");
        byte[] result;
        using (var package = new ExcelPackage())
        {
            //Add a new worksheet to the empty workbook
            var worksheet = package.Workbook.Worksheets.Add(fName);

            #region Branch/Employee Header
            worksheet.Cells[dRow, 3, dRow + 1, 3].Value = "ใบโอนสินค้า";
            worksheet.Cells[dRow, 3, dRow + 1, 3].Merge = true;
            worksheet.Cells[dRow, 3, dRow + 1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            using (var range = worksheet.Cells[dRow, 2, dRow + 1, 3])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(blueColor);
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                range.Style.Font.Size = 11;
            }

            worksheet.Cells[dRow, 4, dRow + 1, 4].Value = "เลขอ้างอิง2";
            worksheet.Cells[dRow, 4, dRow + 1, 4].Merge = true;
            worksheet.Cells[dRow, 4, dRow + 1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            using (var range = worksheet.Cells[dRow, 4, dRow + 1, 4])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(yellowColor);
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                range.Style.Font.Size = 11;
            }

            worksheet.Cells[dRow, 5, dRow + 1, 5].Value = "พนักงานยิง";
            worksheet.Cells[dRow, 5, dRow + 1, 5].Merge = true;
            worksheet.Cells[dRow, 5, dRow + 1, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            using (var range = worksheet.Cells[dRow, 5, dRow + 1, 5])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(yellowColor);
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                range.Style.Font.Size = 11;
            }

            worksheet.Cells[dRow, 6, dRow + 1, 6].Value = "พนักงานแพ็ค";
            worksheet.Cells[dRow, 6, dRow + 1, 6].Merge = true;
            worksheet.Cells[dRow, 6, dRow + 1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            using (var range = worksheet.Cells[dRow, 6, dRow + 1, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(yellowColor);
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                range.Style.Font.Size = 11;
            }

            dRow = dRow + 2;
            #endregion

            #region Branch/Employee detail
            //รหัสสาขา, วันที่สร้าง
            worksheet.Cells[dRow, 2].Value = branchID;
            worksheet.Cells[dRow, 2].Style.Font.Size = 14;
            worksheet.Cells[dRow + 1, 2].Value = createdDate;
            using (var range = worksheet.Cells[dRow, 2, dRow + 1, 2])
            {
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(lightBlueColor);
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }

            //ชื่อสาขา, รวมส่งออก text
            worksheet.Cells[dRow, 3].Value = branchName;
            worksheet.Cells[dRow, 3].Style.Font.Size = 14;
            worksheet.Cells[dRow, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[dRow + 1, 3].Value = "รวมส่งออก";
            worksheet.Cells[dRow + 1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            using (var range = worksheet.Cells[dRow, 3, dRow + 1, 3])
            {
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(lightBlueColor);
            }

            //เลขอ้างอิง2, จำนวนรวมส่งออก
            worksheet.Cells[dRow, 4].Value = refNo;
            //text format
            worksheet.Cells[dRow, 4].Style.Numberformat.Format = "@";
            worksheet.Cells[dRow, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[dRow + 1, 4].Value = totalTransferQty;
            worksheet.Cells[dRow + 1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            //พนักงานยิง
            worksheet.Cells[dRow, 5, dRow + 1, 5].Value = createdByName;
            worksheet.Cells[dRow, 5, dRow + 1, 5].Merge = true;
            worksheet.Cells[dRow, 5, dRow + 1, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[dRow, 5, dRow + 1, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

            //พนักงานแพ็ค
            worksheet.Cells[dRow, 6, dRow + 1, 6].Value = string.Empty;
            worksheet.Cells[dRow, 6, dRow + 1, 6].Merge = true;
            worksheet.Cells[dRow, 6, dRow + 1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[dRow, 6, dRow + 1, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

            dRow = dRow + 2;
            #endregion

            #region Transfer Item
            //Item Transfer Header
            worksheet.Cells[dRow, 1].Value = "ลำดับ";
            worksheet.Cells[dRow, 2].Value = "รหัสสินค้า";
            worksheet.Cells[dRow, 3].Value = "ชื่อสินค้า";
            worksheet.Cells[dRow, 4].Value = "จำนวนที่เติม";
            worksheet.Cells[dRow, 5].Value = "จำนวนรับสินค้า";
            worksheet.Cells[dRow, 6].Value = "ขาด/เกิน";

            //Set Item Transfer Header Style
            using (var range = worksheet.Cells[dRow, 1, dRow, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(grayColor);
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                range.Style.Font.Size = 10;
            }
            dRow++;

            int startHeaderItemRow = dRow;
            int endHeaderItemRow = 0;
            foreach (var item in resReportData.detail)
            {
                worksheet.Cells[dRow, 1].Value = item.seq;
                worksheet.Cells[dRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[dRow, 2].Value = item.itemcode;
                worksheet.Cells[dRow, 3].Value = item.itemname;
                worksheet.Cells[dRow, 4].Value = item.transferqty;
                worksheet.Cells[dRow, 5].Value = item.receiveqty <= 0 ? null : item.receiveqty;
                worksheet.Cells[dRow, 6].Value = item.excessqty <= 0 ? null : item.excessqty;
                using (var range = worksheet.Cells[dRow, 4, dRow, 6])
                {
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }
                dRow++;
            }

            //Row รวมทั้งหมด
            endHeaderItemRow = dRow;
            worksheet.Cells[dRow, 3].Value = "รวมทั้งหมด";
            worksheet.Cells[dRow, 3].Style.Font.Bold = true;
            worksheet.Cells[dRow, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            //worksheet.Cells[dRow, 4].Value = resReportData.data.totaltransferqty;
            worksheet.Cells[dRow, 4].Formula = $"SUM({worksheet.Cells[startHeaderItemRow, 4].Address}:{worksheet.Cells[endHeaderItemRow - 1, 4].Address})";
            worksheet.Cells[dRow, 4].Calculate();
            worksheet.Cells[dRow, 4].Style.Font.Bold = true;
            worksheet.Cells[dRow, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            dRow++;
            #endregion

            #region SubItem
            //Header SubItem
            worksheet.Cells[dRow, 1].Value = "ลำดับ";
            worksheet.Cells[dRow, 2].Value = "ประเภทฟิล์ม";
            worksheet.Cells[dRow, 3].Value = "จำนวนทำออก";

            //Set Header SubItem Style
            using (var range = worksheet.Cells[dRow, 1, dRow, 3])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(grayColor);
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                range.Style.Font.Size = 10;
            }
            dRow++;

            int startSubItemRow = dRow;
            int endSubItemRow = 0;
            foreach (var item in resReportData.subitemdetail)
            {
                worksheet.Cells[dRow, 1].Value = item.seq;
                worksheet.Cells[dRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[dRow, 2].Value = item.subitemtypename;
                worksheet.Cells[dRow, 3].Value = item.transferqty;
                worksheet.Cells[dRow, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                dRow++;
            }

            endSubItemRow = dRow;
            //Row รวมทั้งหมด
            worksheet.Cells[dRow, 2].Value = "จำนวนรวม";
            worksheet.Cells[dRow, 2].Style.Font.Bold = true;
            worksheet.Cells[dRow, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            worksheet.Cells[dRow, 3].Value = resReportData.totalsubitemtransferqty;
            worksheet.Cells[dRow, 3].Style.Font.Bold = true;
            worksheet.Cells[dRow, 3].Formula = $"SUM({worksheet.Cells[startSubItemRow, 3].Address}:{worksheet.Cells[endSubItemRow - 1, 3].Address})";
            worksheet.Cells[dRow, 3].Calculate();
            worksheet.Cells[dRow, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            #region Set fonesize for all item row
            using (var range = worksheet.Cells[startHeaderItemRow, 1, endSubItemRow, 6])
            {
                range.Style.Font.Size = 10;
            }
            #endregion

            dRow++;
            #endregion

            #region ท้ายตาราง ชื่อพนักงาน/ วันที่รับสินค้า/ วันที่นับสินค้า
            dRow++;
            //ชื่อพนักงาน, วันที่รับสินค้า, วันที่นับสินค้า
            worksheet.Cells[dRow, 1, dRow, 2].Value = "ชื่อพนักงาน";
            using (var range = worksheet.Cells[dRow, 1, dRow, 2])
            {
                range.Merge = true;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }

            worksheet.Cells[dRow, 3, dRow, 4].Value = "วันที่รับสินค้า";
            using (var range = worksheet.Cells[dRow, 3, dRow, 4])
            {
                range.Merge = true;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }

            worksheet.Cells[dRow, 5, dRow, 6].Value = "วันที่นับสินค้า";
            using (var range = worksheet.Cells[dRow, 5, dRow, 6])
            {
                range.Merge = true;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }
            dRow++;

            worksheet.Cells[dRow, 1, dRow, 2].Value = "(………………………………………)";
            using (var range = worksheet.Cells[dRow, 1, dRow, 2])
            {
                range.Merge = true;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }

            worksheet.Cells[dRow, 3, dRow, 4].Value = "(……………………………………………………………)";
            using (var range = worksheet.Cells[dRow, 3, dRow, 4])
            {
                range.Merge = true;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }

            worksheet.Cells[dRow, 5, dRow, 6].Value = "(………………………………………)";
            using (var range = worksheet.Cells[dRow, 5, dRow, 6])
            {
                range.Merge = true;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }
            dRow++;
            dRow++;

            //ตำแหน่ง, เวลา
            worksheet.Cells[dRow, 1, dRow, 2].Value = "ตำแหน่ง";
            using (var range = worksheet.Cells[dRow, 1, dRow, 2])
            {
                range.Merge = true;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }

            worksheet.Cells[dRow, 3, dRow, 4].Value = "เวลา";
            using (var range = worksheet.Cells[dRow, 3, dRow, 4])
            {
                range.Merge = true;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }
            dRow++;

            worksheet.Cells[dRow, 1, dRow, 2].Value = "(………………………………………)";
            using (var range = worksheet.Cells[dRow, 1, dRow, 2])
            {
                range.Merge = true;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }

            worksheet.Cells[dRow, 3, dRow, 4].Value = "(……………………………………………………………)";
            using (var range = worksheet.Cells[dRow, 3, dRow, 4])
            {
                range.Merge = true;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }
            #endregion

            #region Set All Font
            using (var rage = worksheet.Cells[worksheet.Dimension.Start.Row,
                worksheet.Dimension.Start.Column,
                worksheet.Dimension.End.Row,
                worksheet.Dimension.End.Column])
            {
                rage.Style.Font.Name = "Tahoma";
            }
            #endregion

            #region Autofit all Column
            //Make all text fit the cells
            //worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            //worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns(autofitMinimumSize);
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns(autofitMinimumSize, autofitMaximumSize);

            //for (int col = 1; col < worksheet.Dimension.End.Column; col++)
            //{
            //    worksheet.Column(col).AutoFit();
            //    //worksheet.Column(col).Width = worksheet.Column(col).Width + 2;

            //    //wrap text in the cells
            //    //if (col == 13)
            //    //{
            //    //    worksheet.Column(13).Style.WrapText = true;
            //    //}
            //}

            //worksheet.Cells[1, 6].AutoFitColumns();
            //worksheet.Cells[2, 6].AutoFitColumns();
            //worksheet.Cells[1, 6, 2, 6].AutoFitColumns();

            //Adjust Column พนักงานแพ็ค Width Manually
            worksheet.Column(6).Width = 15;
            #endregion

            #region Assign Excel
            result = package.GetAsByteArray();
            #endregion

        }
        #endregion
        return new BaseResponse<GenerateStockTransferExcelReportResponseDTO>
        {
            result = true,
            data = new GenerateStockTransferExcelReportResponseDTO
            {
                filename = fName,
                filebyte = result
            },
            soruce = "api",
            message = "Success",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
