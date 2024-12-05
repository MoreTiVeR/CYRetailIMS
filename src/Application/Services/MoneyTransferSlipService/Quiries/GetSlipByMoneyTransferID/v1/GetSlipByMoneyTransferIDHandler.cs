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

namespace CYRetailIMS.Application.Services.MoneyTransferSlipService.Quiries.GetSlipByMoneyTransferID.v1;
public class GetSlipByMoneyTransferIDHandler : BaseService, IRequestHandler<GetSlipByMoneyTransferIDQuery, BaseResponse<GetSlipByMoneyTransferIDResponseDTO>>
{
    public GetSlipByMoneyTransferIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetSlipByMoneyTransferIDResponseDTO>> Handle(GetSlipByMoneyTransferIDQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TTMoneyTransfer> res = await _unitOfWork.Repository<TTMoneyTransfer>().FindWithInclude(w => w.MoneyTransferID == request.moneytransferid,
            i => i.Include(s => s.MoneyTransferSlip),
            ii => ii.Include(s => s.MoneyTransferSlip.TTMoneyTransferSlipsDetails));
        if(res == null || !res.Any())
        {
            throw new Exception("ไม่พบข้อมูลสลิปโอนเงิน");
        }

        if(res.First().MoneyTransferSlipID == null || res.First().MoneyTransferSlipID == 0)
        {
            throw new Exception("ไม่พบข้อมูลสลิปโอนเงิน");
        }

        GetSlipByMoneyTransferIDResponseDTO resData = res.Select(s => new GetSlipByMoneyTransferIDResponseDTO
        {
            moneytransferid = s.MoneyTransferID,
            sliptransferid = s.MoneyTransferSlipID,
            totalamounttransfer = s.MoneyTransferSlip.TotalAmountTransfer,
            createdby = s.CreatedBy,
            createddate = s.CreatedDate,
            slipdetail = s.MoneyTransferSlip.TTMoneyTransferSlipsDetails.Select(d => new GetSlipByMoneyTransferIDDetailResponseDTO
            {
                slipdetailid = d.MoneyTransferSlipDetailID,
                imgtitle = d.SlipImagePath.Split("/")?.LastOrDefault(),
                imgpath = d.SlipImagePath
            }).OrderBy(o => o.slipdetailid).ToList()
        }).First();
        return new BaseResponse<GetSlipByMoneyTransferIDResponseDTO>
        {
            result = true,
            data = resData,
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
