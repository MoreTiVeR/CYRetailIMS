using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.EODSummaryService.Commands.UpdateEndOfDaySummary;
public record UpdateEndOfDaySummaryCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int endofdayid { get; init; }

    public DateTime summarydate { get; init; }
    public decimal totalcash { get; init; }
    public decimal depositedcash { get; init; }
    public decimal totaltransfer { get; init; }
    public decimal customertransfer { get; init; }
    public decimal grandtotal { get; init; }
    public decimal? substitutewage { get; init; }
    public decimal? fee { get; init; }
    public decimal? otherexpense { get; init; }
    public string? otherexpensenote { get; init; }
    public decimal finaltotal { get; init; }
    public bool isactive { get; init; }

    public string updatedby { get; init; }
}
