using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ReportService.Queries.ItemTransactionLogReport.v1;
public class ItemTransactionLogReportHandler : BaseService, IRequestHandler<ItemTransactionLogReportQuery, BaseResponse<List<ItemTransactionLogReportResponseDTO>>>
{
    public ItemTransactionLogReportHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<ItemTransactionLogReportResponseDTO>>> Handle(ItemTransactionLogReportQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TTItemTransactionLog> itemTransactionLogs;
        if (request.branchid.HasValue)
        {
            itemTransactionLogs = await _unitOfWork.Repository<TTItemTransactionLog>().QueryAsync(w => w.BranchID == request.branchid && w.IsActive);
        }
        else
        {
            itemTransactionLogs = await _unitOfWork.Repository<TTItemTransactionLog>().QueryAsync(w => w.IsActive);
        }

        int itemSeq = 1;
        var resData = (from a in itemTransactionLogs.ToList()
                       join b in await _unitOfWork.Repository<TMItem>().FindWithInclude(w => w.IsActive, i => i.Include(s => s.ItemType), i => i.Include(s => s.Brand)) on a.ItemID equals b.ItemID into jItem
                       from item in jItem.DefaultIfEmpty()
                       join c in await _unitOfWork.Repository<TMBranch>().QueryAsync() on a.BranchID equals c.BranchID into jBranch
                       from branch in jBranch.DefaultIfEmpty()
                       select new ItemTransactionLogReportResponseDTO
                       {
                           seq = itemSeq++,
                           itemid = a.ItemID,
                           itemcode = item.ItemCode,
                           itemname = item.Name,
                           itemtypeid = item.ItemTypeID,
                           itemtypename = item.ItemType.ItemTypeName,
                           branchid = a.BranchID,
                           branchname = branch != null ? branch.BranchName : "N/A",
                           oldprice = a.OldPrice,
                           newprice = a.NewPrice,
                           brandid = item.BrandID,
                           brandname = item.Brand.BrandName,
                           description = item.Description,
                           barcode = item.BarCode,
                           price = item.Price,
                           qty = item.Qty,
                           notifyminqty = item.NotifyMinQty,
                           notifymaxqty = item.NotifyMaxQty,
                           createdby = a.CreatedBy,
                           createddate = a.CreatedDate,
                           updatedby = a.UpdatedBy,
                           updateddate = a.UpdatedDate
                       }).ToList();

        if (resData.Count == 0)
        {
            throw new Exception("Data not found");
        }

        #region Update updatedby data from emp name
        List<string> userNameList = resData.Select(s => s.createdby).ToList();
        IEnumerable<TMUsers> userList = await _unitOfWork.Repository<TMUsers>().FindWithInclude(w => userNameList.Contains(w.UserName), i => i.Include(w => w.TMEmployees));
        var empDataList = userList.Select(s => new { s.UserName, s.TMEmployees.FirstOrDefault().FirstName }).ToList();
        resData = resData.Select(s =>
        {
            if (!string.IsNullOrEmpty(s.createdby))
            {
                s.createdby = empDataList.FirstOrDefault(w => w.UserName == s.createdby) != null
                ? empDataList.FirstOrDefault(w => w.UserName == s.createdby).FirstName : s.createdby;
            }

            if (!string.IsNullOrEmpty(s.updatedby))
            {
                s.updatedby = empDataList.FirstOrDefault(w => w.UserName == s.updatedby) != null
                ? empDataList.FirstOrDefault(w => w.UserName == s.updatedby).FirstName : s.updatedby;
            }
            return s;
        }).ToList();
        #endregion

        return new BaseResponse<List<ItemTransactionLogReportResponseDTO>>
        {
            result = true,
            data = resData,
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
