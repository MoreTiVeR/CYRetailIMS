using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransferList.v1;

public record CreateMoneyTransferSlipCommand
{
    public string slipimagepath { get; init; }
}
