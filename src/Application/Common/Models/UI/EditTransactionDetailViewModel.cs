using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;

[Serializable]
public class EditTransactionDetailViewModel
{
    public int Seq { get; set; }
    public int TransactionID { get; set; }
    public int TransactionDetailID { get; set; }

    public int ItemID { get; set; }

    public string ItemName { get; set; }

    public decimal Price { get; set; }

    public int Qty { get; set; }

    public decimal Amount { get; set; }
}
