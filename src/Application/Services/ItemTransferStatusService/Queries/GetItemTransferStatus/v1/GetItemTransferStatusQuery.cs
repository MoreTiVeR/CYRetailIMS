using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemTransferStatusService.Queries.GetItemTransferStatus.v1;
public record GetItemTransferStatusQuery : IRequest<BaseResponse<List<GetItemTransferStatusResponseDTO>>>
{
}
