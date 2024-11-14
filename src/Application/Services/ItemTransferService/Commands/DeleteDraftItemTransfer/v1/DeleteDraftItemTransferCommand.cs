using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.DeleteDraftItemTransfer.v1;
public record DeleteDraftItemTransferCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int draftid { get; init; }
    public string updatedby { get; init; }
}
