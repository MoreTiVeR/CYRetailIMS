using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempList.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempByID.v1;
public record GetReceiveTempByIDQuery : IRequest<BaseResponse<GetReceiveTempResponseDTO>>
{
    public int tempreceiveid { get; init; }

}
