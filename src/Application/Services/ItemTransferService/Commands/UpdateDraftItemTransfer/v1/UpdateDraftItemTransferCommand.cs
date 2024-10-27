using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.UpdateDraftItemTransfer.v1;
public record UpdateDraftItemTransferCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int draftid { get; set; }
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
