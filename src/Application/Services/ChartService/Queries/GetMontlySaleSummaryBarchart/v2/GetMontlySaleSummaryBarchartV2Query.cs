using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ChartService.Queries.GetMontlySaleSummaryBarchart.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.ChartService.Queries.GetMontlySaleSummaryBarchart.v2;

[Serializable]
public record GetMontlySaleSummaryBarchartV2Query : IRequest<BaseResponse<List<GetMontlySaleSummaryBarchartResponseDTO>>>
{
    public int month { get; init; }
}
