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

namespace CYRetailIMS.Application.Services.SupplierTypeService.Queries.GetSupplierTypeList.v1;
public class GetSupplierTypeListQueryhandler : BaseService, IRequestHandler<GetSupplierTypeListQuery, BaseResponse<List<GetSupplierTypeResponseDTO>>>
{
    public GetSupplierTypeListQueryhandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetSupplierTypeResponseDTO>>> Handle(GetSupplierTypeListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TMSupplierType> resSuppliers = await _unitOfWork.Repository<TMSupplierType>().QueryAsync(w => w.IsActive);
        if (!resSuppliers.Any())
        {
            throw new Exception("ไม่พบข้อมูลซัฟพลายเออร์");
        }
        return new BaseResponse<List<GetSupplierTypeResponseDTO>>
        {
            result = true,
            data = _mapper.Map<List<GetSupplierTypeResponseDTO>>(resSuppliers),
            message = "Sucess",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
