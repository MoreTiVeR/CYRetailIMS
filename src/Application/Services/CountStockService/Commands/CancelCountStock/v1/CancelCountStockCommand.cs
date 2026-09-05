using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.CancelCountStock.v1;

/// <summary>
/// Command to cancel a submitted count stock and set it back to draft.
/// Changes status from Submitted(1) to Draft(0).
/// </summary>
public record CancelCountStockCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int countstockid { get; init; }
    public string canceledby { get; init; }
}
