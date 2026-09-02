using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockApprovalReport.v1;

public class GetCountStockApprovalReportHandler : BaseService,
    IRequestHandler<GetCountStockApprovalReportQuery, BaseResponse<GetCountStockApprovalReportResponseDTO>>
{
    public GetCountStockApprovalReportHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetCountStockApprovalReportResponseDTO>> Handle(
        GetCountStockApprovalReportQuery request,
        CancellationToken cancellationToken)
    {
        var historyQuery = await _unitOfWork.Repository<TTCountStockApprovalHistory>()
            .QueryAsync(w => w.IsActive);

        if (request.branchid.HasValue && request.branchid.Value > 0)
        {
            historyQuery = historyQuery.Where(w => w.BranchID == request.branchid.Value);
        }

        if (request.startdate.HasValue)
        {
            DateTime start = request.startdate.Value.Date;
            historyQuery = historyQuery.Where(w => w.ApprovedDate >= start);
        }

        if (request.enddate.HasValue)
        {
            DateTime endExclusive = request.enddate.Value.Date.AddDays(1);
            historyQuery = historyQuery.Where(w => w.ApprovedDate < endExclusive);
        }

        var branchQuery = await _unitOfWork.Repository<TMBranch>().QueryAsync(w => w.IsActive);

        var groupedQuery =
            from g in
                (from h in historyQuery
                 group h by new
                 {
                     h.CountStockID,
                     h.CountStockDate,
                     h.BranchID,
                     h.CounterRole,
                     h.ApprovedBy,
                     h.ApprovedDate
                 }
                    into grouped
                 select new
                 {
                     grouped.Key.CountStockID,
                     grouped.Key.CountStockDate,
                     grouped.Key.BranchID,
                     grouped.Key.CounterRole,
                     grouped.Key.ApprovedBy,
                     grouped.Key.ApprovedDate,
                     TotalItems = grouped.Select(s => s.ItemID).Distinct().Count(),
                     TotalQtyBefore = grouped.Sum(s => s.QtyInBranchBeforeApprove),
                     TotalQtyAfter = grouped.Sum(s => s.QtyInBranchAfterApprove),
                     TotalAdjustedQty = grouped.Sum(s => s.QtyInBranchAfterApprove - s.QtyInBranchBeforeApprove)
                 })
            select new GetCountStockApprovalReportItemDTO
            {
                countstockid = g.CountStockID,
                countstockdate = g.CountStockDate,
                branchid = g.BranchID,
                branchname = branchQuery
                    .Where(b => b.BranchID == g.BranchID)
                    .Select(b => b.BranchName)
                    .FirstOrDefault() ?? string.Empty,
                counterrole = g.CounterRole,
                approvedby = g.ApprovedBy,
                approveddate = g.ApprovedDate,
                totalitems = g.TotalItems,
                totalqtybefore = g.TotalQtyBefore,
                totalqtyafter = g.TotalQtyAfter,
                totaladjustedqty = g.TotalAdjustedQty
            };

        if (!string.IsNullOrWhiteSpace(request.searchvalue))
        {
            string sv = request.searchvalue.Trim();
            groupedQuery = groupedQuery.Where(w =>
                w.branchname.Contains(sv)
                || (w.counterrole ?? string.Empty).Contains(sv)
                || (w.approvedby ?? string.Empty).Contains(sv)
                || w.countstockid.ToString().Contains(sv));
        }

        groupedQuery = groupedQuery
            .OrderByDescending(o => o.approveddate)
            .ThenByDescending(o => o.countstockid);

        int totalRows = await groupedQuery.CountAsync(cancellationToken);

        List<GetCountStockApprovalReportItemDTO> data;
        if (request.isexportalldata)
        {
            data = await groupedQuery.ToListAsync(cancellationToken);
        }
        else
        {
            int pageSize = request.pagesize <= 0 ? 20 : request.pagesize;
            int startRow = request.startrow < 0 ? 0 : request.startrow;

            // Some SQL Server compatibility levels do not support OFFSET/FETCH.
            // Use TOP on SQL side, then apply Skip in memory.
            int fetchCount = startRow + pageSize;
            var buffered = await groupedQuery.Take(fetchCount).ToListAsync(cancellationToken);
            data = buffered.Skip(startRow).Take(pageSize).ToList();
        }

        var response = new GetCountStockApprovalReportResponseDTO
        {
            totalrow = totalRows,
            transactiondata = data
        };

        return new BaseResponse<GetCountStockApprovalReportResponseDTO>
        {
            result = true,
            data = response,
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
