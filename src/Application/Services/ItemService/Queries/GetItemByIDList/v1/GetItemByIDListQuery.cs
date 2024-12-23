using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemService.Queries.GetItemByIDList.v1;
public record GetItemByIDListQuery : IRequest<BaseResponse<List<GetItemListResponseDTO>>>
{
    public List<int> itemidlist { get; set; }
}