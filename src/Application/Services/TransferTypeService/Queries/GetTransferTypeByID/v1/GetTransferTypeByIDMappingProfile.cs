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

namespace CYRetailIMS.Application.Services.TransferTypeService.Queries.GetTransferTypeByID.v1;
public class GetTransferTypeByIDMappingProfile : Profile
{
    public GetTransferTypeByIDMappingProfile()
    {
        CreateMap<TMTransferType, GetTransferTypeByIDResponseDTO>()
            .ForMember(w => w.transfertypeid, f => f.MapFrom(w => w.TransferTypeID))
            .ForMember(w => w.transfertypename, f => f.MapFrom(w => w.TransferTypeName))
            .ForMember(w => w.description, f => f.MapFrom(w => w.Description))
            .ForMember(w => w.createdby, f => f.MapFrom(w => w.CreatedBy))
            .ForMember(w => w.creadeddate, f => f.MapFrom(w => w.CreadedDate))
            .ForMember(w => w.isactive, f => f.MapFrom(w => w.IsActive));
    }
}
