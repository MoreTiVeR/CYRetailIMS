using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.SupplierContactTypeService.Queries.GetSupplierContactTypeList.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.SupplierContactTypeService.Queries.GetSupplierContactTypeByID.v1;

[Serializable]
public record GetSupplierContactTypeByIDCommand : IRequest<BaseResponse<GetSupplierContactTypeResposeDTO>>
{
    public int suppliercontacttypeid { get; init; }
}
