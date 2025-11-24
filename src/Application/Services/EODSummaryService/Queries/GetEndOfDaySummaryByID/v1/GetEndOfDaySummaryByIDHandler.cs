using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.EODSummaryService.Queries.GetEndOfDaySummaryByCriteria.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.EODSummaryService.Queries.GetEndOfDaySummaryByID.v1;
public class GetEndOfDaySummaryByIDHandler : BaseService, IRequestHandler<GetEndOfDaySummaryByIDQuery, BaseResponse<GetEndOfDaySummaryByCriteriaDetail>>
{
    public GetEndOfDaySummaryByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetEndOfDaySummaryByCriteriaDetail>> Handle(GetEndOfDaySummaryByIDQuery request, CancellationToken cancellationToken)
    {
        IQueryable<GetEndOfDaySummaryByCriteriaDetail> resData = (from a in await _unitOfWork.Repository<TTEndOfDaySummary>().QueryAsync()
                                                                  join b in await _unitOfWork.Repository<TMBranch>().QueryAsync() on a.BranchID equals b.BranchID into join_branch
                                                                  from b in join_branch.DefaultIfEmpty()
                                                                  where a.EndOfDayId == request.eodid
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

        if(resData == null || !resData.Any())
        {
            throw new Exception("ไม่พบข้อมูลสรุปยอดสิ้นวัน");
        }

        return new BaseResponse<GetEndOfDaySummaryByCriteriaDetail>
        {
            result = true,
            data = resData.FirstOrDefault(),
            status = StatusCodes.Status200OK.ToString(),
            message = "Success",
            soruce = "db"
        };
    }
}
