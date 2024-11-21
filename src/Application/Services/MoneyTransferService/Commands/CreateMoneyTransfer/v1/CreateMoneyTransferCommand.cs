using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransfer.v1;
public record CreateMoneyTransferCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int branchid { get; init; }
    public DateTime transferdate { get; init; }
    public decimal amounttransfer { get; init; }
    public string description { get; init; }
    public string slipimagepath { get; init; }
    public string createdby { get; init; }

}
