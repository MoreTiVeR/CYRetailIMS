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

namespace CYRetailIMS.Application.Services.ReceiveTempService.Commands.CreateReceiveTemplate.v1;
public class CreateReceiveTemplateHandler : BaseService, IRequestHandler<CreateReceiveTemplateCommand, BaseResponse<CommandResponse>>
{
    public CreateReceiveTemplateHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreateReceiveTemplateCommand request, CancellationToken cancellationToken)
    {
        TMReceiveTemplate tmReceiveTemplateEntity = _mapper.Map<TMReceiveTemplate>(request);
        tmReceiveTemplateEntity.ActiveStatus();
        tmReceiveTemplateEntity.SetCreatedDate();
        tmReceiveTemplateEntity.AddDomainEvent(new TMReceiveTemplateCreateEvent(tmReceiveTemplateEntity));
        await _unitOfWork.Repository<TMReceiveTemplate>().AddAsync(tmReceiveTemplateEntity);
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
