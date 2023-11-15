using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ChartService.Queries.GetMontlySaleSummaryBarchart.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ChartService.Queries.GetMontlySaleSummaryBarchart.v2;
public class GetMontlySaleSummaryBarchartHandler : BaseService, IRequestHandler<GetMontlySaleSummaryBarchartV2Query, BaseResponse<List<GetMontlySaleSummaryBarchartResponseDTO>>>
{
    public GetMontlySaleSummaryBarchartHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetMontlySaleSummaryBarchartResponseDTO>>> Handle(GetMontlySaleSummaryBarchartV2Query request, CancellationToken cancellationToken)
    {
        DateTime dt = new DateTime(DateTime.Now.Year, request.month, 1);

        //List<GetMontlySaleSummaryBarchartResponseDTO> resData = (from a in await _unitOfWork.Repository<TTTransaction>().QueryAsync(w => w.IsActive)
        //                                                         join b in await _unitOfWork.Repository<TMBranch>().QueryAsync(w => w.IsActive) on a.BranchID equals b.BranchID
        //                                                         where a.TransactionDate.Year == dt.Year && a.TransactionDate.Month == dt.Month
        //                                                         select new GetMontlySaleSummaryBarchartResponseDTO
        //                                                         {
        //                                                             branchid = a.BranchID,
        //                                                             branchname = b.BranchName,
        //                                                             totalamount = a.TotalAmount
        //                                                         }).ToList();
        List<GetMontlySaleSummaryBarchartResponseDTO> resData = (from branch in await _unitOfWork.Repository<TMBranch>().QueryAsync(w => w.IsActive)
                                                                 join a in await _unitOfWork.Repository<TTTransaction>().QueryAsync(w => w.IsActive & 
                                                                 (w.TransactionDate.Year == dt.Year && w.TransactionDate.Month == dt.Month)) on branch.BranchID equals a.BranchID
                                                                 into jTran 
                                                                 from tran in jTran.DefaultIfEmpty()
                                                                 //where a.TransactionDate.Year == dt.Year && a.TransactionDate.Month == dt.Month
                                                                 select new GetMontlySaleSummaryBarchartResponseDTO
                                                                 {
                                                                     branchid = branch.BranchID,
                                                                     branchname = branch.BranchName,
                                                                     totalamount = tran != null ? tran.TotalAmount : 0
                                                                 }).ToList();
        if (resData.Count == 0)
        {
            throw new Exception("ไม่พบข้อมูล");
        }
        resData = resData.GroupBy(g => g.branchid).Select(s => new GetMontlySaleSummaryBarchartResponseDTO
        {
            branchid = s.Key,
            branchname = s.First().branchname,
            totalamount = s.Sum(ss => ss.totalamount)
        }).OrderBy(w => w.branchid).ToList();
        return new BaseResponse<List<GetMontlySaleSummaryBarchartResponseDTO>>
        {
            result = true,
            data = resData,
            soruce = "db",
            message = "Success",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
