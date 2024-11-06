using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.MoneyTransferService.Commands.UpdateMoneyTransfer.v1;
public record UpdateMoneyTransferCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int moneytransferid { get; set; }
    public int branchid { get; set; }
    public DateTime transferdate { get; set; }
    public decimal amounttransfer { get; set; }
    public string description { get; set; }
    public string slipimagepath { get; set; }
    public string updatedby { get; set; }
    public bool isactive { get; set; }
}
