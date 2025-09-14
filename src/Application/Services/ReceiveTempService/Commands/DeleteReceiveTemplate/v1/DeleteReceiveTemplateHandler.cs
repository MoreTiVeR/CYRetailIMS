using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMReceiveTemplates;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ReceiveTempService.Commands.DeleteReceiveTemplate.v1;
public class DeleteReceiveTemplateHandler : BaseService, IRequestHandler<DeleteReceiveTemplateCommand, BaseResponse<CommandResponse>>
{
    public DeleteReceiveTemplateHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(DeleteReceiveTemplateCommand request, CancellationToken cancellationToken)
    {
        TMReceiveTemplate resEnt = await _unitOfWork.Repository<TMReceiveTemplate>().FirstOrDefaultAsync(w => w.ReceiveTempID == request.receivetemplateid);
        if (resEnt == null)
        {
            throw new Exception("ไม่พบข้อมูล");
        }
        resEnt.SetUpdatedBy(request.updatedby);
        resEnt.SetUpdatedDate();
        resEnt.DeActiveStatus();
        resEnt.AddDomainEvent(new TMReceiveTemplateDeleteEvent(resEnt));
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse
            {
                result = true,
            },
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
