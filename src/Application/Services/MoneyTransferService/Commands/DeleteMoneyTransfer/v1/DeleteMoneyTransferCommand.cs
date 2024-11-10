using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.MoneyTransferService.Commands.DeleteMoneyTransfer.v1;
public record DeleteMoneyTransferCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int moeytransferid { get; init; }
    public string updatedby { get; init; }
}
