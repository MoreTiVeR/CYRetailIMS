using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.SubItemTypeService.Commands.CreateSubItemType.v1;
using CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeByID.v1;
using CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeByItemIDList.v1;
using CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeList.v1;

namespace CYRetailIMS.Application.ExternalService.SubItemTypeAPI;

public interface ISubItemTypeAPI
{
    Task<BaseResponse<CommandResponse>> CreateSubItemTypeAsync(CreateSubItemTypeCommand subItemTypeCommand);
    Task<BaseResponse<List<GetSubItemTypeResponseDTO>>> GetSubItemTypeListAsync();
    Task<BaseResponse<GetSubItemTypeResponseDTO>> GetSubItemTypeByIDAsync(GetSubItemTypeByIDQuery subItemTypeByIDQuery);
    Task<BaseResponse<List<GetSubItemTypeByItemIDListResponseDTO>>> GetSubItemTypeByItemIDListAsync(GetSubItemTypeByItemIDListQuery listQuery);
}
