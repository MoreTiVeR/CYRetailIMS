using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;

namespace CYRetailIMS.Application.Services.ReportService.Queries.AvailableStockReport.v1;
public class AvailableStockReportHandler : BaseService, IRequestHandler<AvailableStockReportQuery, BaseResponse<List<AvailableStockReportResponseDTO>>>
{
    public AvailableStockReportHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public Task<BaseResponse<List<AvailableStockReportResponseDTO>>> Handle(AvailableStockReportQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
