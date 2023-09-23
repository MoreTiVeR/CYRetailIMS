using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMItemInBranchs;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Commands.DeleteItemInBranch.v1;
public class DeleteItemInBranchHandler : BaseService, IRequestHandler<DeleteItemInBranchCommand, BaseResponse<CommandResponse>>
{
    public DeleteItemInBranchHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(DeleteItemInBranchCommand request, CancellationToken cancellationToken)
    {
        IQueryable<TMItemInBranch> resItemInBranch = await _unitOfWork.Repository<TMItemInBranch>().QueryAsync(w => w.ItemID == request.itemid 
        && w.BranchID == request.branchid);
        if (resItemInBranch == null || !resItemInBranch.Any())
        {
            throw new Exception("ไม่พบข้อมูลสินค้าในสาขา");
        }

        resItemInBranch.ToList().ForEach(e =>
        {
            e.DeActiveStatus();
            e.SetUpdatedBy(request.updatedby);
            e.SetUpdatedDate(request.updateddate);
            e.AddDomainEvent(new TMItemInBranchUpdateEvent(e));
        });
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse { result = true },
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
