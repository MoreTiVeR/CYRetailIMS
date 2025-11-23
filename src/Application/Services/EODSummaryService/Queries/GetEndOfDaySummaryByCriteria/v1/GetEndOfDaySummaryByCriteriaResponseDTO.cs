using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;

namespace CYRetailIMS.Application.Services.EODSummaryService.Queries.GetEndOfDaySummaryByCriteria.v1;
public class GetEndOfDaySummaryByCriteriaResponseDTO
{
    public int totalrow { get; set; }

    public List<GetEndOfDaySummaryByCriteriaDetail> transactiondata { get; set; }

}
