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
        IEnumerable<GetItemByIDResponseDTO> resData = (from a in await _unitOfWork.Repository<TMItem>().QueryAsync(w => w.ItemID == request.itemid && w.IsActive)
                                                       join b in await _unitOfWork.Repository<TMItemBrand>().QueryAsync(w => w.IsActive) on a.BrandID equals b.BrandID
                                                       join c in await _unitOfWork.Repository<TMItemType>().QueryAsync(w => w.IsActive) on a.ItemTypeID equals c.ItemTypeID
                                                       join d in await _unitOfWork.Repository<TMUnitOfMeasure>().QueryAsync(w => w.IsActive) on a.UnitOfMeasureID equals d.UnitOfMeasureID
                                                       select new GetItemByIDResponseDTO
                                                       {
                                                           itemid = a.ItemID,
                                                           itemcode = a.ItemCode,
                                                           name = a.Name,
                                                           shortname = a.ShortName,
                                                           brandid = a.BrandID,
                                                           brandname = b.BrandName,
                                                           itemtypeid = a.ItemTypeID,
                                                           itemtypename = c.ItemTypeName,
                                                           unitofmeasureid = a.UnitOfMeasureID,
                                                           unitofmeasurename = d.UnitOfMeasureName,
                                                           barcode = a.BarCode,
                                                           description = a.Description,
                                                           itemimageurl = a.ItemImageUrl,
                                                           price = a.Price
                                                       }).AsEnumerable();
        if(!resData.Any())
        {
            throw new Exception("Data not found");
        }
        return new BaseResponse<GetItemByIDResponseDTO>
        {
            result = true,
            data = resData.FirstOrDefault(),
            message = " Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
