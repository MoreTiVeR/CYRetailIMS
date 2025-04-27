using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.TransactionTypeService.Queries.GetTrasnactionList.v1;
public class GetTrasnactionByCriteriaResponseDTO
{
    public int transactiontypeid { get; set; }
    public string transactiontypecode { get; set; }
    public string transactiontypename { get; set; }
    public string desc { get; set; }
    public bool isactive { get; set; }
}
