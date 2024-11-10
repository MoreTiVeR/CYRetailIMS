using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.MoneyTransferService.Quiries.GetMoneyTransferByCriteria.v1;
public record GetMoneyTransferByCriteriaQuery : IRequest<BaseResponse<List<GetMoneyTransferByCriteriaResponseDTO>>>
{
    public DateTime? startdate { get; init; }
    public DateTime? enddate { get; init; }
    public List<int>? branchlist { get; init; }
}
