using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.SupplierTypeService.Queries.GetSupplierTypeList.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.SupplierTypeService.Queries.SupplierTypeByID.v1;
public class SupplierTypeByIDHandler : BaseService, IRequestHandler<SupplierTypeByIDQuery, BaseResponse<GetSupplierTypeResponseDTO>>
{
    public SupplierTypeByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetSupplierTypeResponseDTO>> Handle(SupplierTypeByIDQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TMSupplierType> resSuppliers = await _unitOfWork.Repository<TMSupplierType>().QueryAsync(w => w.IsActive);
        if (!resSuppliers.Any())
        {
            throw new Exception("ไม่พบข้อมูลซัฟพลายเออร์");
        }
        return new BaseResponse<GetSupplierTypeResponseDTO>
        {
            result = true,
            data = _mapper.Map<GetSupplierTypeResponseDTO>(resSuppliers.FirstOrDefault()),
            message = "Sucess",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
