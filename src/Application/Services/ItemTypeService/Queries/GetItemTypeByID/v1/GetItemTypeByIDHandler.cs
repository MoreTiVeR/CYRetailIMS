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

namespace CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeByID.v1;
public class GetItemTypeByIDHandler : BaseService, IRequestHandler<GetItemTypeByIDQuery, BaseResponse<GetItemTypeByIDResponseDTO>>
{
    public GetItemTypeByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetItemTypeByIDResponseDTO>> Handle(GetItemTypeByIDQuery request, CancellationToken cancellationToken)
    {
        TMItemType resItem = await _unitOfWork.Repository<TMItemType>().FirstOrDefaultAsync(w => w.ItemTypeID == request.itemtypeid && w.IsActive);
        if(resItem == null)
        {
            throw new Exception("Data not found");
        }
        return new BaseResponse<GetItemTypeByIDResponseDTO>
        {
            result = true,
            data = _mapper.Map<GetItemTypeByIDResponseDTO>(resItem),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
