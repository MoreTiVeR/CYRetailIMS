using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.CreateAdjustItem.v1;
public record CreateAdjustItemCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int adjusttypeid { get; set; }
    public int itemid { get; set; }
    public int qty { get; set; }
    public string createdby { get; set; }
    public DateTime createddate { get; set; }
}
