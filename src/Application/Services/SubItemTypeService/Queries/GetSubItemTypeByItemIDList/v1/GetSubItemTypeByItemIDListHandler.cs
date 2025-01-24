using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeList.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeByItemIDList.v1;
public class GetSubItemTypeByItemIDListHandler : BaseService, IRequestHandler<GetSubItemTypeByItemIDListQuery, BaseResponse<List<GetSubItemTypeByItemIDListResponseDTO>>>
{
    public GetSubItemTypeByItemIDListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetSubItemTypeByItemIDListResponseDTO>>> Handle(GetSubItemTypeByItemIDListQuery request, CancellationToken cancellationToken)
    {
        var res = (from a in await _unitOfWork.Repository<TMItem>().QueryAsync(w => request.itemids.Contains(w.ItemID))
                   join b in await _unitOfWork.Repository<TMSubItemType>().QueryAsync(w => w.IsActive) on a.SubItemTypeID equals b.SubItemTypeID into _jSubItem
                   from subitem in _jSubItem.DefaultIfEmpty()
                   select new GetSubItemTypeByItemIDListResponseDTO
                   {
                       itemid = a.ItemID,
                       itemname = a.Name,
                       subitemtypeid = a.SubItemTypeID.HasValue ? a.SubItemTypeID.Value : null,
                       subitemcode = subitem != null ? subitem.SubItemCode : null,
                       nameth = subitem != null ? subitem.SubTypeNameTH : null,
                       nameen = subitem != null ? subitem.SubTypeNameEN : null
                   }).ToList();
        if(res is null || res.Count == 0)
        {
            throw new Exception("ไม่ม่พบข้อมูล");
        }
        return new BaseResponse<List<GetSubItemTypeByItemIDListResponseDTO>>
        {
            result = true,
            data = res,
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
