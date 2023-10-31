using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.AdjustItemTransactionService.Queries.GetAdjustItemTransactions.v1;

[Serializable]
public class GetAdjustItemTransactionsResponseDTO
{
    public int adjustid { get; set; }

    public int adjusttypeid { get; set; }

    public string adjusttypename { get; set; }

    public int branchid { get; set; }

    public int itemid { get; set; }

    public string itemcode { get; set; }

    public string itemname { get; set; }

    public int itemtypeid { get; set; }
    public string itemtypename { get; set; }

    public int itembrandid { get; set; }
    public string itembrandname { get; set; }

    public int qty { get; set; }

    public string remark { get; set; }

    public string createdby { get; set; }

    public DateTime createddate { get; set; }

    public bool isactive { get; set; }
}
