namespace CYRetailIMS.Application.Services.CountStockService.Queries.GetPendingApprovals.v1;

/// <summary>
/// DTO for pending approval list (หน้ารออนุมัติ)
/// </summary>
public class GetPendingApprovalsResponseDTO
{
    public int countstockid { get; set; }
    public DateTime countstockdate { get; set; }
    public int branchid { get; set; }
    public string branchname { get; set; }

    /// <summary>
    /// บทบาทผู้นับ: "PC" หรือ "HeadPC"
    /// </summary>
    public string counterrole { get; set; }

    /// <summary>
    /// true = รายการจาก NewCountStockEntry, false = รายการนับสต๊อกแบบเก่า
    /// </summary>
    public bool isnewentry { get; set; }

    /// <summary>
    /// ชนิดการนับ: NewCountStockEntry หรือ LegacyCountStock
    /// </summary>
    public string countstockentrytype => isnewentry ? "NewCountStockEntry" : "LegacyCountStock";

    /// <summary>
    /// ชื่อผู้นับ
    /// </summary>
    public string createdby { get; set; }

    /// <summary>
    /// สถานะ: 0=Draft, 1=รออนุมัติ, 2=อนุมัติแล้ว
    /// </summary>
    public int counterstockstatusid { get; set; }

    public string counterstockstatusname => counterstockstatusid switch
    {
        0 => "แบบร่าง",
        1 => "รออนุมัติ",
        2 => "อนุมัติแล้ว",
        _ => "ไม่ทราบสถานะ"
    };

    /// <summary>
    /// จำนวนวันที่รออนุมัติ (นับจากวันที่ส่ง)
    /// </summary>
    public int waitingdays => (int)(DateTime.Now - countstockdate).TotalDays;

    public string? approvedby { get; set; }
    public DateTime? approveddate { get; set; }

    /// <summary>
    /// ลิงค์ดาวน์โหลด Excel (ถ้ามี)
    /// </summary>
    public string exceldownloadurl => $"/Stock/ExportCountStockExcel?countstockid={countstockid}";
}
