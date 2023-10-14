using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.PaymentTypeService.Queries.GetPaymentTypeList.v1;

[Serializable]
public record GetPaymentTypeListCommand : IRequest<BaseResponse<List<GetPaymentTypeListResponseDTO>>>
{
}
