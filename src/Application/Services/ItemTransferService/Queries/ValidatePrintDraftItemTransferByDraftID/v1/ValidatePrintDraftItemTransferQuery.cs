using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.ValidatePrintDraftItemTransferByDraftID.v1;
public record ValidatePrintDraftItemTransferQuery : IRequest<BaseResponse<ValidatePrintDraftItemTransferResponseDTO>>
{
    public int draftid { get; init; }
}
