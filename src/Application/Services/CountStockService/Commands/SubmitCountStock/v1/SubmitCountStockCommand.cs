using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.SubmitCountStock.v1;

/// <summary>
/// Command to submit a draft count stock for audit/approval.
/// Changes status from Draft(0) to Submitted(1).
/// </summary>
public record SubmitCountStockCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int countstockid { get; init; }
    public string submittedby { get; init; }
}
