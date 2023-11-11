using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemByID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ItemService.Queries.GetItemByBarcode.v1;
public class GetItemByBarcodeHandler : BaseService, IRequestHandler<GetItemByBarcodeQuery, BaseResponse<GetItemByIDResponseDTO>>
{
    public GetItemByBarcodeHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetItemByIDResponseDTO>> Handle(GetItemByBarcodeQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<GetItemByIDResponseDTO> resData = (from a in await _unitOfWork.Repository<TMItem>().QueryAsync(w => w.IsActive && !string.IsNullOrEmpty(w.BarCode))
                                                       join b in await _unitOfWork.Repository<TMItemType>().QueryAsync(w => w.IsActive) on a.ItemTypeID equals b.ItemTypeID
                                                       join c in await _unitOfWork.Repository<TMItemBrand>().QueryAsync(w => w.IsActive) on a.BrandID equals c.BrandID
                                                       join d in await _unitOfWork.Repository<TMUnitOfMeasure>().QueryAsync(w => w.IsActive) on a.UnitOfMeasureID equals d.UnitOfMeasureID
                                                       join emp in await _unitOfWork.Repository<TMEmployee>().FindWithInclude(w => w.IsActive, i => i.Include(ic => ic.User))
                                                           on a.CreatedBy equals emp.User.UserName into tUser
                                                       from jUser in tUser.DefaultIfEmpty()
                                                       where a.BarCode.Trim().ToUpper() == request.itembarcode.Trim().ToUpper() && a.IsActive
                                                       select new GetItemByIDResponseDTO
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
                                                           notifyminqty = a.NotifyMinQty,
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
