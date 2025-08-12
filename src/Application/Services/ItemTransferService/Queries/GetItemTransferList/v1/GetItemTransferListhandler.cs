using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static CYRetailIMS.Application.Common.Models.EnumModel;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferList.v1;

public class GetItemTransferListhandler : BaseService, IRequestHandler<GetItemTransferListQuery, BaseResponse<GetItemTransferListResponseDTO>>
{
    public GetItemTransferListhandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetItemTransferListResponseDTO>> Handle(GetItemTransferListQuery request, CancellationToken cancellationToken)
    {
        int totalRow = 0;
        var resData = (from a in await _unitOfWork.Repository<TTItemTransfer>().QueryAsync()
                       join item in await _unitOfWork.Repository<TMItem>().QueryAsync() on a.ItemID equals item.ItemID
                       join b in await _unitOfWork.Repository<TMItemTransferStatus>().QueryAsync() on a.TransferStatus equals b.TransferStatusID
                       join c in await _unitOfWork.Repository<TMTransferType>().QueryAsync() on a.TransferTypeID equals c.TransferTypeID
                       where a.IsActive
                       select new GetItemTransferResponseDTO
                       {
                           transferid = a.TransferID,
                           transfertypeid = a.TransferTypeID,
                           transfertypename = c.TransferTypeName,
                           description = a.Description,
                           sourceid = a.SourceID,
                           destinationid = a.DestinationID,
                           createddate = a.CreatedDate,
                           createdby = a.CreatedBy,
                           transferstatusid = a.TransferStatus,
                           transferstatusname_th = b.TransferStatusName_TH,
                           transferstatusname_en = b.TransferStatusName_EN,
                           itemid = a.ItemID,
                           itemname = item.Name,
                           qty = a.Qty,
                           receiveqty = a.ReceiveQTY,
                           returnqty = a.ReturnQTY,
                           updatedby = a.UpdatedBy,
                           updateddate = a.UpdatedDate
                       }).AsEnumerable();

        if (request.transferstartdate.HasValue)
        {
            resData = resData.Where(w => w.createddate.Date >= request.transferstartdate.Value.Date);
        }
        //else
        //{
        //    //Current Month
        //    resData = resData.Where(w => w.createddate.Month >= DateTime.Now.Month);
        //}

        if (request.transferenddate.HasValue)
        {
            resData = resData.Where(w => w.createddate.Date <= request.transferenddate.Value.Date);
        }

        if (request.transferstatusid.HasValue)
        {
            resData = resData.Where(w => w.transferstatusid == request.transferstatusid.Value);
        }

        if (request.branchid.HasValue)
        {
            resData = resData.Where(w => w.destinationid == request.branchid.Value);
        }

        if (!resData.Any())
        {
            throw new Exception("ไม่พบรายการโอนสินค้า");
        }

        //Assign total row
        totalRow = resData.Count();
        var Where = resData.Where(w => w.transferstatusid == 0).ToList();
        var OrderBy = resData.OrderBy(s => s.transferstatusid).ToList();

        //Assign data
        List<GetItemTransferResponseDTO> resItemTransfer = new List<GetItemTransferResponseDTO>();

        //Paging
        if (request.isexportalldata)
        {
            resItemTransfer = resData.OrderBy(s => s.transferstatusid).ThenByDescending(w => w.createddate).ToList();
        }
        else
        {
            resItemTransfer = resData.OrderBy(s => s.transferstatusid).ThenByDescending(w => w.createddate).ToList().Skip(request.startrow).Take(request.pagesize).ToList();
        }

        if (!resItemTransfer.Any())
        {
            throw new Exception("ไม่พบข้อมูลรายงานขายสินค้า");
        }

        //Get TMApproveStatus list
        List<TMBranch> resBranchList = _unitOfWork.Repository<TMBranch>().Where(w =>
        resItemTransfer.Select(s => s.sourceid).Distinct().Contains(w.BranchID)
        || resItemTransfer.Select(s => s.destinationid).Distinct().Contains(w.BranchID)).Distinct().ToList();

        //Update updatedby data from emp name
        List<string> userNameList = resItemTransfer.Select(s => s.createdby).Union(resItemTransfer.Select(s => s.updatedby)).Distinct().ToList();
        IEnumerable<TMUsers> userList = await _unitOfWork.Repository<TMUsers>().FindWithInclude(w => userNameList.Contains(w.UserName), i => i.Include(w => w.TMEmployees));
        var empDataList = userList.Select(s => new { s.UserName, s.TMEmployees.FirstOrDefault().FirstName }).ToList();


        resItemTransfer.ForEach(e =>
        {
            string sourceBrachName = e.sourceid == (int)TransferSource.WAREHOUSE ? "สำนักงานใหญ่" : resBranchList.FirstOrDefault(w => w.BranchID == e.sourceid).BranchName;
            string destinationBrachName = resBranchList.FirstOrDefault(w => w.BranchID == e.destinationid)?.BranchName;
            e.sourcename = sourceBrachName;
            e.destinationname = destinationBrachName;

            if (!string.IsNullOrEmpty(e.createdby))
            {
                e.createdby = empDataList.FirstOrDefault(w => w.UserName == e.createdby) != null
                ? empDataList.FirstOrDefault(w => w.UserName == e.createdby).FirstName : e.createdby;
            }

            if (!string.IsNullOrEmpty(e.updatedby))
            {
                e.updatedby = empDataList.FirstOrDefault(w => w.UserName == e.updatedby) != null
                ? empDataList.FirstOrDefault(w => w.UserName == e.updatedby).FirstName : e.updatedby;
            }
        });

        return new BaseResponse<GetItemTransferListResponseDTO>
        {
            result = true,
            data = new GetItemTransferListResponseDTO
            {
                totalrow = totalRow,
                transactiondata = resItemTransfer.OrderBy(s => s.transferstatusid).ThenByDescending(w => w.createddate).ToList()
            },
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
