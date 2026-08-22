using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.ApproveCountStock.v1;

/// <summary>
/// Command to approve a submitted count stock (HeadPC only).
/// Changes status to Approved(2) and updates TMItemInBranch stock to counted quantity.
/// Only Admin can approve HeadPC submissions.
/// </summary>
public record ApproveCountStockCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int countstockid { get; init; }
    public string approvedby { get; init; }
}
