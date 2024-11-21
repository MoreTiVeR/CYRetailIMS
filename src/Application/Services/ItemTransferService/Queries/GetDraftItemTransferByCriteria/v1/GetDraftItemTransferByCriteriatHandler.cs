using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetDraftItemTransferByBranchID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.GetDraftItemTransferByCriteria.v1;
public class GetDraftItemTransferByCriteriatHandler : BaseService, IRequestHandler<GetDraftItemTransferByCriteriaQuery, BaseResponse<List<GetDraftItemTransferByBranchIDResponseDTO>>>
{
    public GetDraftItemTransferByCriteriatHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetDraftItemTransferByBranchIDResponseDTO>>> Handle(GetDraftItemTransferByCriteriaQuery request, CancellationToken cancellationToken)
    {
        DateTime startDate = request.transferdate.HasValue ? request.transferdate.Value : DateTime.Now;
        var resData = (from header in await _unitOfWork.Repository<TTDraftItemTransfer>().FindWithInclude(w => w.IsActive, i => i.Include(s => s.TTDraftItemTransferDetails))
                       join b in await _unitOfWork.Repository<TMBranch>().QueryAsync() on header.DestinationBranchID equals b.BranchID into branch
                       from jBranch in branch.DefaultIfEmpty()
                       where header.CreatedDate.Date >= startDate.Date
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
                       }).AsEnumerable();

        if (!resData.Any())
        {
            throw new Exception("ไม่พบข้อมูล");
        }

        //Filter enddate
        if (request.transferenddate.HasValue)
        {
            resData = resData.Where(w => w.createddate.Date <= request.transferenddate.Value.Date);
        }

        //Filter branchid
        if (request.branchid.HasValue)
        {
            if (request.branchid <= 0)
            {
                throw new Exception("ข้อมูลสาขาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
            }
            resData = resData.Where(w => w.destinationbranchid == request.branchid.Value);
        }

        if (!resData.Any())
        {
            throw new Exception("ไม่พบข้อมูล");
        }

        return new BaseResponse<List<GetDraftItemTransferByBranchIDResponseDTO>>
        {
            result = true,
            data = resData.ToList(),
            message = " Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
