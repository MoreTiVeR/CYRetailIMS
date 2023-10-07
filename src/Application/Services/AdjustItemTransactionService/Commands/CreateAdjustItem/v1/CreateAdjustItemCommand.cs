using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.CreateAdjustItem.v1;

[Serializable]
public record CreateAdjustItemCommand : IRequest<BaseResponse<CommandResponse>>
{
    public string remark { get; init; }
    public string createdby { get; init; }
    public DateTime createddate { get; init; }
    public List<CreateAdjustItemDetailCommand> items { get; set; }

}
