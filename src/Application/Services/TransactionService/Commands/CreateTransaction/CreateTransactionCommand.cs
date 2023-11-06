using System.ComponentModel.DataAnnotations;
using CYRetailIMS.Application.Common.Models;
using MediatR;
using Newtonsoft.Json;

namespace CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;

[Serializable]
public record CreateTransactionCommand : IRequest<BaseResponse<CommandResponse>>
{
    [Required(ErrorMessage = "Required field")]
    public int transactiontypeid { get; init; }

    [Required(ErrorMessage = "Required field")]
    public DateTime transactiondate { get; init; }

    [Required(ErrorMessage = "Required field")]
    public int branchid { get; init; }

    [Required(ErrorMessage = "Required field")]
    public decimal amounttransfer { get; init; }

    [Required(ErrorMessage = "Required field")]
    public decimal amountdeposit { get; init; }

    [Required(ErrorMessage = "Required field")]
    public decimal amountcash { get; init; }

    [Required(ErrorMessage = "Required field")]
    public decimal fee { get; init; }

    [Required(ErrorMessage = "Required field")]
    public decimal totalamount { get; init; }

    [Required(ErrorMessage = "Required field")]
    public bool isexcludevat { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(10, ErrorMessage = "Maximum length 10")]
    public string createdby { get; init; }

    [Required(ErrorMessage = "Required field")]
    public DateTime createddate { get; init; }

    [Required(ErrorMessage = "Required field")]
    public bool isactive { get; init; }

    [Required(ErrorMessage = "Required field")]
    [JsonProperty(Required = Required.Always)]
    public List<CreateTransactionDetailCommand> transactiondetail { get; init; }
}
