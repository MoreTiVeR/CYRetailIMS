using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempList.v1;
public class GetReceiveTempListQuery : IRequest<BaseResponse<List<GetReceiveTempResponseDTO>>>
{
}
