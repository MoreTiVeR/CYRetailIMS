using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TTTransactionAudits;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ReportService.Commands.CreateAuditReport.v1;
public class CreateAuditReportHandler : BaseService, IRequestHandler<CreateAuditReportCommand, BaseResponse<CommandResponse>>
{
    public CreateAuditReportHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreateAuditReportCommand request, CancellationToken cancellationToken)
    {
        TTTransactionAudit resExist = await _unitOfWork.Repository<TTTransactionAudit>().FirstOrDefaultAsync(w => w.TransactionID == request.transactionid);
        if(resExist != null)
        {
            throw new Exception("ไม่สามารถทำรายการได้ เนื่องรายการนี้ได้ทำการตรวจสอบเรียกร้อยแล้ว");
        }

        TTTransactionAudit autiEntity = new TTTransactionAudit
        {
            TransactionID = request.transactionid,
            TotalAuditAmount = request.totalamountaudit,
            Description = request.description
        };
        autiEntity.SetCreatedBy(request.createdby);
        autiEntity.SetCreatedDate(request.createddate);
        autiEntity.ActiveStatus();
        autiEntity.AddDomainEvent(new TTTransactionAuditCreateEvent(autiEntity));
        await _unitOfWork.Repository<TTTransactionAudit>().AddAsync(autiEntity);
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
}
