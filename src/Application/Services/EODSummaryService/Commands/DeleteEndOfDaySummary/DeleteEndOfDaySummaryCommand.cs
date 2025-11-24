using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.EODSummaryService.Commands.DeleteEndOfDaySummary;
public record DeleteEndOfDaySummaryCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int eodid { get; init; }
    public string updatedby { get; init; }
    public bool isactive { get; init; }
}
