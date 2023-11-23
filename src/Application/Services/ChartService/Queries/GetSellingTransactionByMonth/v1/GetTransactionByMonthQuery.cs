using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ChartService.Queries.GetSellingTransactionByMonth.v1;
public record GetTransactionByMonthQuery : IRequest<BaseResponse<List<GetSellingTransactionByMonthResponseDTO>>>
{
    public int month { get; init; }
    public int year { get; init; }
}
