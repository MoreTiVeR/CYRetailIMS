using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeByItemIDList.v1;
public record GetSubItemTypeByItemIDListQuery : IRequest<BaseResponse<List<GetSubItemTypeByItemIDListResponseDTO>>>
{
    public List<int> itemids { get; init; }
}
