using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferList.v1;

public class GetItemTransferListhandler : BaseService, IRequestHandler<GetItemTransferListQuery, BaseResponse<List<GetItemTransferResponseDTO>>>
{
    public GetItemTransferListhandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public Task<BaseResponse<List<GetItemTransferResponseDTO>>> Handle(GetItemTransferListQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
