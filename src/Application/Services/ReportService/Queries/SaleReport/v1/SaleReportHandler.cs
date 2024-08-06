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

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;
public class SaleReportHandler : BaseService, IRequestHandler<SaleReportQuery, BaseResponse<List<SaleReportResponseDTO>>>
{
    public SaleReportHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<SaleReportResponseDTO>>> Handle(SaleReportQuery request, CancellationToken cancellationToken)
    {
        IQueryable<SaleReportResponseDTO> searchData = (from tran in await _unitOfWork.Repository<TTTransaction>().QueryAsync(tran => tran.IsActive && (tran.TransactionDate.Date >= request.transaction_startdate.Date && tran.TransactionDate.Date <= request.transaction_enddate.Date))
                                                        join detail in await _unitOfWork.Repository<TTTransactonDetail>().QueryAsync() on tran.TransactionID equals detail.TransactionID
                                                        select new SaleReportResponseDTO
                                                        {
                                                            transactionid = tran.TransactionID,
                                                            transactiondate = tran.TransactionDate,
                                                            itemid = detail.ItemID,
                                                            //itemcode = item.ItemCode,
                                                            //itemname = item.Name,
                                                            //brandid = item.BrandID,
                                                            //brandname = brand.BrandName,
                                                            qty = detail.Qty,
                                                            //unitprice = item.Price,
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

        List<SaleReportResponseDTO> resData = searchData.ToList();
        if (!resData.Any())
        {
            throw new Exception("ไม่พบข้อมูลรายงานขายสินค้า");
        }

        #region Master data
        var items = await _unitOfWork.Repository<TMItem>().FindWithInclude(w => resData.Select(s => s.itemid).Contains(w.ItemID),
            i => i.Include(s => s.Brand));
        var itemList = items.ToList();
        var branchList = await _unitOfWork.Repository<TMBranch>().FindListAsync(w => resData.Select(s => s.branchid).Contains(w.BranchID));
        foreach (var data in resData)
        {
            var item = itemList?.FirstOrDefault(w => w.ItemID == data.itemid);
            var branch = branchList.FirstOrDefault(w => w.BranchID == data.branchid);
            data.itemcode = item.ItemCode;
            data.itemname = item.Name;
            data.brandid = item.BrandID;
            data.brandname = item.Brand.BrandName;
            data.unitprice = item.Price;
            data.branchname = branch.BranchName;
        }
        #endregion

        #region Before 7/8/2024
        //IEnumerable<SaleReportResponseDTO> resData = (from tran in await _unitOfWork.Repository<TTTransaction>().QueryAsync()
        //                                              join detail in await _unitOfWork.Repository<TTTransactonDetail>().QueryAsync() on tran.TransactionID equals detail.TransactionID
        //                                              join item in await _unitOfWork.Repository<TMItem>().QueryAsync() on detail.ItemID equals item.ItemID
        //                                              join branch in await _unitOfWork.Repository<TMBranch>().QueryAsync() on tran.BranchID equals branch.BranchID
        //                                              join brand in await _unitOfWork.Repository<TMItemBrand>().QueryAsync() on item.BrandID equals brand.BrandID
        //                                              where tran.IsActive
        //                                              //&& tran.BranchID == request.branchid
        //                                              && (tran.TransactionDate.Date >= request.transaction_startdate.Date && tran.TransactionDate.Date <= request.transaction_enddate.Date)
        //                                              select new SaleReportResponseDTO
        //                                              {
        //                                                  transactionid = tran.TransactionID,
        //                                                  transactiondate = tran.TransactionDate,
        //                                                  itemcode = item.ItemCode,
        //                                                  itemname = item.Name,
        //                                                  brandid = item.BrandID,
        //                                                  brandname = brand.BrandName,
        //                                                  qty = detail.Qty,
        //                                                  unitprice = item.Price,
        //                                                  amounttransfer = tran.AmountTransfer,
        //                                                  amountdeposit = tran.AmountDeposit,
        //                                                  depositfee = tran.Fee,
        //                                                  amountcash = tran.AmountCash,
        //                                                  totalamount = tran.TotalAmount,
        //                                                  branchid = tran.BranchID,
        //                                                  branchname = branch.BranchName,
        //                                                  createdby = tran.CreatedBy,
        //                                                  createddate = tran.CreatedDate,
        //                                                  //createdbystaff = "N/A"
        //                                              }).AsEnumerable();
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

        return new BaseResponse<List<SaleReportResponseDTO>>
        {
            result = true,
            data = resData.ToList(),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };

    }
}
