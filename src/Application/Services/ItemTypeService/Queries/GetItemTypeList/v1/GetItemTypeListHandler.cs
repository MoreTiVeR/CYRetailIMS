

using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeList.v1;
public class GetItemTypeListHandler : BaseService, IRequestHandler<GetItemTypeListQuery, BaseResponse<List<GetItemTypeListResponseDTO>>>
{
    public GetItemTypeListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetItemTypeListResponseDTO>>> Handle(GetItemTypeListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TMItemType> resItemsType = await _unitOfWork.Repository<TMItemType>().FindListAsync(w => w.IsActive);
        if (!resItemsType.Any())
        {
            throw new Exception("Data not found");
        }
        return new BaseResponse<List<GetItemTypeListResponseDTO>>
        {
            result = true,
            data = _mapper.Map<List<GetItemTypeListResponseDTO>>(resItemsType),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
