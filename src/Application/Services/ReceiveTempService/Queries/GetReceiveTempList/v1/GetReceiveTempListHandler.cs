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
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempList.v1;
public class GetReceiveTempListHandler : BaseService, IRequestHandler<GetReceiveTempListQuery, BaseResponse<List<GetReceiveTempResponseDTO>>>
{
    public GetReceiveTempListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetReceiveTempResponseDTO>>> Handle(GetReceiveTempListQuery request, CancellationToken cancellationToken)
    {
        var resData = (from rtemp in await _unitOfWork.Repository<TMReceiveTemplate>().QueryAsync(w => w.BranchID > 0)
                       join branch in await _unitOfWork.Repository<TMBranch>().QueryAsync(w => w.IsActive) on rtemp.BranchID equals branch.BranchID
                       select new GetReceiveTempResponseDTO
                       {
                           receivetempid = rtemp.ReceiveTempID,
                           branchid = rtemp.BranchID,
                           branchname = branch.BranchName,
                           shopheadernametext = rtemp.ShopHeaderNameText,
                           shopheaderaddresstext = rtemp.ShopHeaderAddressText,
                           additionalheadertext = rtemp.AdditionalHeaderText,
                           shopfootertext = rtemp.ShopFooterText,
                           additionalfootertext = rtemp.AdditionalFooterText,
                           createdby = rtemp.CreatedBy,
                           createddate = rtemp.CreatedDate,
                           updatedby = rtemp.UpdatedBy,
                           updateddate = rtemp.UpdatedDate,
                           isactive = rtemp.IsActive
                       }).AsEnumerable();
        if (!resData.Any())
        {
            throw new Exception("ไม่พบข้อมูล");
        }
        return new BaseResponse<List<GetReceiveTempResponseDTO>> 
        { 
            result = true,
            data = resData.ToList(),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
