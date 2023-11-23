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

namespace CYRetailIMS.Application.Services.ChartService.Queries.GetSellingTransactionByMonth.v1;
public class GetSellingTransactionByMonthHandler : BaseService, IRequestHandler<GetTransactionByMonthQuery, BaseResponse<List<GetSellingTransactionByMonthResponseDTO>>>
{
    public GetSellingTransactionByMonthHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    /// <summary>
    /// Share of Market (SOM) เป็นการคิดเรื่องว่าเรามีส่วนแบ่งแค่ไหนในการตลาด โดยคำนวนมาจาก SOM = ยอดขายบริษัท หารด้วย ยอดขายทั้งตลาด คูณด้วย 100% 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<BaseResponse<List<GetSellingTransactionByMonthResponseDTO>>> Handle(GetTransactionByMonthQuery request, CancellationToken cancellationToken)
    {
        var resData = (from a in await _unitOfWork.Repository<TTTransaction>().QueryAsync(w => w.TransactionDate.Month == request.month 
                       && w.TransactionDate.Year == request.year 
                       && w.IsActive)
                       join b in await _unitOfWork.Repository<TTTransactonDetail>().QueryAsync(w => w.IsActive.HasValue) on a.TransactionID equals b.TransactionID
                       join c in await _unitOfWork.Repository<TMItem>().QueryAsync(w => w.IsActive) on b.ItemID equals c.ItemID
                       select new
                       {
                           txnmonth = a.TransactionDate.Month,
                           itemid = b.ItemID,
                           itemname = c.Name,
                           qty = b.Qty
                       }).ToList();
        if (resData.Count == 0)
        {
            throw new Exception("ไม่พบข้อมูล");
        }

        var resGroup = resData.GroupBy(g => g.itemid).Select(s => new
        {
            month = s.FirstOrDefault().txnmonth,
            itemid = s.Key,
            itemname = s.FirstOrDefault().itemname,
            totalqty = s.Sum(w => w.qty),
        }).ToList();

        int sumQty = resGroup.Sum(s => s.totalqty);
        int maxSellingItemID = resGroup.OrderByDescending(s => s.totalqty).FirstOrDefault().itemid;
        List<GetSellingTransactionByMonthResponseDTO> result = resGroup.Select(s => new GetSellingTransactionByMonthResponseDTO
        {
            itemname = s.itemname,
            percent = (double)decimal.Multiply(decimal.Divide(s.totalqty, sumQty), 100),
            isselected = s.itemid == maxSellingItemID ? true : false,
            issliced = s.itemid == maxSellingItemID ? true : false,
        }).ToList();

        return new BaseResponse<List<GetSellingTransactionByMonthResponseDTO>>
        {
            result = true,
            data = result,
            soruce = "db",
            message = "Success",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
