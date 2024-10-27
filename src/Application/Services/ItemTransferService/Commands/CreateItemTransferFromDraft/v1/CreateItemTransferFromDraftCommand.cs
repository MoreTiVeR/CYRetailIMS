using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer.v1;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransferFromDraft.v1;
public record CreateItemTransferFromDraftCommand : CreateItemTransferCommand
{
    public int draftid { get; set; }
}
