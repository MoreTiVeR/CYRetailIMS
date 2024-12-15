using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.MoneyTransferSlipService.Quiries.GetSlipByMoneyTransferID.v1;
public class GetSlipByMoneyTransferIDResponseDTO
{
    public int moneytransferid { get; set; }
    public int? sliptransferid { get; set; }
    public decimal totalamounttransfer { get; set; }
    public string createdby { get; set; }
    public DateTime createddate { get; set; }
    public List<GetSlipByMoneyTransferIDDetailResponseDTO>? slipdetail { get; set; }
}
