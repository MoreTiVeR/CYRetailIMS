using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;

[Serializable]
public class GetTransactionByBranchIDResponseDTO
{
    public int transactionid { get; set; }
    public DateTime transactiondate { get; set; }

	#region Branch
	public int branchid { get; set; }
	public string branchname { get; set; }
	#endregion

	#region Transaction Type
	public int transactiontypeid { get; set; }
	public string transactiontypename { get; set; }
	public string transactiontypedesc { get; set; }
	#endregion

	public decimal amounttransfer { get; set; }
	public decimal amountdeposit { get; set; }
	public decimal amountcash { get; set; }
	public decimal totalamount { get; set; }
    public decimal depositfee { get; set; }
    public string createdbystaff { get; set; }
    public string createdby { get; set; }
	public DateTime createddate { get; set; }
	public string updatedby { get; set; }
	public DateTime? updateddate { get; set; }
	public bool isactive { get; set; }
    public string remark { get; set; }
    public List<GetTransactionDetailResponseDTO> detail { get; set; }
}
