using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMSupplierContacts;
using CYRetailIMS.Domain.Events.TMSuppliers;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.SupplierService.Commands.CreateSupplier.v1;
public class CreateSupplierHandler : BaseService, IRequestHandler<CreateSupplierCommand, BaseResponse<CommandResponse>>
{
    public CreateSupplierHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        //Check TMSupplier is exist
        IEnumerable<TMSupplier> resSupplier = await _unitOfWork.Repository<TMSupplier>().QueryAsync(w => w.SupplierName_TH == request.suppliernameth.Trim()
        || w.SupplierName_EN == request.suppliernameen.Trim());
        if (resSupplier.Any())
        {
            throw new Exception("มีข้อมูลซัฟพลายเออร์นี้ในระบบแล้ว");
        }

        //TMSupplier
        TMSupplier supplierEnt = _mapper.Map<TMSupplier>(request);

        //Disable
        //supplierEnt.TMSupplierDetails.ToList().ForEach(e =>
        //{
        //    e.SetCreatedBy(request.createdby);
        //    e.SetCreatedDate(request.createddate);
        //    e.ActiveStatus();
        //});

        supplierEnt.SupplierName_TH = supplierEnt.SupplierName_TH.Trim();
        supplierEnt.SupplierName_EN = supplierEnt.SupplierName_EN.Trim();
        supplierEnt.TMSupplierContacts.ToList().ForEach(e =>
        {
            e.SetCreatedBy(request.createdby);
            e.SetCreatedDate(request.createddate);
            e.ActiveStatus();
            e.AddDomainEvent(new TMSupplierContactCreateEvent(e));
        });

        supplierEnt.SetCreatedBy(request.createdby);
        supplierEnt.SetCreatedDate(request.createddate);
        supplierEnt.ActiveStatus();
        supplierEnt.AddDomainEvent(new TMSupplierCreateEvent(supplierEnt));

        await _unitOfWork.Repository<TMSupplier>().AddAsync(supplierEnt);
        await _unitOfWork.SaveChangesAsync();

        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse { result = true },
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };

    }
}
