namespace CYRetailIMS.Application.Common.Models.UI;

/// <summary>
/// Search model for pending approvals page
/// </summary>
public class SearchPendingApprovalViewModel
{
    public string? counterrole { get; set; }
    public int? statuscid { get; set; }
    public int draw { get; set; }
    public int start { get; set; }
    public int length { get; set; }
    public string? searchValue { get; set; }
}

/// <summary>
/// Search model for count stock comparison page
/// </summary>
public class SearchCountStockComparisonViewModel
{
    public int branchid { get; set; }
    public string? subitemtypename { get; set; }
    public string? salesstartdate { get; set; }
    public string? salesenddate { get; set; }
    public string? auditstartdate { get; set; }
    public string? auditenddate { get; set; }
    public int draw { get; set; }
    public int start { get; set; }
    public int length { get; set; }
    public string? searchValue { get; set; }
}

/// <summary>
/// Model for submitting count stock
/// </summary>
public class SubmitCountStockViewModel
{
    public int CountStockID { get; set; }
}

/// <summary>
/// Model for approving count stock
/// </summary>
public class ApproveCountStockViewModel
{
    public int CountStockID { get; set; }
}

/// <summary>
/// Search model for count stock approval report (index)
/// </summary>
public class SearchCountStockApprovalReportViewModel
{
    public int? branchid { get; set; }
    public string? startdate { get; set; }
    public string? enddate { get; set; }
    public int draw { get; set; }
    public int start { get; set; }
    public int length { get; set; }
    public string? searchValue { get; set; }
}
