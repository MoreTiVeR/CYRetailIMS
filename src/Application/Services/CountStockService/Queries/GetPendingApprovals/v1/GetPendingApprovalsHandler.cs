using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.GetPendingApprovals.v1;

/// <summary>
/// Handler: ดึงรายการนับสต๊อกที่รออนุมัติ
/// </summary>
public class GetPendingApprovalsHandler : BaseService, IRequestHandler<GetPendingApprovalsQuery, BaseResponse<List<GetPendingApprovalsResponseDTO>>>
{
    public GetPendingApprovalsHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetPendingApprovalsResponseDTO>>> Handle(GetPendingApprovalsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<TTCountStock> countStockQuery = await _unitOfWork.Repository<TTCountStock>()
            .FindWithInclude(
                w => w.IsActive,
                i => i.Include(s => s.TTCountStockDetails));

        // When statuscid is provided, filter by that status
        // Default: show all active records (0=draft, 1=submitted, 2=approved) so data appears before migration
        if (request.statuscid.HasValue)
        {
            countStockQuery = countStockQuery.Where(w => w.CountStockStatusID == request.statuscid.Value);
        }

        if (!string.IsNullOrEmpty(request.counterrole))
        {
            if (request.counterrole == "HeadPC")
            {
                countStockQuery = countStockQuery.Where(w =>
                    w.CounterRole == "HeadPC"
                    || w.CounterRole == "PC Supervisor"
                    || w.CounterRole == "SaleArea");
            }
            else if (request.counterrole == "PC")
            {
                countStockQuery = countStockQuery.Where(w =>
                    w.CounterRole == "PC"
                    || w.CounterRole == null);
            }
            else
            {
                countStockQuery = countStockQuery.Where(w => w.CounterRole == request.counterrole);
            }
        }

        if (request.isnewentryonly.HasValue)
        {
            if (request.isnewentryonly.Value)
            {
                countStockQuery = countStockQuery.Where(w =>
                    w.TTCountStockDetails.Any(d => d.ItemID.HasValue && d.ItemID.Value > 0));
            }
            else
            {
                countStockQuery = countStockQuery.Where(w =>
                    !w.TTCountStockDetails.Any(d => d.ItemID.HasValue && d.ItemID.Value > 0));
            }
        }

        IQueryable<TMBranch> branchQuery = await _unitOfWork.Repository<TMBranch>().QueryAsync();

        var result = (from cs in countStockQuery
                      join b in branchQuery on cs.BranchID equals b.BranchID into branchJoin
                      from branch in branchJoin.DefaultIfEmpty()
                      orderby cs.CountDate descending
                      select new GetPendingApprovalsResponseDTO
                      {
                          countstockid = cs.CountStockID,
                          countstockdate = cs.CountDate,
                          branchid = cs.BranchID,
                          branchname = branch != null ? branch.BranchName : string.Empty,
                          counterrole = cs.CounterRole == "HeadPC"
                              || cs.CounterRole == "PC Supervisor"
                              || cs.CounterRole == "SaleArea"
                              ? "HeadPC"
                              : "PC",
                          isnewentry = cs.TTCountStockDetails.Any(d => d.ItemID.HasValue && d.ItemID.Value > 0),
                          createdby = cs.CreatedBy,
                          counterstockstatusid = cs.CountStockStatusID,
                          approvedby = cs.ApprovedBy,
                          approveddate = cs.ApprovedDate
                      }).ToList();

        return new BaseResponse<List<GetPendingApprovalsResponseDTO>>
        {
            result = true,
            data = result,
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
