using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByBranchID.v1;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByID.v1;
public class InquiryCountStockByIDResponseDTO
{
    public int countstockid { get; set; }
    public int branchid { get; set; }
    public string branchname { get; set; }

    /// <summary>
    /// หมายเหตุ (ถ้ามี)
    /// </summary>
    public string remark { get; set; }
    public int totalcount { get; set; }
    public string createdby { get; set; }
    public DateTime countstockdate { get; set; }
    public List<InquiryCountStockByIDDetail> detail { get; set; }
}
