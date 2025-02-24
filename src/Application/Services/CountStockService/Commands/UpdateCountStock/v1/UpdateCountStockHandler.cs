using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using EFCore.BulkExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.UpdateCountStock.v1;
public class UpdateCountStockHandler : BaseService, IRequestHandler<UpdateCountStockCommand, BaseResponse<CommandResponse>>
{
    public UpdateCountStockHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(UpdateCountStockCommand request, CancellationToken cancellationToken)
    {
        TTCountStock resCountStockEnt = await _unitOfWork.Repository<TTCountStock>().FirstOrDefaultAsync(w => w.CountStockID == request.countstockid);
        if(resCountStockEnt == null)
        {
            throw new Exception("ไม่พบข้อมูลนับสต๊อก");
        }

        var bulkConfig = new BulkConfig { SetOutputIdentity = true, BatchSize = 4000 };
        List<TTCountStock> countStockEntity = PrepareUpdateData(request, 
            resCountStockEnt.CountDate, 
            resCountStockEnt.CreatedBy, 
            resCountStockEnt.CreatedDate);
        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.Repository<TTCountStock>().BulkInsertOrUpdateAsync(countStockEntity, bulkConfig);
        await _unitOfWork.Repository<TTCountStockDetail>().BulkInsertOrUpdateAsync(countStockEntity.FirstOrDefault().TTCountStockDetails.ToList(), bulkConfig);
        await _unitOfWork.CommitTransactionAsync();
        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse { result = true },
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }

    private List<TTCountStock> PrepareUpdateData(UpdateCountStockCommand stockCommand,
        DateTime countStockDate, 
        string createdBy,
        DateTime createdDate)
    {
        DateTime updatedDate = DateTime.Now;
        TTCountStock updateEnt = new TTCountStock
        {
            CountStockID = stockCommand.countstockid,
            BranchID = stockCommand.branchid,
            TotalCount = stockCommand.totalcount,
            Remark = stockCommand.remark,
            CountDate = countStockDate,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            UpdatedDate = updatedDate,
            UpdatedBy = stockCommand.updatedby,
            TTCountStockDetails = stockCommand.detail.Select(s => new TTCountStockDetail
            {
                CountStockDetailID = s.countstockdetailid,
                SubItemTypeID = s.subitemtypeid,
                QtyInBranchOfCountStockDay = s.qtyinbranchofcountstockday,
                CountStockID = stockCommand.countstockid,
                QtyInBranch = s.qtyinbranch,
                CountedAmountQty = s.countedamountqty,
                PendingReStockQty = s.pendingrestockqty,
                DamagedQty = s.damagedqty,
                SaleBeforeCountQty = s.salebeforecountqty,
                TotalCountQty = s.totalcountqty,
                ShortageSurplusQty = s.shortagesurplusqty,
                CreatedBy = createdBy,
                CreatedDate = createdDate,
                UpdatedBy = stockCommand.updatedby,
                UpdatedDate = updatedDate,
            }).ToList()
        };
        return new List<TTCountStock> { updateEnt };
    }
}
