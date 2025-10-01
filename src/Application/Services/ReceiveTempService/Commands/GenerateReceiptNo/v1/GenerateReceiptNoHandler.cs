using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace CYRetailIMS.Application.Services.ReceiveTempService.Commands.GenerateReceiptNo.v1;
public class GenerateReceiptNoHandler : BaseService, IRequestHandler<GenerateReceiptNoCommand, BaseResponse<GenerateReceiptNoResponseDTO>>
{
    public GenerateReceiptNoHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GenerateReceiptNoResponseDTO>> Handle(GenerateReceiptNoCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.branchcode))
        {
            throw new ValidationException("Branch Code is required");
        }

        await _unitOfWork.BeginTransactionAsync();
        DateOnly dtToday = DateOnly.FromDateTime(DateTime.Now);

        // หา running ล่าสุด
        TMReceiptNumber receiptNoEnt = await _unitOfWork.Repository<TMReceiptNumber>().Where(r => r.BranchCode == request.branchcode && r.ReceiptDate == dtToday).FirstOrDefaultAsync();
        //.OrderByDescending(r => r.RunningNo).Select(r => r.RunningNo).FirstOrDefaultAsync();

        //int newRunning = receiptNoEnt == null == 0 ? 1 : lastRunning + 1;
        int newRunning = 1;
        if (receiptNoEnt == null)
        {
            //Add new
            TMReceiptNumber tmReceiptNumberEnt = new TMReceiptNumber
            {
                BranchCode = request.branchcode,
                ReceiptDate = dtToday,
                RunningNo = newRunning
            };
            tmReceiptNumberEnt.AddDomainEvent(new TMReceiptNumberCreateEvent(tmReceiptNumberEnt));
            await _unitOfWork.Repository<TMReceiptNumber>().AddAsync(tmReceiptNumberEnt);
        }
        else
        {
            //Update
            newRunning = receiptNoEnt.RunningNo + 1;
            receiptNoEnt.RunningNo = newRunning;
            receiptNoEnt.AddDomainEvent(new TMReceiptNumberUpdateEvent(receiptNoEnt));
        }
        await _unitOfWork.SaveChangesAsync();
        await _unitOfWork.CommitTransactionAsync();

        var response = new BaseResponse<GenerateReceiptNoResponseDTO>
        {
            result = true,
            message = "Receipt No generated successfully",
            data = new GenerateReceiptNoResponseDTO
            {
                receiptno = $"{request.branchcode}-{newRunning:D3}-{DateTime.Now:yyyyMMdd}"
            },
            soruce = "DB",
            status = StatusCodes.Status200OK.ToString(),
        };
        return response;

    }
}
