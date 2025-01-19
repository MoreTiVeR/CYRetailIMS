using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v1;
public class CreateCountStockMappingProfile : Profile
{
    public CreateCountStockMappingProfile()
    {
        CreateMap<TMItemInBranch, TTCountStocksHistory>();
        //CreateMap<TMItemInBranch, TTCountStocksHistory>().ForMember(s => s.WarehouseQty, f => f.MapFrom(m => m.Item.Qty));
    }
}
