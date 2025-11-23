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

namespace CYRetailIMS.Application.Services.EODSummaryService.Commands.CreateEndOfDaySummary;
public class CreateEndOfDaySummaryHandler : BaseService, IRequestHandler<CreateEndOfDaySummaryCommand, BaseResponse<CommandResponse>>
{
    public CreateEndOfDaySummaryHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreateEndOfDaySummaryCommand request, CancellationToken cancellationToken)
    {
        var isExist = await _unitOfWork.Repository<TTEndOfDaySummary>().AnyAsync(x => x.BranchID == request.branchid && x.SummaryDate.Date == request.summarydate.Date);
        if (isExist)
        {
            throw new Exception("ไม่สามารถทำรายการได้, เนื่องจากสาขาดังกล่าวได้ทำการสรุปยอดสิ้นวันเรียบร้อยแล้ว.");
        }
        TTEndOfDaySummary entity = _mapper.Map<TTEndOfDaySummary>(request);
        entity.SetCreatedDate();
        //var entity = new TTEndOfDaySummary
        //{
        //    BranchID = request.BranchID,
        //    SummaryDate = request.SummaryDate,
        //    TotalCash = request.TotalCash,
        //    DepositedCash = request.DepositedCash,
        //    TotalTransfer = request.TotalTransfer,
        //    CustomerTransfer = request.CustomerTransfer,
        //    GrandTotal = request.GrandTotal,
        //    SubstituteWage = request.SubstituteWage,
        //    Fee = request.Fee,
        //    OtherExpense = request.OtherExpense,
        //    OtherExpenseNote = request.OtherExpenseNote,
        //    FinalTotal = request.FinalTotal,
        //    IsActive = request.IsActive,
        //    CreatedBy = request.CreatedBy,
        //    CreatedDate = DateTime.Now,
        //    UpdatedBy = null,
        //    UpdatedDate = null
        //};
        entity.AddDomainEvent(new TTEndOfDaySummaryCreateEvent(entity));
        await _unitOfWork.Repository<TTEndOfDaySummary>().AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse
            {
                result = true,
                transactionid = entity.EndOfDayId,
            },
            status = StatusCodes.Status200OK.ToString(),
            message = "Success",
            soruce = "db"
        };
    }
}
