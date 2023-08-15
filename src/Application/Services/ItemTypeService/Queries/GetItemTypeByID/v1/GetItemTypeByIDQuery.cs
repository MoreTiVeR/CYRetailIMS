using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeByID.v1;

[Serializable]
public record GetItemTypeByIDQuery : IRequest<BaseResponse<GetItemTypeByIDResponseDTO>>
{
    [Required(ErrorMessage = "Itemtypeid is required")]
    public int itemtypeid { get; init; }
}
