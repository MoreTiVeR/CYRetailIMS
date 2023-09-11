using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
public class GetItemTransferByTransferIDHandler : BaseService, IRequestHandler<GetItemTransferByTransferIDQuery, BaseResponse<GetItemTransferResponseDTO>>
{
    public GetItemTransferByTransferIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public Task<BaseResponse<GetItemTransferResponseDTO>> Handle(GetItemTransferByTransferIDQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
