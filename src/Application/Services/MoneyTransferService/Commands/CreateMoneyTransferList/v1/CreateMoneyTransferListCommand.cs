using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransfer.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransferList.v1;
public record CreateMoneyTransferListCommand : IRequest<BaseResponse<CommandResponse>>
{
    public List<CreateMoneyTransferCommand> mtransferdata { get; init; }
    public List<CreateMoneyTransferSlipCommand> transferslipdetail { get; init; }
}
