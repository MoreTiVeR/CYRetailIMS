using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ChartService.Queries.GetMontlySaleSummaryBarchart.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ChartService.Queries.GetMontlySaleSummaryByYear.v1;
public class GetMontlySaleSummaryByYearHandler : BaseService, IRequestHandler<GetMontlySaleSummaryByYearQuery, BaseResponse<List<GetMontlySaleSummaryByYearResponseDTO>>>
{
    public GetMontlySaleSummaryByYearHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetMontlySaleSummaryByYearResponseDTO>>> Handle(GetMontlySaleSummaryByYearQuery request, CancellationToken cancellationToken)
    {
        var resData = (from a in await _unitOfWork.Repository<TTTransaction>().QueryAsync(w => w.IsActive)
                       where a.TransactionDate.Year == request.year
                       select new GetMontlySaleSummaryByYearResponseDTO
                       {
                           month = a.TransactionDate.Month,
                           //monthname = new DateTime(DateTime.Now.Year, a.TransactionDate.Month, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("th")),
                           totalamount = a.TotalAmount
                       }).ToList();

        List<GetMontlySaleSummaryByYearResponseDTO> finalData = new List<GetMontlySaleSummaryByYearResponseDTO>();
        for (int i = 1; i <= 12; i++)
        {
            string monthName = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("th"));
            var dataOfMonth = resData.Where(w => w.month == i);
            if (dataOfMonth.Any())
            {
                GetMontlySaleSummaryByYearResponseDTO data = new GetMontlySaleSummaryByYearResponseDTO
                {
                    month = i,
                    monthname = monthName,
                    totalamount = dataOfMonth.Sum(w => w.totalamount)
                };
                finalData.Add(data);
            }
            else
            {
                finalData.Add(new GetMontlySaleSummaryByYearResponseDTO { month = i, monthname = monthName, totalamount = 0 });
            }
        }

        if(finalData.Count != 12)
        {
            throw new Exception("ไม่สามารถสร้างข้อมูลได้ กรุณาลองใหม่อีกครั้ง!");
        }

        return new BaseResponse<List<GetMontlySaleSummaryByYearResponseDTO>>
        {
            result = true,
            data = finalData,
            soruce = "db",
            message = "Success",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
