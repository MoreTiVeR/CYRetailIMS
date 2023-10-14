using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.PaymentTypeService.Queries.PaymentTypeByID.v1;

[Serializable]
public record PaymentTypeByIDCommand : IRequest<BaseResponse<PaymentTypeByIDResponseDTO>>
{
    public int paymentid { get; init; }
}
