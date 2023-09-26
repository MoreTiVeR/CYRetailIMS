using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.TransferTypeService.Queries.GetTransferTypeList.v1;
public record GetTransferTypeListQuery : IRequest<BaseResponse<List<GetTransferTypeListResponseDTO>>>
{
}
