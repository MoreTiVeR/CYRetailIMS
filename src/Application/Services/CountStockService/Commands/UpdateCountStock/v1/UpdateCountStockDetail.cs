using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v1;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.UpdateCountStock.v1;
public record UpdateCountStockDetail : CreateCountStockDetail
{
    public int countstockdetailid { get; init; }
}
