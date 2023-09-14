using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMBranchs;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.BranchService.Commands.CreateBranch.v1;
public class CreateBranchHandler : BaseService, IRequestHandler<CreateBranchCommand, BaseResponse<CommandResponse>>
{
    public CreateBranchHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        TMBranch isExist = await _unitOfWork.Repository<TMBranch>().FindAsync(w => w.BranchCode.Equals(request.branchcode) 
        || (w.BranchName.Contains(request.branchname) || w.BranchName.Equals(request.branchname)));
        if (isExist != null)
        {
            throw new Exception("มีข้อมูลสาขานี้ในระบบแล้ว");
        }

        TMBranch entData = PrepareBranchData(request);
        entData.AddDomainEvent(new TMBranchCreateEvent(entData));
        await _unitOfWork.Repository<TMBranch>().AddAsync(entData);

        await _unitOfWork.SaveChangesAsync();

        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse { result = true },
            status = StatusCodes.Status200OK.ToString(),
            message = "Success",
            soruce = "db"
        };
    }

    private TMBranch PrepareBranchData(CreateBranchCommand req)
    {
        return new TMBranch
        {
            BranchCode = req.branchcode.Trim().ToUpper(),
            BranchName = req.branchname,
            CreadedDate = req.creadeddate,
            CreatedBy = req.createdby,
            IsActive = req.isactive
        };
    }
}
