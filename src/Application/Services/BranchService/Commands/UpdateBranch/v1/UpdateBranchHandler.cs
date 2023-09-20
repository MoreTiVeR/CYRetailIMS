using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.BranchService.Commands.DeleteBranch.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMBranchs;
using CYRetailIMS.Domain.Events.TMBranchsDetail;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.BranchService.Commands.UpdateBranch.v1;

public class UpdateBranchHandler : BaseService, IRequestHandler<UpdateBranchCommand, BaseResponse<CommandResponse>>
{
    public UpdateBranchHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
    {
        IQueryable<TMBranch> resBranch = await _unitOfWork.Repository<TMBranch>().FindWithInclude(w => w.BranchID == request.branhid, i => i.Include(s => s.TMBranchDetail));
        if(resBranch == null && !resBranch.Any())
        {
            throw new Exception("ไม่พบข้อมูลสาขา");
        }

        DateTime updateDate = DateTime.Now;
        TMBranch branchEnt = resBranch.FirstOrDefault();
        branchEnt.BranchName = request.branchname;
        branchEnt.SetUpdatedBy(request.updatedby);
        branchEnt.SetUpdatedDate(updateDate);
        //If exist then update
        if (branchEnt.TMBranchDetail != null)
        {
            branchEnt.TMBranchDetail.Address1 = request.address;
            branchEnt.TMBranchDetail.SetUpdatedBy(request.updatedby);
            branchEnt.TMBranchDetail.SetUpdatedDate(updateDate);
            branchEnt.TMBranchDetail.AddDomainEvent(new TMBranchDetailUpdateEvent(branchEnt.TMBranchDetail));
        }
        else
        {
            //Create new TMBranchDetail
            branchEnt.TMBranchDetail = MappingBranchDetail(request);
            branchEnt.TMBranchDetail.SetCreatedBy(request.updatedby);
            branchEnt.TMBranchDetail.SetCreatedDate(updateDate);
            branchEnt.TMBranchDetail.AddDomainEvent(new TMBranchDetailCreateEvent(branchEnt.TMBranchDetail));
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

    private TMBranchDetail MappingBranchDetail(UpdateBranchCommand req)
    {
        return new TMBranchDetail
        {
            Address1 = req.address,

        };
    }
}
