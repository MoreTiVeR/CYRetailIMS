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
using static CYRetailIMS.Application.Common.Models.EnumModel;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
public class GetItemTransferByTransferIDHandler : BaseService, IRequestHandler<GetItemTransferByTransferIDQuery, BaseResponse<GetItemTransferResponseDTO>>
{
    public GetItemTransferByTransferIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetItemTransferResponseDTO>> Handle(GetItemTransferByTransferIDQuery request, CancellationToken cancellationToken)
    {
        List<GetItemTransferResponseDTO> resItemTransfer = (from a in await _unitOfWork.Repository<TTItemTransfer>().QueryAsync()
                                                            join item in await _unitOfWork.Repository<TMItem>().QueryAsync() on a.ItemID equals item.ItemID
                                                            join b in await _unitOfWork.Repository<TMItemTransferStatus>().QueryAsync() on a.TransferStatus equals b.TransferStatusID
                                                            join c in await _unitOfWork.Repository<TMTransferType>().QueryAsync() on a.TransferTypeID equals c.TransferTypeID
                                                            join emp in await _unitOfWork.Repository<TMEmployee>().FindWithInclude(w => w.IsActive, i => i.Include(ic => ic.User)) 
                                                            on a.CreatedBy equals emp.User.UserName into tUser
                                                            from jUser in tUser.DefaultIfEmpty()
                                                            where a.TransferID == request.transferid && a.IsActive
                                                            select new GetItemTransferResponseDTO
                                                            {
                                                                transferid = a.TransferID,
                                                                transfertypeid = a.TransferTypeID,
                                                                transfertypename = c.TransferTypeName,
                                                                description = a.Description,
                                                                sourceid = a.SourceID,
                                                                destinationid = a.DestinationID,
                                                                createddate = a.CreadedDate,
                                                                createdby = jUser != null ? jUser.FirstName : "N/A",
                                                                transferstatusid = b.TransferStatusID,
                                                                transferstatusname_th = b.TransferStatusName_TH,
                                                                transferstatusname_en = b.TransferStatusName_EN,
                                                                itemid = a.ItemID,
                                                                itemname = item.Name,
                                                                qty = a.Qty
                                                            }).ToList();

        if (!resItemTransfer.Any())
        {
            throw new Exception("ไม่พบรายการโอนสินค้า");
        }

        //Get TMApproveStatus list
        List<TMBranch> resBranchList = _unitOfWork.Repository<TMBranch>().Where(w =>
        resItemTransfer.Select(s => s.sourceid).Distinct().Contains(w.BranchID)
        || resItemTransfer.Select(s => s.destinationid).Distinct().Contains(w.BranchID)).Distinct().ToList();

        //Update updatedby data from emp name
        List<string> userNameList = resItemTransfer.Select(s => s.updatedby).ToList();
        IEnumerable<TMUsers> userList = await _unitOfWork.Repository<TMUsers>().FindWithInclude(w => userNameList.Contains(w.UserName), i => i.Include(w => w.TMEmployees));
        var empDataList = userList.Select(s => new { s.UserName, s.TMEmployees.FirstOrDefault().FirstName }).ToList();

        resItemTransfer.ForEach(e =>
        {
            string sourceBrachName = e.sourceid == (int)TransferSource.WAREHOUSE ? "สำนักงานใหญ่" : resBranchList.FirstOrDefault(w => w.BranchID == e.sourceid).BranchName;
            string destinationBrachName = resBranchList.FirstOrDefault(w => w.BranchID == e.destinationid)?.BranchName;
            e.sourcename = sourceBrachName;
            e.destinationname = destinationBrachName;


            if (!string.IsNullOrEmpty(e.updatedby))
            {
                e.updatedby = empDataList.FirstOrDefault(w => w.UserName == e.updatedby) != null
                ? empDataList.FirstOrDefault(w => w.UserName == e.updatedby).FirstName : e.updatedby;
            }
        });

        return new BaseResponse<GetItemTransferResponseDTO>
        {
            result = true,
            data = resItemTransfer.FirstOrDefault(),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
