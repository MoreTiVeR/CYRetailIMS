using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.Services.SupplierService.Queries.GetSupplierList.v1;

namespace CYRetailIMS.Application.Common.Mappings.UI.Supplier;
public class SupplierMappingProfile : Profile
{
    public SupplierMappingProfile()
    {
        CreateMap<GetSupplierResponseDTO, EditSupplierViewModel>();
        
    }
}
