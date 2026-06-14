using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMBranchs;
using CYRetailIMS.Domain.Events.TMBranchsDetail;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.BranchService.Commands.DeleteBranch.v1;

public class DeleteBranchHandler : BaseService, IRequestHandler<DeleteBranchCommand, BaseResponse<CommandResponse>>
{
    public DeleteBranchHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        if(request.branhid == 1)
        {
            throw new Exception("ไม่สามารถลบสาขาสำนักงานใหญ่ได้");
        }

        IQueryable<TMBranch> resBranch = await _unitOfWork.Repository<TMBranch>().FindWithInclude(w => w.BranchID == request.branhid, i => i.Include(s => s.TMBranchDetail),
            ii => ii.Include(s => s.TMItemInBranches.Where(w => w.IsActive)));
        if (resBranch == null && !resBranch.Any())
        {
            throw new Exception("ไม่พบข้อมูลสาขา");
        }

        var itemsInBranch = resBranch.FirstOrDefault().TMItemInBranches;
        if (itemsInBranch != null && itemsInBranch.Any())
        {
            throw new Exception("ไม่สามารถลบสาขาได้, เนื่องจากมีสต๊อกอยู่ในสาขา");
        }

        DateTime updateDate = DateTime.Now;
        TMBranch branchEnt = resBranch.FirstOrDefault();
        branchEnt.IsActive = false;
        branchEnt.SetUpdatedBy(request.updatedby);
        branchEnt.SetUpdatedDate(updateDate);
        if (branchEnt.TMBranchDetail != null)
        {
            branchEnt.TMBranchDetail.IsActive = false;
            branchEnt.TMBranchDetail.SetUpdatedBy(request.updatedby);
            branchEnt.TMBranchDetail.SetUpdatedDate(updateDate);
            branchEnt.TMBranchDetail.AddDomainEvent(new TMBranchDetailUpdateEvent(branchEnt.TMBranchDetail));
        }
        branchEnt.AddDomainEvent(new TMBranchUpdateEvent(branchEnt));
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
