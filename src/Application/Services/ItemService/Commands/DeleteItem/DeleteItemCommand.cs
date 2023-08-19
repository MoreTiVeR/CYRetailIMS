using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemService.Commands.DeleteItem;

[Serializable]
public record class DeleteItemCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int itemid { get; init; }
}
