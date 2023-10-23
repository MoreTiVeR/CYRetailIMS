using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.SupplierTypeService.Queries.GetSupplierTypeList.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.SupplierTypeService.Queries.SupplierTypeByID.v1;
public record SupplierTypeByIDQuery : IRequest<BaseResponse<GetSupplierTypeResponseDTO>>
{
    public int suppliertypeid { get; init; }
}
