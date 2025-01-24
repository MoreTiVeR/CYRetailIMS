using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v1;
public record CreateCountStockCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int branchid { get; init; }
    public DateTime countstockdate { get; init; }
    public int totalcount { get; init; }
    public string? remark { get; init; }
    public string createdby { get; init; }
    public List<CreateCountStockDetail> detail { get; init; }
}
