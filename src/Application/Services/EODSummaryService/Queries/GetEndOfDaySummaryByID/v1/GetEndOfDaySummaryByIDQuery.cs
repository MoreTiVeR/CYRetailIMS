using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.EODSummaryService.Queries.GetEndOfDaySummaryByCriteria.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.EODSummaryService.Queries.GetEndOfDaySummaryByID.v1;
public record GetEndOfDaySummaryByIDQuery : IRequest<BaseResponse<GetEndOfDaySummaryByCriteriaDetail>>
{
    public int eodid { get; init; }
}
