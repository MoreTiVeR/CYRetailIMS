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

namespace CYRetailIMS.Application.Services.ItemService.Queries.GetItemByID.v1;
public class GetItemByIDHandler : BaseService, IRequestHandler<GetItemByIDQuery, BaseResponse<GetItemByIDResponseDTO>>
{
    public GetItemByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetItemByIDResponseDTO>> Handle(GetItemByIDQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<GetItemByIDResponseDTO> resData = (from a in await _unitOfWork.Repository<TMItem>().QueryAsync()
                                                       join b in await _unitOfWork.Repository<TMItemType>().QueryAsync() on a.ItemTypeID equals b.ItemTypeID
                                                       join c in await _unitOfWork.Repository<TMItemBrand>().QueryAsync() on a.BrandID equals c.BrandID
                                                       join d in await _unitOfWork.Repository<TMUnitOfMeasure>().QueryAsync() on a.UnitOfMeasureID equals d.UnitOfMeasureID
                                                       join emp in await _unitOfWork.Repository<TMEmployee>().FindWithInclude(w => w.IsActive, i => i.Include(ic => ic.User))
                                                           on a.CreatedBy equals emp.User.UserName into tUser
                                                       from jUser in tUser.DefaultIfEmpty()
                                                       join isub in await _unitOfWork.Repository<TMSubItemType>().QueryAsync(s => s.IsActive) on a.SubItemTypeID equals isub.SubItemTypeID into jsubitem
                                                       from subitem in jsubitem.DefaultIfEmpty()
                                                       where a.ItemID == request.itemid
                                                       select new GetItemByIDResponseDTO
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
                                                           createdby = jUser != null ? jUser.FirstName : "N/A",
                                                           createddate = a.CreatedDate,
                                                           cost = a.Cost,
                                                           discountpercent = a.DiscountPercent,
                                                           updatedby = a.UpdatedBy,
                                                           updateddate = a.UpdatedDate,
                                                           isactive = a.IsActive
                                                       }).AsEnumerable();

        if (!resData.Any())
        {
            throw new Exception("ไม่พบข้อมูลสินค้า");
        }

        #region Update updatedby data from emp name
        List<string> userNameList = resData.Select(s => s.updatedby).ToList();
        IEnumerable<TMUsers> userList = await _unitOfWork.Repository<TMUsers>().FindWithInclude(w => userNameList.Contains(w.UserName), i => i.Include(w => w.TMEmployees));
        var empDataList = userList.Select(s => new { s.UserName, s.TMEmployees.FirstOrDefault().FirstName }).ToList();
        List<GetItemByIDResponseDTO> resItems = resData.Select(s =>
        {
            if (!string.IsNullOrEmpty(s.updatedby))
            {
                s.updatedby = empDataList.FirstOrDefault(w => w.UserName == s.updatedby) != null 
                ? empDataList.FirstOrDefault(w => w.UserName == s.updatedby).FirstName : s.updatedby;
            }
            return s;
        }).ToList();
        #endregion

        return new BaseResponse<GetItemByIDResponseDTO>
        {
            result = true,
            data = resItems.FirstOrDefault(),
            message = " Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
