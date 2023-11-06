using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer;

[Serializable]
public record CreateItemTransferCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int transfertypeid { get; init; }

    public int sourceid { get; init; }

    public int destinationid { get; init; }

    public string description { get; init; }

    public string createdby { get; init; }

    public DateTime createddate { get; init; }

    public bool isactive { get; init; }

    public int transferstatus { get; init; }

    public List<CreateItemTransferDetailCommand> items { get; set; }
}
