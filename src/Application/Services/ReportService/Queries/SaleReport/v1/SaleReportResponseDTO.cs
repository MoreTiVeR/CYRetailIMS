using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;

[Serializable]
public class SaleReportResponseDTO
{
    public int transactionid { get; set; }
    public DateTime transactiondate { get; set; }
    public string itemcode { get; set; }
    public string itemname { get; set; }
    public int brandid { get; set; }
    public string brandname { get; set; }
    public int qty { get; set; }
    public decimal unitprice { get; set; }

    private decimal _amount { get; set; }

	public decimal amount
    {
        get
        {
            if(_amount == 0)
            {
                return qty * unitprice;
            }
            return _amount;
        }
        set => _amount = value;
	}

    public decimal amounttransfer { get; set; }
	public decimal amountdeposit { get; set; }
	public decimal depositfee { get; set; }
	public decimal amountcash{ get; set; }
	public decimal totalamount { get; set; }
	public int branchid { get; set; }
    public string branchname { get; set; }
    public DateTime createddate { get; set; }
    public string createdby { get; set; }
    public string createdbystaff { get; set; }
}
