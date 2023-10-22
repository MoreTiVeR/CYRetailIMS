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
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.SupplierService.Commands.UpdateSupplier.v1;
public class UpdateSupplierHandler : BaseService, IRequestHandler<UpdateSupplierCommand, BaseResponse<CommandResponse>>
{
    public UpdateSupplierHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        //Get Supplier
        IEnumerable<TMSupplier> resSupplier = await _unitOfWork.Repository<TMSupplier>().FindWithInclude(w => w.SupplierID == request.supplierid, 
            i => i.Include(w => w.TMSupplierContacts));
        if (!resSupplier.Any())
        {
            throw new Exception("ไม่พบข้อมูลซัฟพลายเออร์");
        }

        //Check dupplicate
        TMSupplier isExistSupplier = await _unitOfWork.Repository<TMSupplier>().FirstOrDefaultAsync(w => w.SupplierName_TH == request.suppliernameth
        || w.SupplierName_EN == request.suppliernameen);
        if(isExistSupplier != null)
        {
            throw new Exception("มีข้อมูลซัฟพลายเออร์นี้ในระบบแล้ว");
        }

        UpdateSupplierContact reqUpdateContact = request.contact.FirstOrDefault();
        resSupplier.ToList().ForEach(e =>
        {
            e.SupplierName_TH = request.suppliernameth;
            e.SupplierName_EN = request.suppliernameen;
            e.SupplierTypeID = request.suppliertypeid;
            e.SetUpdatedBy(request.updatedby);
            e.SetUpdatedDate(request.updateddate);
            if(reqUpdateContact != null)
            {
                e.TMSupplierContacts.ToList().ForEach(d =>
                {
                    d.SupplierContactTypeID = reqUpdateContact.suppliercontacttypeid;
                    d.ContactAccountName = reqUpdateContact.contactaccountname;
                    d.ContactPerson = reqUpdateContact.contactperson;
                    d.MobileNo = reqUpdateContact.mobileno;
                    d.Description = reqUpdateContact.desctiption;
                    d.SetUpdatedBy(request.updatedby);
                    d.SetUpdatedDate(request.updateddate);
                    d.AddDomainEvent(new TMSupplierContactUpdateEvent(d));
                });
            }
            e.AddDomainEvent(new TMSupplierUpdateEvent(e));
        });

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
