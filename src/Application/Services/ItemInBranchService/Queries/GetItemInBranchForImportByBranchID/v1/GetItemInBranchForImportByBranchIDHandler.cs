using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchForImportByBranchID.v1;
public class GetItemInBranchForImportByBranchIDHandler : BaseService, IRequestHandler<GetItemInBranchForImportByBranchIDQuery, BaseResponse<List<GetItemInBranchForImportByBranchIDResponseDTO>>>
{
    public GetItemInBranchForImportByBranchIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetItemInBranchForImportByBranchIDResponseDTO>>> Handle(GetItemInBranchForImportByBranchIDQuery request, CancellationToken cancellationToken)
    {
        var resItemInBranch = await _unitOfWork.Repository<TMItemInBranch>().FindListAsync(w => w.BranchID == request.branchid);
        if (!resItemInBranch.Any())
        {
            throw new Exception("ไม่พบข้อมูลสินค้าสาขา!");
        }

        return new BaseResponse<List<GetItemInBranchForImportByBranchIDResponseDTO>>
        {
            result = true,
            data = resItemInBranch.Select(s => new GetItemInBranchForImportByBranchIDResponseDTO { itemid = s.ItemID }).ToList(),
            soruce = "db",
            message = "Success",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
