using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.TransactionTypeService.Queries.GetTrasnactionList.v1;
public record class GetTrasnactionByCriteriaQuery : IRequest<BaseResponse<List<GetTrasnactionByCriteriaResponseDTO>>>
{
    public int? transactiontypeid { get; init; }
    public string? transactiontypecode { get; set; }
    public bool? isactive { get; set; }
}
