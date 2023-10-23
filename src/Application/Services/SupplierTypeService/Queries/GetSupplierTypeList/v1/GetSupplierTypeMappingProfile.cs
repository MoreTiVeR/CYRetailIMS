using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.SupplierTypeService.Queries.GetSupplierTypeList.v1;
public class GetSupplierTypeMappingProfile : Profile
{
    public GetSupplierTypeMappingProfile()
    {
        CreateMap<TMSupplierType, GetSupplierTypeResponseDTO>();
    }
}
