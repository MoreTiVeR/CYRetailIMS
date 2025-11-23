using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.Services.EODSummaryService.Commands.UpdateEndOfDaySummary;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.EODSummaryService.Commands.CreateEndOfDaySummary;
public class EndOfDaySummaryProfile : Profile
{
    public EndOfDaySummaryProfile()
    {
        // Entity -> ViewModel
        CreateMap<TTEndOfDaySummary, EndOfDaySummaryViewModel>()
            .ForMember(d => d.EndOfDayId, opt => opt.MapFrom(s => s.EndOfDayId));

        // ViewModel -> CreateCommand
        CreateMap<EndOfDaySummaryViewModel, CreateEndOfDaySummaryCommand>()
            .ForMember(d => d.createdby, opt => opt.Ignore()) // ให้ Controller ใส่จาก User
            .ForMember(d => d.summarydate, opt => opt.MapFrom(s => s.SummaryDate));

        // CreateCommand -> Entity
        CreateMap<CreateEndOfDaySummaryCommand, TTEndOfDaySummary>()
            .ForMember(d => d.EndOfDayId, opt => opt.Ignore()); // Auto Generate

        // ViewModel -> UpdateCommand
        CreateMap<EndOfDaySummaryViewModel, UpdateEndOfDaySummaryCommand>()
            .ForMember(d => d.endofdayid, opt => opt.MapFrom(s => s.EndOfDayId.GetValueOrDefault()))
            .ForMember(d => d.updatedby, opt => opt.Ignore()); // Controller ใส่จาก User

        // Entity -> Update ViewModel (for Edit page)
        CreateMap<TTEndOfDaySummary, EndOfDaySummaryViewModel>();
    }
}
