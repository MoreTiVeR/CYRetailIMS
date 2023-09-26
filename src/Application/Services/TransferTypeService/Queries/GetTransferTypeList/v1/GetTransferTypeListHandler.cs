using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.TransferTypeService.Queries.GetTransferTypeByID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.TransferTypeService.Queries.GetTransferTypeList.v1;
public class GetTransferTypeListHandler : BaseService, IRequestHandler<GetTransferTypeListQuery, BaseResponse<List<GetTransferTypeListResponseDTO>>>
{
    public GetTransferTypeListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetTransferTypeListResponseDTO>>> Handle(GetTransferTypeListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TMTransferType> resTransferType = await _unitOfWork.Repository<TMTransferType>().QueryAsync(w => w.IsActive);
        if (!resTransferType.Any() && resTransferType.Count() == 0)
        {
            throw new Exception("ไม่พบข้อมูลสถานะการโอนสินค้า");
        }
        return new BaseResponse<List<GetTransferTypeListResponseDTO>>
        {
            result = true,
            data = _mapper.Map<List<GetTransferTypeListResponseDTO>>(resTransferType),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
