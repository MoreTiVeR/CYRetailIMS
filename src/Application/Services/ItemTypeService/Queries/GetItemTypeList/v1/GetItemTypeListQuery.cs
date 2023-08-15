using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeByID.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeList.v1;

[Serializable]
public record GetItemTypeListQuery : IRequest<BaseResponse<List<GetItemTypeListResponseDTO>>> { }
