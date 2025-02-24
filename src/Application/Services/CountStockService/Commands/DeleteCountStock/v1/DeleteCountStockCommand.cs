using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.DeleteCountStock.v1;
public record DeleteCountStockCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int countstockid { get; init; }
    public string deletedby { get; set; }

}
