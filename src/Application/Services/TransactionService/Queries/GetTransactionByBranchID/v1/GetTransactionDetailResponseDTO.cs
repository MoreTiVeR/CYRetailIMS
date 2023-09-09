using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;

[Serializable]
public class GetTransactionDetailResponseDTO
{
	public int transactiondetailid { get; set; }

	public int itemid { get; set; }

	public string itemname { get; set; }

	public decimal price { get; set; }

	public int qty { get; set; }

	public decimal amount { get; set; }

	public bool isactive { get; set; }
}
