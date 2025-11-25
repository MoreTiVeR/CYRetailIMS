using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleBarcodeReport.v1;

[Serializable]
public class SaleBarcodeReportResponseDetailDTO
{
    /// <summary>
    /// รหัสรายการ
    /// </summary>
    public int transactionid { get; set; }

    /// <summary>
    /// วันที่
    /// </summary>
    public DateTime transactiondate { get; set; }

    /// <summary>
    /// เดือน-ปี
    /// </summary>
    //public string monthyear { get; set; }

    /// <summary>
    /// รหัสสาขา
    /// </summary>
    public int branchid { get; set; }

    /// <summary>
    /// ชื่อสาขา
    /// </summary>
    public string branchname { get; set; }

    /// <summary>
    /// ชื่อพนักงาน
    /// </summary>
    public string username { get; set; }

    /// <summary>
    /// เงินสดฝาก
    /// </summary>
    public decimal amountcash { get; set; }

    /// <summary>
    /// เงินลูกค้าโอน
    /// </summary>
    public decimal amounttransfer { get; set; }

    /// <summary>
    /// ค่าแรงคนแทน
    /// </summary>
    public decimal substitutefee { get; set; }

    /// <summary>
    /// ค่าธรรมเนียม
    /// </summary>
    public decimal depositfee { get; set; }

    /// <summary>
    /// ค่าอื่นๆ
    /// </summary>
    public decimal otherfee { get; set; }

    /// <summary>
    /// ราคารวม
    /// </summary>
    public decimal totalamount { get; set; }

    /// <summary>
    /// VAT
    /// </summary>
    public decimal? vat { get; set; }

    /// <summary>
    /// ส่วนลด
    /// </summary>
    public decimal? discount { get; set; }

    /// <summary>
    /// หมายเหตุ
    /// </summary>
    public string? remark { get; set; }

    /// <summary>
    /// สถานะตรวจสอบ
    /// </summary>
    //public string status { get; set; }

    /// <summary>
    /// สถานะ IsActive ตาราง EndOfDaySummary
    /// </summary>
    public bool eodsummarystatus { get; set; }

    /// <summary>
    /// รหัสการตรวจสอบ
    /// </summary>
    public int auditid { get; set; }

    /// <summary>
    /// ผู้ตรวจสอบ
    /// </summary>
    public string auditorname { get; set; }

    /// <summary>
    /// เลขอ้างอิง
    /// </summary>
    public string referenceno { get; set; }
}
