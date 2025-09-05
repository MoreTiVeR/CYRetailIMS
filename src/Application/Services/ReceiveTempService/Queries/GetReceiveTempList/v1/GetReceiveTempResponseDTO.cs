using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempList.v1;
public class GetReceiveTempResponseDTO
{
    public int receivetempid { get; set; }
    public int branchid { get; set; }
    public string branchname { get; set; }
    public string shopheadernametext { get; set; }
    public string shopheaderaddresstext { get; set; }
    public string? additionalheadertext { get; set; }
    public string? shopfootertext { get; set; }
    public string? additionalfootertext { get; set; }
    public string telephoneno { get; set; }
    public string createdby { get; set; }
    public DateTime createddate { get; set; }
    public string? updatedby { get; set; }
    public DateTime? updateddate { get; set; }
    public bool isactive { get; set; }
}
