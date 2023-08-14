using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemService.Queries.GetItemByID.v1;
public record GetItemByIDQuery : IRequest<BaseResponse<GetItemByIDResponseDTO>>
{
    [Required(ErrorMessage = "Item id is required")]
    public int itemid { get; init; }
}
