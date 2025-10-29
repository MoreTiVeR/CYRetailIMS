using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class CreateReceiptTemplateViewModel
{
    public string branchid { get; init; }
    public string shopheadernametext { get; init; }
    public string shopheaderaddresstext { get; init; }
    public string? additionalheadertext { get; init; }
    public string? shopfootertext { get; init; }
    public string? additionalfootertext { get; init; }
    public string telephoneno { get; init; }
    public string printername { get; init; }
    public string isactive { get; init; }
    public string createdby { get; init; }
}
