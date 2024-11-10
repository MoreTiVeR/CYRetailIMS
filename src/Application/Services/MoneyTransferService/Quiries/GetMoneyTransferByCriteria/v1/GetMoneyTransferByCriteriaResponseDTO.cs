using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.MoneyTransferService.Quiries.GetMoneyTransferByCriteria.v1;
public class GetMoneyTransferByCriteriaResponseDTO
{
    public int moneytransferid { get; set; }
    public int branchid { get; set; }
    public string branchname { get; set; }
    public decimal amounttransfer { get; set; }
    public DateTime transferdate { get; set; }
    public string description { get; set; }
    public string imgpath { get; set; }
    public string createdby { get; set; }
    public DateTime createddate { get; set; }
    public bool isactive { get; set; }
}
