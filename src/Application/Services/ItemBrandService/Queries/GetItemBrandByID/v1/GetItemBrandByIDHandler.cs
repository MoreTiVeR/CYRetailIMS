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

namespace CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandByID.v1;
public class GetItemBrandByIDHandler : BaseService, IRequestHandler<GetItemBrandByIDQuery, BaseResponse<GetItemBrandByIDResponseDTO>>
{
    public GetItemBrandByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetItemBrandByIDResponseDTO>> Handle(GetItemBrandByIDQuery request, CancellationToken cancellationToken)
    {
        TMItemBrand resItemBrand = await _unitOfWork.Repository<TMItemBrand>().FirstOrDefaultAsync(w => w.BrandID == request.itembrandid);
        if(resItemBrand == null)
        {
            throw new Exception("Data not found");
        }
        return new BaseResponse<GetItemBrandByIDResponseDTO>
        {
            result = true,
            data = _mapper.Map<GetItemBrandByIDResponseDTO>(resItemBrand),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
