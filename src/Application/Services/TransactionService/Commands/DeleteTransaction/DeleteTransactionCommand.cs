using System;
using System.ComponentModel.DataAnnotations;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.TransactionService.Commands.DeleteTransaction;

[Serializable]
public record DeleteTransactionCommand : IRequest<BaseResponse<CommandResponse>>
{
    [Required(ErrorMessage = "Required field.")]
    public int transactionid { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(10, ErrorMessage = "Maximum length 10")]
    public string deletedby { get; init; }


    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(500, ErrorMessage = "Maximum length 500")]
    public string reason { get; init; }

}
