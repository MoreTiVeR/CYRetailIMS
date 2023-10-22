using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMSuppliers;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.SupplierService.Commands.DeleteSupplier.v1;
public class DeleteSupplierHandler : BaseService, IRequestHandler<DeleteSupplierCommand, BaseResponse<CommandResponse>>
{
    public DeleteSupplierHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        TMSupplier resSupplier = await _unitOfWork.Repository<TMSupplier>().FirstOrDefaultAsync(w => w.SupplierID == request.supplierid);
        if(resSupplier == null)
        {
            throw new Exception("ไม่พบข้อมูลซัฟพลายเออร์");
        }

        resSupplier.DeActiveStatus();
        resSupplier.SetUpdatedBy(request.deleteddby);
        resSupplier.SetUpdatedDate(request.deleteddate);
        resSupplier.AddDomainEvent(new TMSupplierDeleteEvent(resSupplier));

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
