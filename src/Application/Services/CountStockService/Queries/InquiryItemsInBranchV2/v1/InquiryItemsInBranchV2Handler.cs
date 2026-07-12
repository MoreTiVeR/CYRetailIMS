using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.InquiryItemsInBranchV2.v1;

public class InquiryItemsInBranchV2Handler : BaseService,
    IRequestHandler<InquiryItemsInBranchV2Query, BaseResponse<List<InquiryItemsInBranchV2ResponseDTO>>>
{
    public InquiryItemsInBranchV2Handler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<InquiryItemsInBranchV2ResponseDTO>>> Handle(
        InquiryItemsInBranchV2Query request, CancellationToken cancellationToken)
    {
        var itemsInBranch = await _unitOfWork.Repository<TMItemInBranch>()
            .QueryAsync(w => w.IsActive == true && w.BranchID == request.branchid);

        var subItemTypes = await _unitOfWork.Repository<TMSubItemType>().QueryAsync();

        var result = (from a in itemsInBranch
                      join s in subItemTypes on a.Item.SubItemTypeID equals s.SubItemTypeID
                      into jSubItem
                      from sub in jSubItem.DefaultIfEmpty()
                      select new InquiryItemsInBranchV2ResponseDTO
                      {
                          branchid      = a.BranchID,
                          itemid        = a.ItemID,
                          itemcode      = a.Item.ItemCode,
                          itemname      = a.Item.Name,
                          subitemtypeid = sub != null ? sub.SubItemTypeID : (int?)null,
                          subitemcode   = sub != null ? sub.SubItemCode : null,
                          qtyinbranch   = a.Qty,
                      }).ToList();

        if (!result.Any())
        {
            throw new Exception("ไม่พบข้อมูลสินค้าสาขาที่ต้องการนับสต๊อก");
        }

        return new BaseResponse<List<InquiryItemsInBranchV2ResponseDTO>>
        {
            result  = true,
            data    = result,
            message = "Success",
            soruce  = "db",
            status  = StatusCodes.Status200OK.ToString()
        };
    }
}
