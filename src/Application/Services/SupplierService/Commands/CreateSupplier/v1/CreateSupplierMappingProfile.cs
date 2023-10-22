using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.SupplierService.Commands.CreateSupplier.v1;
public class CreateSupplierMappingProfile : Profile
{
    public CreateSupplierMappingProfile()
    {
        CreateMap<CreateSupplierCommand, TMSupplier>()
            .ForMember(w => w.SupplierTypeID, f => f.MapFrom(w => w.suppliertypeid))
            .ForMember(w => w.SupplierName_TH, f => f.MapFrom(w => w.suppliernameth))
            .ForMember(w => w.SupplierName_EN, f => f.MapFrom(w => w.suppliernameen))
            .ForMember(w => w.Description, f => f.MapFrom(w => w.description))
            //.ForMember(w => w.TMSupplierDetails, f => f.MapFrom(w => w.detail))
            .ForMember(w => w.TMSupplierContacts, f => f.MapFrom(w => w.contact));


        CreateMap<CreateSupplierDetail, TMSupplierDetail>()
            .ForMember(w => w.Address, f => f.MapFrom(w => w.address))
            .ForMember(w => w.City, f => f.MapFrom(w => w.city))
            .ForMember(w => w.ZipCode, f => f.MapFrom(w => w.zipcode))
            .ForMember(w => w.Phone, f => f.MapFrom(w => w.phone))
            .ForMember(w => w.Description, f => f.MapFrom(w => w.description));

        CreateMap<CreateSupplierContact, TMSupplierContact>()
            .ForMember(w => w.SupplierContactTypeID, f => f.MapFrom(w => w.suppliercontacttypeid))
            .ForMember(w => w.ContactAccountName, f => f.MapFrom(w => w.contactaccountname))
            .ForMember(w => w.ContactPerson, f => f.MapFrom(w => w.contactperson))
            .ForMember(w => w.MobileNo, f => f.MapFrom(w => w.mobileno))
            .ForMember(w => w.Description, f => f.MapFrom(w => w.desctiption));
    }
}
