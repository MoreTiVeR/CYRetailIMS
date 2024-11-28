using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransfer.v1;
using FluentValidation;

namespace CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransferList.v1;
public class CreateMoneyTransferListValidator : AbstractValidator<CreateMoneyTransferListCommand>
{
    public CreateMoneyTransferListValidator()
    {
        RuleForEach(s => s.mtransferdata).SetValidator(new CreateMoneyTransferValidator());
    }
}