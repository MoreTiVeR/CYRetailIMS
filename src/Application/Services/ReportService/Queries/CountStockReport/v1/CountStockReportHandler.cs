using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStocks.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ReportService.Queries.CountStockReport.v1;
public class CountStockReportHandler : BaseService, IRequestHandler<CountStockReportQuery, BaseResponse<List<CountStockReportResponseDTO>>>
{
    public CountStockReportHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<CountStockReportResponseDTO>>> Handle(CountStockReportQuery request, CancellationToken cancellationToken)
    {
        var resCountStockEntities = (from a in await _unitOfWork.Repository<TTCountStock>().QueryAsync()
                                     join b in await _unitOfWork.Repository<TTCountStockDetail>().QueryAsync() on a.CountStockID equals b.CountStockID
                                     //join c in await _unitOfWork.Repository<TMSubItemType>().QueryAsync(w => w.IsActive) on b.SubItemTypeID equals c.SubItemTypeID
                                     join subitem in await _unitOfWork.Repository<TMSubItemType>().QueryAsync() on b.SubItemTypeID equals subitem.SubItemTypeID
                                     into jSubitemType
                                     from c in jSubitemType.DefaultIfEmpty()
                                     join d in await _unitOfWork.Repository<TMBranch>().QueryAsync(w => w.IsActive) on a.BranchID equals d.BranchID
                                     select new CountStockReportResponseDTO
                                     {
                                         countstockid = a.CountStockID,
                                         countstockdate = a.CreatedDate,
                                         branchid = a.BranchID,
                                         branchname = d.BranchName,
                                         countstockdetailid = b.CountStockDetailID,
                                         subitemtypeid = b.SubItemTypeID,
                                         subitemtypename = c != null ? c.SubTypeNameTH : "ไม่มีประเภทย่อย",
                                         qtyinbranch = b.QtyInBranch,
                                         qtyinbranchofcountstockday = b.QtyInBranchOfCountStockDay,
                                         countedamountqty = b.CountedAmountQty,
                                         pendingrestockqty = b.PendingReStockQty,
                                         damagedqty = b.DamagedQty,
                                         salebeforecountqty = b.SaleBeforeCountQty,
                                         totalcount = b.TotalCountQty,
                                         remark = a.Remark,
                                         createdby = a.CreatedBy,
                                         createddate = a.CreatedDate,
                                         isactive = b.IsActive
                                     }).AsQueryable();

        if (request.branchid > 0)
        {
            resCountStockEntities = resCountStockEntities.Where(w => w.branchid == request.branchid);
        }

        if(request.subitemtypeid.HasValue && request.subitemtypeid > 0)
        {
            resCountStockEntities = resCountStockEntities.Where(w => w.subitemtypeid == request.subitemtypeid);
        }

        if (request.startdate.HasValue)
        {
            resCountStockEntities = resCountStockEntities.Where(w => w.createddate.Date >= request.startdate.Value.Date);
        }

        if (request.enddate.HasValue)
        {
            resCountStockEntities = resCountStockEntities.Where(w => w.createddate.Date <= request.enddate.Value.Date);
        }

        if (!resCountStockEntities.Any())
        {
            throw new Exception("ไม่พบข้อมูล");
        }

        List<CountStockReportResponseDTO> resCountStockList = resCountStockEntities.ToList();

        #region Update updatedby data from emp name
        List<string> userNameList = resCountStockList.ToList().Select(s => s.createdby).Distinct().ToList();
        IEnumerable<TMUsers> userList = await _unitOfWork.Repository<TMUsers>().FindWithInclude(w => userNameList.Contains(w.UserName), i => i.Include(w => w.TMEmployees));
        var empDataList = userList.Select(s => new { s.UserName, name = $"{s.TMEmployees.FirstOrDefault().FirstName} {s.TMEmployees.FirstOrDefault().LastName}" }).ToList();
        resCountStockList = resCountStockList.Select(s =>
        {
            if (!string.IsNullOrEmpty(s.createdby))
            {
                var empData = empDataList.FirstOrDefault(w => w.UserName == s.createdby);
                s.createdby = empData != null ? empData.name : s.createdby;
            }

            return s;
        }).OrderBy(s => s.countstockdate).ThenBy(o => o.branchid).ToList();
        #endregion

        return new BaseResponse<List<CountStockReportResponseDTO>>
        {
            result = true,
            data = resCountStockList,
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
