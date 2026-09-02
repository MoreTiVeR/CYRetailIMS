namespace CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockApprovalReportByID.v1;

public class GetCountStockApprovalReportByIDResponseDTO
{
    public int countstockid { get; set; }
    public DateTime countstockdate { get; set; }
    public int branchid { get; set; }
    public string branchname { get; set; } = string.Empty;
    public string counterrole { get; set; } = "PC";
    public string approvedby { get; set; } = string.Empty;
    public DateTime approveddate { get; set; }
    public string? remark { get; set; }
    public int totalqtybefore { get; set; }
    public int totalqtyafter { get; set; }
    public int totaladjustedqty { get; set; }
    public List<GetCountStockApprovalReportByIDDetailDTO> detail { get; set; } = new();
}

public class GetCountStockApprovalReportByIDDetailDTO
{
    public int countstockapprovalhistoryid { get; set; }
    public int countstockdetailid { get; set; }
    public int itemid { get; set; }
    public string itemcode { get; set; } = string.Empty;
    public string itemname { get; set; } = string.Empty;
    public int subitemtypeid { get; set; }
    public string subitemcode { get; set; } = string.Empty;
    public int qtyinbranchofcountstockday { get; set; }
    public int qtyinbranchbeforeapprove { get; set; }
    public int qtyinbranchafterapprove { get; set; }
    public int countedamountqty { get; set; }
    public int pendingrestockqty { get; set; }
    public int damagedqty { get; set; }
    public int salebeforecountqty { get; set; }
    public int totalcountqty { get; set; }
    public int shortagesurplusqty { get; set; }
    public string? itemremark { get; set; }
    public int adjustedqty { get; set; }
}
