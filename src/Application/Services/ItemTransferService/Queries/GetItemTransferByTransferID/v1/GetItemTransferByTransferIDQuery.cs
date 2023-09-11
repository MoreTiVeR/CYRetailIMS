using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
public record GetItemTransferByTransferIDQuery : IRequest<BaseResponse<GetItemTransferResponseDTO>>
{
    public int transferid { get; init; }
}
