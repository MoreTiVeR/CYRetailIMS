using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItemList;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;

[Serializable]
public record CreateItemCommand : CreateItemDetailCommand, IRequest<BaseResponse<CommandResponse>>
{
    
}
