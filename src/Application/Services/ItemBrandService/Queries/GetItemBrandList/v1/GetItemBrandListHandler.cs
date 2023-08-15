using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandList.v1;
public class GetItemBrandListHandler : BaseService, IRequestHandler<GetItemBrandListQuery, BaseResponse<List<GetItemBrandListResponseDTO>>>
{
    public GetItemBrandListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetItemBrandListResponseDTO>>> Handle(GetItemBrandListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TMItemBrand> resItemsBrand = await _unitOfWork.Repository<TMItemBrand>().FindListAsync(w => w.IsActive);
        if (!resItemsBrand.Any())
        {
            throw new Exception("Data not found");
        }
        return new BaseResponse<List<GetItemBrandListResponseDTO>>
        {
            result = true,
            data = _mapper.Map<List<GetItemBrandListResponseDTO>>(resItemsBrand),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
