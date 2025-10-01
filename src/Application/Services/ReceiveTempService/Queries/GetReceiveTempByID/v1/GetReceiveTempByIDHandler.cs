using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempList.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempByID.v1;
public class GetReceiveTempByIDHandler : BaseService, IRequestHandler<GetReceiveTempByIDQuery, BaseResponse<GetReceiveTempResponseDTO>>
{
    public GetReceiveTempByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetReceiveTempResponseDTO>> Handle(GetReceiveTempByIDQuery request, CancellationToken cancellationToken)
    {
        var resData = (from rtemp in await _unitOfWork.Repository<TMReceiveTemplate>().QueryAsync(w => w.BranchID > 0)
                       join branch in await _unitOfWork.Repository<TMBranch>().QueryAsync(w => w.IsActive) on rtemp.BranchID equals branch.BranchID
                       where rtemp.ReceiveTempID == request.tempreceiveid
                       select new GetReceiveTempResponseDTO
                       {
                           receivetempid = rtemp.ReceiveTempID,
                           branchid = rtemp.BranchID,
                           branchname = branch.BranchName,
                           shopheadernametext = rtemp.ShopHeaderNameText,
                           shopheaderaddresstext = rtemp.ShopHeaderAddressText,
                           telephoneno = rtemp.TelephoneNo,
                           additionalheadertext = rtemp.AdditionalHeaderText,
                           shopfootertext = rtemp.ShopFooterText,
                           additionalfootertext = rtemp.AdditionalFooterText,
                           printername = rtemp.PrinterName,
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
        return new BaseResponse<GetReceiveTempResponseDTO>
        {
            result = true,
            data = resData.FirstOrDefault(),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
