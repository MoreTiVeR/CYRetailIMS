using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ReceiveTempService.Commands.CreateReceipt.v1;
public record CreateReceiptCommand : IRequest<BaseResponse<CommandResponse>>
{
    [Required]
    public int transactionid { get; init; }

    [Required]
    public string receiptno { get; init; }

    [Required]
    public string createdby { get; init; }
}
