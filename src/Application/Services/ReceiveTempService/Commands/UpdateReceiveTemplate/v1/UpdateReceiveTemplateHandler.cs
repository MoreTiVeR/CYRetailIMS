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

namespace CYRetailIMS.Application.Services.ReceiveTempService.Commands.UpdateReceiveTemplate.v1;
public class UpdateReceiveTemplateHandler : BaseService, IRequestHandler<UpdateReceiveTemplateCommand, BaseResponse<CommandResponse>>
{
    public UpdateReceiveTemplateHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(UpdateReceiveTemplateCommand request, CancellationToken cancellationToken)
    {
        TMReceiveTemplate resEnt = await _unitOfWork.Repository<TMReceiveTemplate>().FirstOrDefaultAsync(w => w.ReceiveTempID == request.receivetemplateid);
        if(resEnt == null)
        {
            throw new Exception("ไม่พบข้อมูล");
        }
        UpdateEntity(resEnt, request);
        resEnt.AddDomainEvent(new TMReceiveTemplateUpdateEvent(resEnt));
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

    private void UpdateEntity(TMReceiveTemplate entity, UpdateReceiveTemplateCommand request)
    {
        entity.BranchID = request.branchid;
        entity.ShopHeaderNameText = request.shopheadernametext;
        entity.ShopHeaderAddressText = request.shopheaderaddresstext;
        entity.AdditionalHeaderText = request.additionalheadertext;
        entity.ShopFooterText = request.shopfootertext;
        entity.AdditionalFooterText = request.additionalfootertext;
        entity.TelephoneNo = request.telephoneno;
        entity.UpdatedBy = request.updatedby;
        entity.IsActive = request.isactive;
        entity.SetUpdatedDate();
    }
}
