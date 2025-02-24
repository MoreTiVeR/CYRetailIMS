using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.UpdateCountStock.v1;
public record UpdateCountStockCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int countstockid { get; init; }
    public int branchid { get; init; }
    public DateTime countstockdate { get; init; }
    public int totalcount { get; init; }
    public string? remark { get; init; }
    public string updatedby { get; init; }
    public List<UpdateCountStockDetail> detail { get; init; }
}
