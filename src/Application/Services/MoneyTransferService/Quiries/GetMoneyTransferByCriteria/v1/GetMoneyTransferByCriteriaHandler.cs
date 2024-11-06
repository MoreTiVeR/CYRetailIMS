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

namespace CYRetailIMS.Application.Services.MoneyTransferService.Quiries.GetMoneyTransferByCriteria.v1;
public class GetMoneyTransferByCriteriaHandler : BaseService, IRequestHandler<GetMoneyTransferByCriteriaQuery, BaseResponse<List<GetMoneyTransferByCriteriaResponseDTO>>>
{
    public GetMoneyTransferByCriteriaHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetMoneyTransferByCriteriaResponseDTO>>> Handle(GetMoneyTransferByCriteriaQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TTMoneyTransfer> resMoneyTransfer = await _unitOfWork.Repository<TTMoneyTransfer>().FindWithInclude(w => w.TransferDate.Date >= request.startdate.Date,
            i => i.Include(s => s.Branch));
        if (!resMoneyTransfer.Any())
        {
            throw new Exception("ไม่พบข้อมูล");
        }

        #region Filter
        if (request.enddate.HasValue)
        {
            resMoneyTransfer = resMoneyTransfer.Where(w => w.TransferDate.Date <= request.enddate.Value.Date);
        }

        if (request.branchid.HasValue)
        {
            resMoneyTransfer = resMoneyTransfer.Where(w => w.BranchID == request.branchid.Value);
        }
        #endregion

        List<GetMoneyTransferByCriteriaResponseDTO> resData = resMoneyTransfer.Select(s => new GetMoneyTransferByCriteriaResponseDTO
        {
            moneytransferid = s.MoneyTransferID,
            branchid = s.BranchID,
            branchname = s.Branch.BranchName,
            description = s.Description,
            amounttransfer = s.AmountTransfer,
            transferdate = s.TransferDate,
            createdby = s.CreatedBy,
            createddate = s.CreatedDate
        }).ToList();
        return new BaseResponse<List<GetMoneyTransferByCriteriaResponseDTO>>
        {
            result = true,
            data = resData,
            message = "สำเร็จ",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
