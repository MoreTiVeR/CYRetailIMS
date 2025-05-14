using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v2;
public class GetTransactionByBranchIDV2Query : IRequest<BaseResponse<GetTransactionByBranchIDV2ReseponseDTO>>
{
    public int branchid { get; init; }
    public DateTime transaction_startdate { get; init; }
    public DateTime transaction_enddate { get; init; }

    /// <summary>
    /// Start index 1
    /// </summary>
    public int startrow { get; set; }

    /// <summary>
    /// Total row take
    /// </summary>
    public int pagesize { get; set; }

    public string? searchvalue { get; set; }

    public bool isexportalldata { get; set; }
}
