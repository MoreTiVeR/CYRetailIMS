using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleBarcodeReport.v1;

[Serializable]
public class SaleBarcodeReportResponseDTO
{
 public DateTime transactiondate { get; set; }
 public string monthyear { get; set; }
 public string branchname { get; set; }
 public string username { get; set; }
 public decimal amountcash { get; set; }
 public decimal amounttransfer { get; set; }
 public decimal substitutefee { get; set; }
 public decimal depositfee { get; set; }
 public decimal otherfee { get; set; }
 public decimal totalamount { get; set; }
 public decimal vat { get; set; }
 public decimal discount { get; set; }
 public string othernote { get; set; }
 public string status { get; set; }
 public string auditorname { get; set; }
 public string referenceno { get; set; }
}
