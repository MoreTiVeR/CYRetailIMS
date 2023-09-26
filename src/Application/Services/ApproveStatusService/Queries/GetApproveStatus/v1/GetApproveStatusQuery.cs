using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ApproveStatusService.Queries.GetApproveStatus.v1;

[Serializable]
public record GetApproveStatusQuery : IRequest<BaseResponse<List<GetApproveStatusResponseDTO>>>
{
}
