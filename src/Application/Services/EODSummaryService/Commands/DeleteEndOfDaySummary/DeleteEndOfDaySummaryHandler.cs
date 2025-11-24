using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TTEndOfDaySummarys;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.EODSummaryService.Commands.DeleteEndOfDaySummary;
public class DeleteEndOfDaySummaryHandler : BaseService, IRequestHandler<DeleteEndOfDaySummaryCommand, BaseResponse<CommandResponse>>
{
    public DeleteEndOfDaySummaryHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(DeleteEndOfDaySummaryCommand request, CancellationToken cancellationToken)
    {
        var resEODSummary = await _unitOfWork.Repository<TTEndOfDaySummary>().FirstOrDefaultAsync(w => w.EndOfDayId == request.eodid);
        if (resEODSummary == null)
        {
            throw new Exception("ไม่สามารถทำรายการได้, เนื่องจากไม่พบข้อมูลสรุปยอดสิ้นวัน.");
        }

        resEODSummary.IsActive = request.isactive;
        resEODSummary.SetUpdatedBy(request.updatedby);
        resEODSummary.SetUpdatedDate(DateTime.Now);
        resEODSummary.AddDomainEvent(new TTEndOfDaySummaryUpdateEvent(resEODSummary));
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<CommandResponse>
        {
            result = true,
            status = StatusCodes.Status200OK.ToString(),
            message = "ปรับปรุงข้อมูลสรุปยอดสิ้นวันเรียบร้อยแล้ว.",
            data = new CommandResponse
            {
                result = true,
                transactionid = resEODSummary.EndOfDayId
            }
        };
    }
}
