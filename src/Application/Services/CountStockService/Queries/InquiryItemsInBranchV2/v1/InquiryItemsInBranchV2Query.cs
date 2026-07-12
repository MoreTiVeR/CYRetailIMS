using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.InquiryItemsInBranchV2.v1;

public record InquiryItemsInBranchV2Query : IRequest<BaseResponse<List<InquiryItemsInBranchV2ResponseDTO>>>
{
    public int branchid { get; init; }
}
