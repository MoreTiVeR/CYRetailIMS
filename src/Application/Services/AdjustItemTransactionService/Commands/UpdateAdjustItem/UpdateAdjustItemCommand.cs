using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.UpdateAdjustItem;
public record UpdateAdjustItemCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int adjustid { get; init; }
}
