using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;

namespace CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.UpdateAdjustItem;
public class UpdateAdjustItemHandler : BaseService, IRequestHandler<UpdateAdjustItemCommand, BaseResponse<CommandResponse>>
{
    public UpdateAdjustItemHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public Task<BaseResponse<CommandResponse>> Handle(UpdateAdjustItemCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
