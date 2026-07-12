using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStockV2.v1;

public record CreateCountStockV2Command : IRequest<BaseResponse<CommandResponse>>
{
    public int branchid { get; init; }
    public DateTime countstockdate { get; init; }
    public int totalcount { get; init; }
    public string? remark { get; init; }
    public string createdby { get; init; }
    public List<CreateCountStockV2Detail> detail { get; init; }
}
