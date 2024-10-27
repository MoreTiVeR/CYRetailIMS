using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.UpdateItemTransfer.v1;
public record UpdateItemTransferCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int transferid { get; init; }
    public int sourceid { get; init; }
    public int destinationid { get; init; }
    public int itemid { get; set; }
    public int qty { get; init; }
    public int receiveqty { get; init; }
    public int returnqty { get; init; }
    public string description { get; init; }
    public int transferstatusid { get; init; }
    public string updatedby { get; set; }
    public DateTime updateddate { get; set; }
}
