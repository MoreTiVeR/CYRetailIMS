using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReport.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace CYRetailIMS.Application.Services.ReportService.Queries.TransactionDeletionLogReport.v1;
public class TransactionDeletionLogReportHandler : BaseService, IRequestHandler<TransactionDeletionLogReportQuery, BaseResponse<TransactionDeletionLogReportResponseDTO>>
{
    public TransactionDeletionLogReportHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<TransactionDeletionLogReportResponseDTO>> Handle(TransactionDeletionLogReportQuery request, CancellationToken cancellationToken)
    {
        int totalRowCount = 0;
        var resDelTransactionLog = (from a in await _unitOfWork.Repository<TTTransactionDeletionLog>().QueryAsync(w => (w.CreatedDate.Date >= request.transaction_startdate.Date && w.CreatedDate.Date <= request.transaction_enddate.Date))
                                    join t in await _unitOfWork.Repository<TTTransaction>().QueryAsync() on a.TransactionID equals t.TransactionID
                                    join b in await _unitOfWork.Repository<TMBranch>().QueryAsync() on a.BranchID equals b.BranchID
                                    join c in await _unitOfWork.Repository<TMTransactionType>().QueryAsync() on t.TransactionTypeID equals c.TransactionTypeID
                                    select new TransactionDeletionLogReportDetailDTO
                                    {
                                        deltransactionlogid = a.DelTransactionLogID,
                                        transactionid = a.TransactionID,
                                        branchid = a.BranchID,
                                        branchname = b.BranchName,
                                        transactiontypeid = t.TransactionTypeID,
                                        transactiontypename = c.TransactionTypeName,
                                        transactiontypedesc = c.Description,
                                        totalamount = t.TotalAmount,
                                        reason = a.Reason,
                                        createdby = a.CreatedBy,
                                        createddate = a.CreatedDate,
                                        updatedby = a.UpdatedBy,
                                        updateddate = a.UpdatedDate
                                        
                                    }).AsEnumerable();
        if (!resDelTransactionLog.Any())
        {
            throw new Exception("ไม่พบข้อมูลรายการขายที่ถูกยกเลิก");
        }

        #region Filter
        if (request.branchid.HasValue)
        {
            resDelTransactionLog = resDelTransactionLog.Where(w => w.branchid == request.branchid.Value).ToList();
        }

        if (!string.IsNullOrEmpty(request.searchvalue))
        {
            resDelTransactionLog = resDelTransactionLog.Where(w => (w.branchname != null && w.branchname.Contains(request.searchvalue))
            || (!string.IsNullOrEmpty(w.reason) && w.reason.Contains(request.searchvalue))).ToList();
        }

        if (!resDelTransactionLog.Any())
        {
            throw new Exception("ไม่พบข้อมูลรายการขายที่ถูกยกเลิก");
        }
        #endregion

        #region Filter
        List<TransactionDeletionLogReportDetailDTO> resData = new List<TransactionDeletionLogReportDetailDTO>();
        totalRowCount = resDelTransactionLog.Count();
        if (request.isexportalldata)
        {
            resData = resDelTransactionLog.ToList();
        }
        else
        {
            resData = resDelTransactionLog.Skip(request.startrow).Take(request.pagesize).ToList();
        }
        if (!resData.Any())
        {
            throw new Exception("ไม่พบข้อมูลรายงานขายสินค้า");
        }
        #endregion

        #region Update updatedby data from emp name
        List<string> userNameList = resData.ToList().Select(s => s.createdby).Distinct().ToList();
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

        return new BaseResponse<TransactionDeletionLogReportResponseDTO>
        {
            result = true,
            data = new TransactionDeletionLogReportResponseDTO
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
