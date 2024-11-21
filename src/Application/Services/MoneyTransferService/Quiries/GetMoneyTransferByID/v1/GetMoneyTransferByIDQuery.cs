using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.MoneyTransferService.Quiries.GetMoneyTransferByCriteria.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.MoneyTransferService.Quiries.GetMoneyTransferByID.v1;
public record GetMoneyTransferByIDQuery : IRequest<BaseResponse<GetMoneyTransferByCriteriaResponseDTO>>
{
    public int moneytransferid { get; init; }
}
