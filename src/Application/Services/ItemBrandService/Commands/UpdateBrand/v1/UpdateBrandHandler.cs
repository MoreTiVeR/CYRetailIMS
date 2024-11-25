using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMItemBrands;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemBrandService.Commands.UpdateBrand.v1;
public class UpdateBrandHandler : BaseService, IRequestHandler<UpdateBrandCommand, BaseResponse<CommandResponse>>
{
    public UpdateBrandHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        TMItemBrand resBrand = await _unitOfWork.Repository<TMItemBrand>().FirstOrDefaultAsync(w => w.BrandID == request.brandid);
        if(resBrand == null)
        {
            throw new Exception("Data not found");
        }
        resBrand.BrandName = request.brandname;
        resBrand.BrandShortName = request.brandshortname;
        resBrand.Description = request.desription;
        resBrand.IsActive = resBrand.IsActive;
        resBrand.SetUpdatedBy(request.updatedby);
        resBrand.SetUpdatedDate();
        resBrand.AddDomainEvent(new TMItemBrandUpdateEvent(resBrand));
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse {  result = true },
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
