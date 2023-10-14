using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderList.v1;
public class GetPurchaseOrderListHandler : BaseService, IRequestHandler<GetPurchaseOrderListCommand, BaseResponse<List<GetPurchaseOrderResposeDTO>>>
{
    public GetPurchaseOrderListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public Task<BaseResponse<List<GetPurchaseOrderResposeDTO>>> Handle(GetPurchaseOrderListCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
