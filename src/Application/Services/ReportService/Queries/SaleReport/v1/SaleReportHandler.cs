using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;
public class SaleReportHandler : BaseService, IRequestHandler<SaleReportQuery, BaseResponse<SaleReportResponseDTO>>
{
    public SaleReportHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<SaleReportResponseDTO>> Handle(SaleReportQuery request, CancellationToken cancellationToken)
    {
        int totalRowCount = 0;
        IQueryable<SaleReportResponseDetailDTO> searchData = (from tran in await _unitOfWork.Repository<TTTransaction>().QueryAsync(tran => tran.IsActive && (tran.TransactionDate.Date >= request.transaction_startdate.Date && tran.TransactionDate.Date <= request.transaction_enddate.Date))
                                                              join detail in await _unitOfWork.Repository<TTTransactonDetail>().QueryAsync() on tran.TransactionID equals detail.TransactionID
                                                              //join itembranch in await _unitOfWork.Repository<TMItemInBranch>().QueryAsync() on detail.ItemID equals itembranch.ItemID into jitembranch
                                                              //from tmitembranch in jitembranch.DefaultIfEmpty()
                                                              //join item in await _unitOfWork.Repository<TMItem>().QueryAsync() on tmitembranch.ItemID equals item.ItemID into jitem
                                                              //from tmitem in jitem.DefaultIfEmpty()
                                                              //join itembrand in await _unitOfWork.Repository<TMItemBrand>().QueryAsync() on tmitem.BrandID equals itembrand.BrandID
                                                              //join branch in await _unitOfWork.Repository<TMBranch>().QueryAsync() on tran.BranchID equals branch.BranchID
                                                              select new SaleReportResponseDetailDTO
                                                              {
                                                                  transactionid = tran.TransactionID,
                                                                  transactiondate = tran.TransactionDate,
                                                                  itemid = detail.ItemID,
                                                                  //itemcode = tmitem.ItemCode,
                                                                  //itemname = tmitem.Name,
                                                                  //brandid = tmitem.BrandID,
                                                                  //brandname = itembrand.BrandName,
                                                                  qty = detail.Qty,
                                                                  //unitprice = tmitembranch.Price,
                                                                  amounttransfer = tran.AmountTransfer,
                                                                  amountdeposit = tran.AmountDeposit,
                                                                  depositfee = tran.Fee,
                                                                  amountcash = tran.AmountCash,
                                                                  totalamount = tran.TotalAmount,
                                                                  branchid = tran.BranchID,
                                                                  //branchname = branch.BranchName,
                                                                  createdby = tran.CreatedBy,
                                                                  createddate = tran.CreatedDate,
                                                              }).AsQueryable();
        if (request.branchid.HasValue)
        {
            searchData = searchData.Where(w => w.branchid == request.branchid.Value);
        }

        //if (!string.IsNullOrEmpty(request.searchvalue))
        //{
        //    searchData = searchData.Where(w => w.itemname.Contains(request.searchvalue)
        //        || w.itemcode.Contains(request.searchvalue)
        //        || w.branchname.Contains(request.searchvalue)
        //        || w.brandname.Contains(request.searchvalue)
        //        || w.createdby.Contains(request.searchvalue));
        //}

        totalRowCount = searchData.Count();
        List<SaleReportResponseDetailDTO> resData = new List<SaleReportResponseDetailDTO>();
        if (request.isexportalldata)
        {
            resData = searchData.ToList();
        }
        else
        {
            resData = searchData.ToList().Skip(request.startrow).Take(request.pagesize).ToList();
        }
        //List<SaleReportResponseDetailDTO> resData = searchData.ToList().Skip(request.startrow).Take(request.pagesize).ToList();
        if (!resData.Any())
        {
            throw new Exception("ไม่พบข้อมูลรายงานขายสินค้า");
        }

        #region Prepare all master data
        List<int> branchIdList = resData.Select(s => s.branchid).Distinct().ToList();
        List<int> itemIdList = resData.Select(s => s.itemid).Distinct().ToList();
        //var resxxx = await _unitOfWork.Repository<TMItemInBranch>().FindListAsync(w => branchIdList.Contains(w.BranchID) && itemIdList.Contains(w.ItemID));
        //var _resxxx = resxxx.ToList();
        List<SaleReportResponseDetailDTO> itemsInBranch = (from a in await _unitOfWork.Repository<TMItemInBranch>().QueryAsync()
                                                           join i in await _unitOfWork.Repository<TMItem>().QueryAsync() on a.ItemID equals i.ItemID
                                                           join c in await _unitOfWork.Repository<TMItemBrand>().QueryAsync() on i.BrandID equals c.BrandID
                                                           where branchIdList.Contains(a.BranchID) && itemIdList.Contains(a.ItemID)
                                                           select new SaleReportResponseDetailDTO
                                                           {
                                                               itemid = a.ItemID,
                                                               branchid = a.BranchID,
                                                               itemcode = i.ItemCode,
                                                               itemname = i.Name,
                                                               brandid = a.BranchID,
                                                               branchname = c.BrandName,
                                                               unitprice = a.Price
                                                           }).ToList();
        //IEnumerable<TMBranch> branchList = await _unitOfWork.Repository<TMBranch>().FindListAsync(w => resData.Select(s => s.branchid).Contains(w.BranchID));

        List<SaleReportResponseDetailDTO> branchList = (from a in await _unitOfWork.Repository<TMBranch>().QueryAsync()
                                                        where branchIdList.Contains(a.BranchID)
                                                        select new SaleReportResponseDetailDTO
                                                        {
                                                            branchid = a.BranchID,
                                                            branchname = a.BranchName,
                                                        }).ToList();
        foreach (var data in resData)
        {
            SaleReportResponseDetailDTO itembranch = itemsInBranch.FirstOrDefault(w => w.itemid == data.itemid && w.branchid == data.branchid);
            SaleReportResponseDetailDTO branch = branchList.FirstOrDefault(w => w.branchid == data.branchid);
            data.itemcode = itembranch.itemcode;
            data.itemname = itembranch.itemname;
            data.brandid = itembranch.brandid;
            data.brandname = itembranch.brandname;
            data.unitprice = itembranch.unitprice;
            data.branchname = branch.branchname;
        }
        #endregion

        #region Update updatedby data from emp name
        List<string> userNameList = resData.Select(s => s.createdby).Distinct().ToList();
        IEnumerable<TMUsers> userList = await _unitOfWork.Repository<TMUsers>().FindWithInclude(w => userNameList.Contains(w.UserName), i => i.Include(w => w.TMEmployees));
        var empDataList = userList.Select(s => new { s.UserName, s.TMEmployees.FirstOrDefault().FirstName }).ToList();
        resData = resData.Select(s =>
        {
            if (!string.IsNullOrEmpty(s.createdby))
            {
                s.createdbystaff = empDataList.FirstOrDefault(w => w.UserName == s.createdby) != null
                ? empDataList.FirstOrDefault(w => w.UserName == s.createdby).FirstName : s.createdby;
            }
            return s;
        }).ToList();
        #endregion

        return new BaseResponse<SaleReportResponseDTO>
        {
            result = true,
            data = new SaleReportResponseDTO
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
