using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ChartService.Queries.GetMontlySaleSummaryBarchart.v1;

[Serializable]
public record GetMontlySaleSummaryBarchartQuery : IRequest<BaseResponse<List<GetMontlySaleSummaryBarchartResponseDTO>>>
{
    public int month { get; init; }
}
