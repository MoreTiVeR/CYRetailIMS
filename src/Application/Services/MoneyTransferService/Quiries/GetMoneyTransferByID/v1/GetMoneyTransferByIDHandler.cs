using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.MoneyTransferService.Quiries.GetMoneyTransferByCriteria.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.MoneyTransferService.Quiries.GetMoneyTransferByID.v1;
public class GetMoneyTransferByIDHandler : BaseService, IRequestHandler<GetMoneyTransferByIDQuery, BaseResponse<GetMoneyTransferByCriteriaResponseDTO>>
{
    public GetMoneyTransferByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetMoneyTransferByCriteriaResponseDTO>> Handle(GetMoneyTransferByIDQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TTMoneyTransfer> resMoneyTransfer = await _unitOfWork.Repository<TTMoneyTransfer>().FindWithInclude(w => w.MoneyTransferID >= request.moneytransferid,
                    i => i.Include(s => s.Branch));
        if (!resMoneyTransfer.Any())
        {
            throw new Exception("ไม่พบข้อมูล");
        }
        List<GetMoneyTransferByCriteriaResponseDTO> resData = resMoneyTransfer.Select(s => new GetMoneyTransferByCriteriaResponseDTO
        {
            moneytransferid = s.MoneyTransferID,
            branchid = s.BranchID,
            branchname = s.Branch.BranchName,
            description = s.Description,
            amounttransfer = s.AmountTransfer,
            transferdate = s.TransferDate,
            createdby = s.CreatedBy,
            createddate = s.CreatedDate,
            isactive = s.IsActive
        }).ToList();
        return new BaseResponse<GetMoneyTransferByCriteriaResponseDTO>
        {
            result = true,
            data = resData.FirstOrDefault(),
            message = "สำเร็จ",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
