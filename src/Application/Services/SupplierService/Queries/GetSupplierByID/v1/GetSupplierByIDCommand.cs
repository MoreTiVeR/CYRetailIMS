using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.SupplierService.Queries.GetSupplierList.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.SupplierService.Queries.GetSupplierByID.v1;

[Serializable]
public record GetSupplierByIDCommand : IRequest<BaseResponse<GetSupplierResponseDTO>>
{
    public int supplierid { get; init; }
}
