namespace CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockApprovalReport.v1;

public class GetCountStockApprovalReportResponseDTO
{
    public int totalrow { get; set; }
    public List<GetCountStockApprovalReportItemDTO> transactiondata { get; set; } = new();
}

public class GetCountStockApprovalReportItemDTO
{
    public int countstockid { get; set; }
    public DateTime countstockdate { get; set; }
    public int branchid { get; set; }
    public string branchname { get; set; } = string.Empty;
    public string counterrole { get; set; } = "PC";
    public string approvedby { get; set; } = string.Empty;
    public DateTime approveddate { get; set; }
    public int totalitems { get; set; }
    public int totalqtybefore { get; set; }
    public int totalqtyafter { get; set; }
    public int totaladjustedqty { get; set; }
}
