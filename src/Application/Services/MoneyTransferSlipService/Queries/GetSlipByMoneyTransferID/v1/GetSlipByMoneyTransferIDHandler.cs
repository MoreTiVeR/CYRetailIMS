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

namespace CYRetailIMS.Application.Services.MoneyTransferSlipService.Queries.GetSlipByMoneyTransferID.v1;
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
        if (res == null || !res.Any())
        {
            throw new Exception("ไม่พบข้อมูลสลิปโอนเงิน");
        }

        //Get first ent
        TTMoneyTransfer slipData = res.First();

        if (string.IsNullOrEmpty(slipData.SlipImagePath) && slipData.MoneyTransferSlipID is null)
        {
            throw new Exception("ไม่พบข้อมูลสลิปโอนเงิน");
        }

        GetSlipByMoneyTransferIDResponseDTO resData = new GetSlipByMoneyTransferIDResponseDTO
        {
            moneytransferid = slipData.MoneyTransferID,
            sliptransferid = slipData.MoneyTransferSlipID,
            totalamounttransfer = slipData.MoneyTransferSlip != null ? slipData.MoneyTransferSlip.TotalAmountTransfer : slipData.AmountTransfer,
            createdby = slipData.CreatedBy,
            createddate = slipData.CreatedDate
            //slipdetail = !string.IsNullOrEmpty(s.SlipImagePath) ? new List<GetSlipByMoneyTransferIDDetailResponseDTO>
            //{
            //    new GetSlipByMoneyTransferIDDetailResponseDTO
            //    {
            //        slipdetailid = 0,
            //        imgtitle = s.SlipImagePath.Split("/")?.LastOrDefault(),
            //        imgpath = s.SlipImagePath
            //    }
            //} : s.MoneyTransferSlip != null ? s.MoneyTransferSlip.TTMoneyTransferSlipsDetails.Select(d => new GetSlipByMoneyTransferIDDetailResponseDTO
            //{
            //    slipdetailid = d.MoneyTransferSlipDetailID,
            //    imgtitle = d.SlipImagePath.Split("/")?.LastOrDefault(),
            //    imgpath = d.SlipImagePath
            //}).OrderBy(o => o.slipdetailid).ToList() : null
        };

        //1. Add slip each transaction first
        if (!string.IsNullOrEmpty(slipData.SlipImagePath))
        {
            resData.slipdetail = new List<GetSlipByMoneyTransferIDDetailResponseDTO>
            {
                new GetSlipByMoneyTransferIDDetailResponseDTO
                {
                    slipdetailid = 0,
                    imgtitle = slipData.SlipImagePath.Split("/")?.LastOrDefault(),
                    imgpath = slipData.SlipImagePath
                }
            }.ToList();
        }

        //2. Add bundle slip
        if(slipData.MoneyTransferSlip != null)
        {
            if(resData.slipdetail == null)
            {
                resData.slipdetail = new List<GetSlipByMoneyTransferIDDetailResponseDTO>();
            }
            resData.slipdetail.AddRange(slipData.MoneyTransferSlip.TTMoneyTransferSlipsDetails.Select(d => new GetSlipByMoneyTransferIDDetailResponseDTO
            {
                slipdetailid = d.MoneyTransferSlipDetailID,
                imgtitle = d.SlipImagePath.Split("/")?.LastOrDefault(),
                imgpath = d.SlipImagePath
            }).OrderBy(o => o.slipdetailid));
        }

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
