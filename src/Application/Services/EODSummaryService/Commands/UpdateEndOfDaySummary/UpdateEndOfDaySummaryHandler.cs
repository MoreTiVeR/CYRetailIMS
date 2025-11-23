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
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.EODSummaryService.Commands.UpdateEndOfDaySummary;
public class UpdateEndOfDaySummaryHandler : BaseService, IRequestHandler<UpdateEndOfDaySummaryCommand, BaseResponse<CommandResponse>>
{
    public UpdateEndOfDaySummaryHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(UpdateEndOfDaySummaryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<TTEndOfDaySummary>().FirstOrDefaultAsync(x => x.EndOfDayId == request.endofdayid);
        if(entity == null)
        {
            throw new Exception("ไม่สามารถทำรายการได้, เนื่องจากไม่พบข้อมูลสรุปยอดสิ้นวัน.");
        }

        entity.SummaryDate = request.summarydate;
        entity.TotalCash = request.totalcash;
        entity.DepositedCash = request.depositedcash;
        entity.TotalTransfer = request.totaltransfer;
        entity.CustomerTransfer = request.customertransfer;
        entity.GrandTotal = request.grandtotal;
        entity.SubstituteWage = request.substitutewage;
        entity.Fee = request.fee;
        entity.OtherExpense = request.otherexpense;
        entity.OtherExpenseNote = request.otherexpensenote;
        entity.FinalTotal = request.finaltotal;
        entity.IsActive = request.isactive;
        entity.SetUpdatedBy(request.updatedby);
        entity.SetUpdatedDate();
        entity.AddDomainEvent(new TTEndOfDaySummaryUpdateEvent(entity));
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse
            {
                result = true
            },
            status = StatusCodes.Status200OK.ToString(),
            message = "Success",
            soruce = "db"
        };
    }
}
