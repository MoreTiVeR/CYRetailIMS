using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;
public class GetItemListHandler : BaseService, IRequestHandler<GetItemListQuery, BaseResponse<List<GetItemListResponseDTO>>>
{
    public GetItemListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetItemListResponseDTO>>> Handle(GetItemListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TMItem> resItems = await _unitOfWork.Repository<TMItem>().FindListAsync(w => w.IsActive);
        if (!resItems.Any())
        {
            throw new Exception("Data not found");
        }
        List<GetItemListResponseDTO> items = _mapper.Map<List<GetItemListResponseDTO>>(resItems);
        return new BaseResponse<List<GetItemListResponseDTO>>
        {
            result = true,
            data = items,
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
