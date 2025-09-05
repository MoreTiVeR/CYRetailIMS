using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.ReceiveTempService.Commands.CreateReceiveTemplate.v1;
public class CreateReceiveTemplateMappingProfile : Profile
{
    public CreateReceiveTemplateMappingProfile()
    {
        CreateMap<CreateReceiveTemplateCommand, TMReceiveTemplate>()
            .ForMember(dest => dest.ReceiveTempID, opt => opt.Ignore())
            .ForMember(dest => dest.BranchID, opt => opt.MapFrom(src => src.branchid))
            .ForMember(dest => dest.ShopHeaderNameText, opt => opt.MapFrom(src => src.shopheadernametext))
            .ForMember(dest => dest.ShopHeaderAddressText, opt => opt.MapFrom(src => src.shopheaderaddresstext))
            .ForMember(dest => dest.AdditionalHeaderText, opt => opt.MapFrom(src => src.additionalheadertext))
            .ForMember(dest => dest.ShopFooterText, opt => opt.MapFrom(src => src.shopfootertext))
            .ForMember(dest => dest.AdditionalFooterText, opt => opt.MapFrom(src => src.additionalfootertext))
            .ForMember(dest => dest.TelephoneNo, opt => opt.MapFrom(src => src.telephoneno))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.createdby));
            //.ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.createddate))
            //.ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            //.ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
            //.ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
    }
}
