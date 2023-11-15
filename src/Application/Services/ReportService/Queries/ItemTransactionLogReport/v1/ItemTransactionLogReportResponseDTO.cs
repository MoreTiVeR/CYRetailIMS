using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ReportService.Queries.ItemTransactionLogReport.v1;

[Serializable]
public class ItemTransactionLogReportResponseDTO
{
    public int seq { get; set; }
    public int itemid { get; set; }

    public string itemcode { get; set; }

    public string itemname { get; set; }

    public int branchid { get; set; }

    public string branchname { get; set; }

    //public decimal cost { get; set; }

    public int itemtypeid { get; set; }

    public string itemtypename { get; set; }

    public int brandid { get; set; }

    public string brandname { get; set; }

    //public string brandshortname { get; set; }

    public string description { get; set; }

    public decimal price { get; set; }

    public decimal oldprice { get; set; }

    public decimal? newprice { get; set; }

    //public double? discountpercent { get; set; }

    public int? qty { get; set; }

    //public bool isactive { get; set; }

    //public int approvestatus { get; set; }

    public string barcode { get; set; }

    public int notifyminqty { get; set; }

    public int? notifymaxqty { get; set; }

    public string createdby { get; set; }

    public DateTime createddate { get; set; }

    public string? updatedby { get; set; }

    public DateTime? updateddate { get; set; }
}
