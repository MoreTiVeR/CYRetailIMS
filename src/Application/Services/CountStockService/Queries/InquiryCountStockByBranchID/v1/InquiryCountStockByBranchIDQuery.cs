using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByBranchID.v1;
public record InquiryCountStockByBranchIDQuery : IRequest<BaseResponse<List<InquiryCountStockByBranchIDResponseDTO>>>
{
    public int branchid { get; init; }
}
