using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemService.Commands.CreateItemList;

[Serializable]
public record CreateItemListCommand : IRequest<BaseResponse<CommandResponse>>
{
    [Required]
    public List<CreateItemDetailCommand> items { get; init; }
}
