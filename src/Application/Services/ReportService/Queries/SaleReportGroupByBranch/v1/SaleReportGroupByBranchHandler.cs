using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleReportGroupByBranch.v1;
public class SaleReportGroupByBranchHandler : BaseService, IRequestHandler<SaleReportGroupByBranchQuery, BaseResponse<SaleReportGroupByBranchResposneDTO>>
{
    public SaleReportGroupByBranchHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<SaleReportGroupByBranchResposneDTO>> Handle(SaleReportGroupByBranchQuery request, CancellationToken cancellationToken)
    {
        int totalRowCount = 0;
        List<SaleReportGroupByBranchDetailDTO> resData = new List<SaleReportGroupByBranchDetailDTO>();

        // ถ้ามีการค้นหาโดยสาขา group by ด้วย transactiondate, itemcode
        if (request.branchid.HasValue)
        {
            IQueryable<SaleReportGroupByBranchDetailDTO> searchData = (from tran in await _unitOfWork.Repository<TTTransaction>().QueryAsync(tran => tran.IsActive && (tran.TransactionDate.Date >= request.transaction_startdate.Date && tran.TransactionDate.Date <= request.transaction_enddate.Date))
                                                                       join detail in await _unitOfWork.Repository<TTTransactonDetail>().QueryAsync() on tran.TransactionID equals detail.TransactionID
                                                                       join iteminbranch in await _unitOfWork.Repository<TMItemInBranch>().QueryAsync() on new { detail.ItemID, tran.BranchID } equals new { iteminbranch.ItemID, iteminbranch.BranchID } into jitembranch
                                                                       from itembranch in jitembranch.DefaultIfEmpty()
                                                                           //join item in await _unitOfWork.Repository<TMItem>().QueryAsync() on itembranch.ItemID equals item.ItemID
                                                                           //from tmitem in jitem.DefaultIfEmpty()
                                                                           //join itembrand in await _unitOfWork.Repository<TMItemBrand>().QueryAsync() on tmitem.BrandID equals itembrand.BrandID
                                                                       join ibranch in await _unitOfWork.Repository<TMBranch>().QueryAsync() on tran.BranchID equals ibranch.BranchID into jbranch
                                                                       from branch in jbranch.DefaultIfEmpty()
                                                                       where tran.BranchID == request.branchid.Value
                                                                       select new SaleReportGroupByBranchDetailDTO
                                                                       {
                                                                           transactiondate = tran.TransactionDate.Date,
                                                                           branchid = tran.BranchID,
                                                                           branchname = branch.BranchName,
                                                                           itemcode = itembranch.Item.ItemCode,
                                                                           itemname = itembranch.Item.Name,
                                                                           brandid = itembranch.Item.BrandID,
                                                                           brandname = itembranch.Item.Brand.BrandName,
                                                                           totalsaleqty = detail.Qty,
                                                                           itempriceinbranch = detail.Price
                                                                       }).AsQueryable();

            if (!string.IsNullOrEmpty(request.searchvalue))
            {
                searchData = searchData.Where(w => w.brandname.Contains(request.searchvalue)
                || w.itemcode.Contains(request.searchvalue)
                || w.itemname.Contains(request.searchvalue)
                || w.brandname.Contains(request.searchvalue));
            }

            var groupData = searchData.ToList().GroupBy(g => new { g.transactiondate.Date, g.itemcode }).Select(s => new
            {
                transactiondate = s.Key.Date,
                itemcode = s.Key.itemcode,
                data = s
            }).Select(s => new SaleReportGroupByBranchDetailDTO
            {
                transactiondate = s.transactiondate,
                branchid = s.data.FirstOrDefault().branchid,
                branchname = s.data.FirstOrDefault().branchname,
                itemcode = s.itemcode,
                itemname = s.data.FirstOrDefault(w => w.itemcode == s.itemcode && w.transactiondate == s.transactiondate).itemname,
                brandid = s.data.FirstOrDefault(w => w.itemcode == s.itemcode && w.transactiondate == s.transactiondate).brandid,
                brandname = s.data.FirstOrDefault(w => w.itemcode == s.itemcode && w.transactiondate == s.transactiondate).brandname,
                totalsaleqty = s.data.Where(w => w.itemcode == s.itemcode && w.transactiondate == s.transactiondate).Sum(w => w.totalsaleqty),
                itempriceinbranch = s.data.FirstOrDefault(w => w.itemcode == s.itemcode && w.transactiondate == s.transactiondate).itempriceinbranch
            }).OrderBy(x => x.transactiondate).ToList();

            #region Filter
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
                throw new Exception("ไม่พบข้อมูลรายงานขายสินค้า");
            }
            #endregion
        }
        else
        {
            IQueryable<SaleReportGroupByBranchDetailDTO> searchAllData = (from tran in await _unitOfWork.Repository<TTTransaction>().QueryAsync(tran => tran.IsActive && (tran.TransactionDate.Date >= request.transaction_startdate.Date && tran.TransactionDate.Date <= request.transaction_enddate.Date))
                                                                          join detail in await _unitOfWork.Repository<TTTransactonDetail>().QueryAsync() on tran.TransactionID equals detail.TransactionID
                                                                          //join iteminbranch in await _unitOfWork.Repository<TMItemInBranch>().QueryAsync() on new { detail.ItemID, tran.BranchID } equals new { iteminbranch.ItemID, iteminbranch.BranchID } into jitembranch
                                                                          //from itembranch in jitembranch.DefaultIfEmpty()
                                                                              //join item in await _unitOfWork.Repository<TMItem>().QueryAsync() on itembranch.ItemID equals item.ItemID
                                                                              //from tmitem in jitem.DefaultIfEmpty()
                                                                              //join itembrand in await _unitOfWork.Repository<TMItemBrand>().QueryAsync() on tmitem.BrandID equals itembrand.BrandID
                                                                          //join ibranch in await _unitOfWork.Repository<TMBranch>().QueryAsync() on tran.BranchID equals ibranch.BranchID into jbranch
                                                                          //from branch in jbranch.DefaultIfEmpty()
                                                                          select new SaleReportGroupByBranchDetailDTO
                                                                          {
                                                                              transactiondate = tran.TransactionDate.Date,
                                                                              branchid = tran.BranchID,
                                                                              //branchname = branch.BranchName,
                                                                              itemid = detail.ItemID,
                                                                              //itemcode = itembranch.Item.ItemCode,
                                                                              //itemname = itembranch.Item.Name,
                                                                              //brandid = itembranch.Item.BrandID,
                                                                              //brandname = itembranch.Item.Brand.BrandName,
                                                                              totalsaleqty = detail.Qty,
                                                                              itempriceinbranch = detail.Price
                                                                          }).AsQueryable();

            resData = searchAllData.ToList();

            #region Prepare itemcode, itemname, branchname, brandid, brandname before return final result
            List<int> itemids = resData.Select(s => s.itemid).Distinct().ToList();
            IEnumerable<TMItem> itemsList = await _unitOfWork.Repository<TMItem>().FindWithInclude(w => itemids.Contains(w.ItemID), i => i.Include(ss => ss.Brand));
            var searchItemObj = itemsList.Select(s => new
            {
                itemid = s.ItemID,
                itemcode = s.ItemCode,
                itemname = s.Name,
                brandid = s.BrandID,
                brandname = s.Brand.BrandName
            }).ToList();

            List<int> branchids = resData.Select(s => s.branchid).Distinct().ToList();
            IEnumerable<TMBranch> branchList = await _unitOfWork.Repository<TMBranch>().FindListAsync(w => branchids.Contains(w.BranchID));
            var searchBranchObj = branchList.Select(s => new
            {
                branchid = s.BranchID,
                branchname = s.BranchName
            }).ToList();
            #endregion

            resData.ForEach(e =>
            {
                var itemdata = searchItemObj.FirstOrDefault(w => w.itemid == e.itemid);
                var branchdata = searchBranchObj.FirstOrDefault(w => w.branchid == e.branchid);
                e.branchname = branchdata?.branchname;
                e.itemcode = itemdata?.itemcode;
                e.itemname = itemdata?.itemname;
                e.brandid = itemdata?.brandid;
                e.brandname = itemdata?.brandname;
            });

            if (!string.IsNullOrEmpty(request.searchvalue))
            {
                resData = resData.Where(w => w.brandname.Contains(request.searchvalue)
                || w.itemcode.Contains(request.searchvalue)
                || w.itemname.Contains(request.searchvalue)
                || w.brandname.Contains(request.searchvalue)).ToList();
            }

            // ค้นหาแบบทั้งหมด ทุกสาขา groupby ด้วย transactiondate, branchid ,itemcode
            var groupData = resData.GroupBy(g => new { g.transactiondate, g.branchid, g.itemid }).Select(s => new
            {
                transactiondate = s.Key.transactiondate,
                branchid = s.Key.branchid,
                itemid = s.Key.itemid,
                data = s
            }).Select(s => new SaleReportGroupByBranchDetailDTO
            {
                transactiondate = s.transactiondate,
                branchid = s.data.FirstOrDefault().branchid,
                branchname = s.data.FirstOrDefault()?.branchname,
                itemid = s.itemid,
                itemcode = s.data.FirstOrDefault(w => w.itemid == s.itemid && w.branchid == s.branchid && w.transactiondate == s.transactiondate)?.itemcode,
                itemname = s.data.FirstOrDefault(w => w.itemid == s.itemid && w.branchid == s.branchid && w.transactiondate == s.transactiondate)?.itemname,
                brandid = s.data.FirstOrDefault(w => w.itemid == s.itemid && w.branchid == s.branchid && w.transactiondate == s.transactiondate)?.brandid,
                brandname = s.data.FirstOrDefault(w => w.itemid == s.itemid && w.branchid == s.branchid && w.transactiondate == s.transactiondate)?.brandname,
                totalsaleqty = s.data.Where(w => w.itemid == s.itemid && w.branchid == s.branchid && w.transactiondate == s.transactiondate).Sum(w => w.totalsaleqty),
                itempriceinbranch = s.data.FirstOrDefault(w => w.itemid == s.itemid && w.branchid == s.branchid && w.transactiondate == s.transactiondate).itempriceinbranch
            }).OrderBy(x => x.transactiondate).ThenBy(x => x.branchid).ThenBy(x => x.itemid).ToList();

            #region Filter
            totalRowCount = groupData.Count();
            if (request.isexportalldata)
            {
                resData = groupData;
            }
            else
            {
                resData = groupData.Skip(request.startrow).Take(request.pagesize).ToList();
            }
            if (!resData.Any())
            {
                throw new Exception("ไม่พบข้อมูลรายงานขายสินค้า");
            }
            #endregion

        }

        return new BaseResponse<SaleReportGroupByBranchResposneDTO>
        {
            result = true,
            data = new SaleReportGroupByBranchResposneDTO
            {
                totalrow = totalRowCount,
                transactiondata = resData
            },
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
