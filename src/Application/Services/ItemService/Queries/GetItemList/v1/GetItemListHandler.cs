using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;
public class GetItemListHandler : BaseService, IRequestHandler<GetItemListQuery, BaseResponse<List<GetItemListResponseDTO>>>
{
    public GetItemListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetItemListResponseDTO>>> Handle(GetItemListQuery request, CancellationToken cancellationToken)
    {
        //IEnumerable<TMItem> resItems = await _unitOfWork.Repository<TMItem>().FindListAsync(w => w.IsActive);
        IEnumerable<GetItemListResponseDTO> resItems = (from a in await _unitOfWork.Repository<TMItem>().QueryAsync(w => w.IsActive)
                                                        join b in await _unitOfWork.Repository<TMItemType>().QueryAsync(w => w.IsActive) on a.ItemTypeID equals b.ItemTypeID
                                                        join c in await _unitOfWork.Repository<TMItemBrand>().QueryAsync(w => w.IsActive) on a.BrandID equals c.BrandID
                                                        join d in await _unitOfWork.Repository<TMUnitOfMeasure>().QueryAsync(w => w.IsActive) on a.UnitOfMeasureID equals d.UnitOfMeasureID
                                                        where a.IsActive
                                                        select new GetItemListResponseDTO
                                                        {
                                                            itemid = a.ItemID,
                                                            itemcode = a.ItemCode,
                                                            name = a.Name,
                                                            shortname = a.ShortName,
                                                            itemtypeid = a.ItemTypeID,
                                                            itemtypename = b.ItemTypeName,
                                                            brandid = a.BrandID,
                                                            brandname = c.BrandName,
                                                            unitofmeasureid = a.UnitOfMeasureID,
                                                            unitofmeasurename = d.UnitOfMeasureName,
                                                            barcode = a.BarCode,
                                                            description = a.Description,
                                                            itemimageurl = a.ItemImageUrl,
                                                            price = a.Price,
                                                            qty = a.Qty,
                                                            createdby = a.CreatedBy,
                                                            createddate = a.CreadedDate,
                                                            cost = a.Cost,
                                                            discountpercent = a.DiscountPercent,
                                                            updatedby = a.UpdatedBy,
                                                            updateddate = a.UpdatedDate,
                                                            isactive = a.IsActive
                                                        }).AsEnumerable();
        if (!resItems.Any())
        {
            throw new Exception("Data not found");
        }
        List<GetItemListResponseDTO> items = _mapper.Map<List<GetItemListResponseDTO>>(resItems);
        return new BaseResponse<List<GetItemListResponseDTO>>
        {
            result = true,
            data = items,
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
