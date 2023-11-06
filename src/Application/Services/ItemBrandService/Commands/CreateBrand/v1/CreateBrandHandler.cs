using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMBranchs;
using CYRetailIMS.Domain.Events.TMItemBrands;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemBrandService.Commands.CreateBrand.v1;
public class CreateBrandHandler : BaseService, IRequestHandler<CreateBrandCommand, BaseResponse<CommandResponse>>
{
    public CreateBrandHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        TMItemBrand isExist = await _unitOfWork.Repository<TMItemBrand>().FindAsync(w => w.BrandName.Equals(request.brandname)
        || w.BrandShortName.Equals(request.brandshortname)
        || w.BrandName.Contains(request.brandname)
        || w.BrandShortName.Contains(request.brandshortname));
        if(isExist != null) 
        {
            throw new Exception("มีข้อมูลแบรนด์สินค้านี้ในระบบแล้ว");
        }

        TMItemBrand mItemBrand = PrepareItemBrand(request);
        mItemBrand.AddDomainEvent(new TMItemBrandCreateEvent(mItemBrand));
        await _unitOfWork.Repository<TMItemBrand>().AddAsync(mItemBrand);
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse { result = true },
            status = StatusCodes.Status200OK.ToString(),
            message = "Success",
            soruce = "db"
        };
    }

    private TMItemBrand PrepareItemBrand(CreateBrandCommand reqObj)
    {
        return new TMItemBrand
        {
            BrandName = reqObj.brandname,
            BrandShortName = reqObj.brandshortname,
            Description = reqObj.description,
            CreatedBy = reqObj.createdby,
            CreatedDate = reqObj.createddate,
            IsActive = reqObj.isactive
        };
    }
}
