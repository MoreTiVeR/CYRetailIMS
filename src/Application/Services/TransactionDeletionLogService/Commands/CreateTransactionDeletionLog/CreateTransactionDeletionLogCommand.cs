using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.TransactionDeletionLogService.Commands.CreateTransactionDeletionLog;
public record CreateTransactionDeletionLogCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int branchid { get; init; }
    public int transactionid { get; set; }
    public string reason { get; init; }
    public string createdby { get; init; }
}
