using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static CYRetailIMS.Application.Common.Models.EnumModel;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByDestinationBranchID.v1;
public class GetItemTransferByCriteriaHandler : BaseService, IRequestHandler<GetItemTransferByCriteriaQuery, BaseResponse<List<GetItemTransferResponseDTO>>>
{
    public GetItemTransferByCriteriaHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetItemTransferResponseDTO>>> Handle(GetItemTransferByCriteriaQuery request, CancellationToken cancellationToken)
    {
        List<GetItemTransferResponseDTO> resItemTransfer = (from a in await _unitOfWork.Repository<TTItemTransfer>().QueryAsync()
                                                            join item in await _unitOfWork.Repository<TMItem>().QueryAsync() on a.ItemID equals item.ItemID
                                                            join b in await _unitOfWork.Repository<TMItemTransferStatus>().QueryAsync() on a.TransferStatus equals b.TransferStatusID
                                                            join c in await _unitOfWork.Repository<TMTransferType>().QueryAsync() on a.TransferTypeID equals c.TransferTypeID
                                                            where a.DestinationID == request.destinationbranchid && a.ItemID == request.itemid
                                                            && a.IsActive
                                                            select new GetItemTransferResponseDTO
                                                            {
                                                                transferid = a.TransferID,
                                                                transfertypeid = a.TransferTypeID,
                                                                transfertypename = c.TransferTypeName,
                                                                description = a.Description,
                                                                sourceid = a.SourceID,
                                                                destinationid = a.DestinationID,
                                                                creadeddate = a.CreadedDate,
                                                                createdby = a.CreatedBy,
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

        resItemTransfer.ForEach(e =>
        {
            string sourceBrachName = e.sourceid == (int)TransferSource.WAREHOUSE ? "สำนักงานใหญ่" : resBranchList.FirstOrDefault(w => w.BranchID == e.sourceid).BranchName;
            string destinationBrachName = resBranchList.FirstOrDefault(w => w.BranchID == e.destinationid)?.BranchName;
            e.sourcename = sourceBrachName;
            e.destinationname = destinationBrachName;
        });

        return new BaseResponse<List<GetItemTransferResponseDTO>>
        {
            result = true,
            data = resItemTransfer.OrderByDescending(w => w.creadeddate).ToList(),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
