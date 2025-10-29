using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempList.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempByCriteria.v1;
public class GetReceiveTempByCriteriaHandler : BaseService, IRequestHandler<GetReceiveTempByCriteriaQuery, BaseResponse<GetReceiveTempByCriteriaResponseDTO>>
{
    public GetReceiveTempByCriteriaHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetReceiveTempByCriteriaResponseDTO>> Handle(GetReceiveTempByCriteriaQuery request, CancellationToken cancellationToken)
    {
        int totalRowCount = 0;
        IQueryable<GetReceiveTempResponseDTO> searchData = (from rtemp in await _unitOfWork.Repository<TMReceiveTemplate>().QueryAsync(w => w.BranchID > 0)
                                                            join branch in await _unitOfWork.Repository<TMBranch>().QueryAsync(w => w.IsActive) on rtemp.BranchID equals branch.BranchID
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
                                                            });

        if (request.branchid.HasValue)
        {
            searchData = searchData.Where(w => w.branchid == request.branchid.Value);
        }

        if (!string.IsNullOrEmpty(request.searchvalue))
        {
            searchData = searchData.Where(w => w.branchname.Contains(request.searchvalue)
            || w.shopheadernametext.Contains(request.searchvalue)
            || w.shopfootertext.Contains(request.searchvalue)
            || w.shopheaderaddresstext.Contains(request.searchvalue));
        }

        List<GetReceiveTempResponseDTO> resData = new List<GetReceiveTempResponseDTO>();

        totalRowCount = searchData.Count();
        if (request.isexportalldata)
        {
            resData = searchData.ToList();
        }
        else
        {
            resData = searchData.ToList().Skip(request.startrow).Take(request.pagesize).ToList();
        }
        if (!resData.Any())
        {
            throw new Exception("ไม่พบข้อมูลรายงานยอดรวมขายสินค้า");
        }

        return new BaseResponse<GetReceiveTempByCriteriaResponseDTO>
        {
            result = true,
            data = new GetReceiveTempByCriteriaResponseDTO
            {
                totalrow = totalRowCount,
                receipttemplates = resData
            },
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
