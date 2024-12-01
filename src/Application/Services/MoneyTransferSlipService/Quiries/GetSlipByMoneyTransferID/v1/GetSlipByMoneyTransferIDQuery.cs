using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.MoneyTransferSlipService.Quiries.GetSlipByMoneyTransferID.v1;
public record GetSlipByMoneyTransferIDQuery : IRequest<BaseResponse<GetSlipByMoneyTransferIDResponseDTO>>
{
    public int moneytransferid { get; init; }
}
