using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;
public class GetItemListHandler : BaseService, IRequestHandler<GetItemListQuery, BaseResponse<List<GetItemListResponseDTO>>>
{
    public GetItemListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    /// <summary>
    /// Gel all inactive
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<BaseResponse<List<GetItemListResponseDTO>>> Handle(GetItemListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<GetItemListResponseDTO> resItems = (from a in await _unitOfWork.Repository<TMItem>().QueryAsync()
                                                        join b in await _unitOfWork.Repository<TMItemType>().QueryAsync(w => w.IsActive) on a.ItemTypeID equals b.ItemTypeID
                                                        join c in await _unitOfWork.Repository<TMItemBrand>().QueryAsync(w => w.IsActive) on a.BrandID equals c.BrandID
                                                        join d in await _unitOfWork.Repository<TMUnitOfMeasure>().QueryAsync(w => w.IsActive) on a.UnitOfMeasureID equals d.UnitOfMeasureID
                                                        join isub in await _unitOfWork.Repository<TMSubItemType>().QueryAsync(s => s.IsActive) on a.SubItemTypeID equals isub.SubItemTypeID into jsubitem
                                                        from subitem in jsubitem.DefaultIfEmpty()
                                                        //where a.IsActive
                                                        select new GetItemListResponseDTO
                                                        {
                                                            itemid = a.ItemID,
                                                            itemcode = a.ItemCode,
                                                            name = a.Name,
                                                            subitemtypeid = a.SubItemTypeID,
                                                            subitemtypename = subitem != null ? subitem.SubTypeNameTH : null,
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
                                                            notifyminqty = a.NotifyMinQty,
                                                            notifymaxqty = a.NotifyMaxQty,
                                                            createdby = a.CreatedBy,
                                                            createddate = a.CreatedDate,
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

        //Mapping DTO
        List<GetItemListResponseDTO> items = _mapper.Map<List<GetItemListResponseDTO>>(resItems);

        #region Update updatedby data from emp name
        List<string> userNameList = items.Select(s => s.createdby).Union(items.Select(s => s.updatedby)).Distinct().ToList();
        IEnumerable<TMUsers> userList = await _unitOfWork.Repository<TMUsers>().FindWithInclude(w => userNameList.Contains(w.UserName), i => i.Include(w => w.TMEmployees));
        var empDataList = userList.Select(s => new { s.UserName, s.TMEmployees.FirstOrDefault().FirstName }).ToList();
        items = items.Select(s =>
        {
            if (!string.IsNullOrEmpty(s.createdby))
            {
                s.createdby = empDataList.FirstOrDefault(w => w.UserName == s.createdby) != null
                ? empDataList.FirstOrDefault(w => w.UserName == s.createdby).FirstName : s.createdby;
            }

            if (!string.IsNullOrEmpty(s.updatedby))
            {
                s.updatedby = empDataList.FirstOrDefault(w => w.UserName == s.updatedby) != null
                ? empDataList.FirstOrDefault(w => w.UserName == s.updatedby).FirstName : s.updatedby;
            }
            return s;
        }).ToList();
        #endregion

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
