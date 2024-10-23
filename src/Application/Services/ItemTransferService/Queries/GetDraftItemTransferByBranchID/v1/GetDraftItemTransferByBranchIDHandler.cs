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

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.GetDraftItemTransferByBranchID.v1;
public class GetDraftItemTransferByBranchIDHandler : BaseService, IRequestHandler<GetDraftItemTransferByBranchIDQuery, BaseResponse<GetDraftItemTransferByBranchIDResponseDTO>>
{
    public GetDraftItemTransferByBranchIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetDraftItemTransferByBranchIDResponseDTO>> Handle(GetDraftItemTransferByBranchIDQuery request, CancellationToken cancellationToken)
    {
        if(request.branchid <= 0)
        {
            throw new Exception("ข้อมูลสาขาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        }
        var resData = await (from header in await _unitOfWork.Repository<TTDraftItemTransfer>().FindWithInclude(w => w.DestinationBranchID == request.branchid & w.IsActive, i => i.Include(s => s.TTDraftItemTransferDetails))
                             //join detail in await _unitOfWork.Repository<TTDraftItemTransferDetail>().QueryAsync() on header.TransferHeaderID equals detail.TransferHeaderID
                             join b in await _unitOfWork.Repository<TMBranch>().QueryAsync() on header.DestinationBranchID equals b.BranchID into branch
                             from jBranch in branch.DefaultIfEmpty()
                             select new GetDraftItemTransferByBranchIDResponseDTO
                             {
                                 transferheaderid = header.TransferHeaderID,
                                 refno = header.TransferRefNo,
                                 destinationbranchid = header.DestinationBranchID,
                                 destinationbranchname = jBranch != null ? jBranch.BranchName : string.Empty,
                                 createdby = header.CreatedBy,
                                 createddate = header.CreatedDate,
                                 isactive = header.IsActive,
                                 transferstatus = header.TransferStatus,
                                 detail = (from d in header.TTDraftItemTransferDetails
                                           join i in _unitOfWork.Repository<TMItem>().Query() on d.ItemID equals i.ItemID
                                           select new GetDraftItemTransferDetailResponseDTO
                                           {
                                               transferdetailid = d.TransferDetailID,
                                               itemid = d.ItemID,
                                               itemname = i.Name,
                                               qty = d.Qty
                                           }).ToList()
                             }).FirstOrDefaultAsync();

        if(resData == null)
        {
            throw new Exception("ไม่พบข้อมูล");
        }
        return new BaseResponse<GetDraftItemTransferByBranchIDResponseDTO>
        {
            result = true,
            data = resData,
            message = " Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
