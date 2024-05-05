using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.TransactionService.Commands.UpdateTransaction;

[Serializable]
public record UpdateTransactionCommand : IRequest<BaseResponse<CommandResponse>>
{
    [Required(ErrorMessage = "Required field.")]
    public int transactionid { get; init; }

    [Required(ErrorMessage = "Required field")]
    public DateTime transactiondate { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(10, ErrorMessage = "Maximum length 10")]
    public string updatedby { get; init; }

    public string remark { get; set; }

}
