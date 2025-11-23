using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.EODSummaryService.Queries.GetEndOfDaySummaryList.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.ItemStockReport.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.EODSummaryService.Queries.GetEndOfDaySummaryByCriteria.v1;
public class GetEndOfDaySummaryByCriteriaHandler : BaseService, IRequestHandler<GetEndOfDaySummaryByCriteriaQuery, BaseResponse<GetEndOfDaySummaryByCriteriaResponseDTO>>
{
    public GetEndOfDaySummaryByCriteriaHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetEndOfDaySummaryByCriteriaResponseDTO>> Handle(GetEndOfDaySummaryByCriteriaQuery request, CancellationToken cancellationToken)
    {
        int totalRow = 0;
        var resData = (from a in await _unitOfWork.Repository<TTEndOfDaySummary>().QueryAsync()
                       join b in await _unitOfWork.Repository<TMBranch>().QueryAsync() on a.BranchID equals b.BranchID into join_branch
                       from b in join_branch.DefaultIfEmpty()
                       where (a.SummaryDate >= request.transaction_startdate.Date && a.SummaryDate <= request.transaction_enddate.Date)
                       select new GetEndOfDaySummaryByCriteriaDetail
                       {
                           endofdayid = a.EndOfDayId,
                           summarydate = a.SummaryDate,
                           branchid = a.BranchID,
                           branchname = b != null ? b.BranchName : string.Empty,
                           totalcash = a.TotalCash,
                           depositedcash = a.DepositedCash,
                           totaltransfer = a.TotalTransfer,
                           customertransfer = a.CustomerTransfer,
                           grandtotal = a.GrandTotal,
                           substitutewage = a.SubstituteWage,
                           fee = a.Fee,
                           otherexpense = a.OtherExpense,
                           otherexpensenote = a.OtherExpenseNote,
                           finaltotal = a.FinalTotal,
                           isactive = a.IsActive,
                           createdby = a.CreatedBy,
                           createddate = a.CreatedDate,
                           updatedby = a.UpdatedBy,
                           updateddate = a.UpdatedDate
                       }).AsQueryable();

        if (request.branchid.HasValue)
        {
            resData = resData.Where(w => w.branchid == request.branchid);
        }

        if (!resData.Any())
        {
            throw new Exception("ไม่พบข้อมูลสรุปยอดสิ้นวัน");
        }

        //Final data
        List<GetEndOfDaySummaryByCriteriaDetail> resEodSummaryData = await resData.ToListAsync();

        //Addign total row
        totalRow = resEodSummaryData.Count();

        //Assign data
        if (!request.isexportalldata)
        {
            resEodSummaryData = resEodSummaryData.Skip(request.startrow).Take(request.pagesize).ToList();
        }

        return new BaseResponse<GetEndOfDaySummaryByCriteriaResponseDTO>
        {
            result = true,
            status = "200",
            message = "Success",
            soruce = "db",
            data = new GetEndOfDaySummaryByCriteriaResponseDTO
            {
                totalrow = totalRow,
                transactiondata = resEodSummaryData
            }
        };
    }
}
