using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.SubmitCountStock.v1;

/// <summary>
/// Handler: เปลี่ยนสถานะการนับสต๊อกจาก Draft(0) เป็น Submitted(1)
/// เพื่อส่งข้อมูลให้ audit/หัวหน้าตรวจสอบ
/// </summary>
public class SubmitCountStockHandler : BaseService, IRequestHandler<SubmitCountStockCommand, BaseResponse<CommandResponse>>
{
    public SubmitCountStockHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(SubmitCountStockCommand request, CancellationToken cancellationToken)
    {
        TTCountStock countStock = await _unitOfWork.Repository<TTCountStock>()
            .FirstOrDefaultAsync(w => w.CountStockID == request.countstockid && w.IsActive);

        if (countStock is null)
        {
            throw new Exception("ไม่พบข้อมูลนับสต๊อก กรุณาลองใหม่อีกครั้ง");
        }

        if (countStock.CountStockStatusID != 0)
        {
            throw new Exception("ไม่สามารถส่งข้อมูลได้ เนื่องจากรายการนี้ถูกส่งไปแล้ว");
        }

        countStock.CountStockStatusID = 1; // Submitted
        countStock.SetUpdatedBy(request.submittedby);
        countStock.SetUpdatedDate();

        _unitOfWork.Repository<TTCountStock>().Update(countStock);
        await _unitOfWork.SaveChangesAsync();

        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse { result = true },
            message = "ส่งข้อมูลนับสต๊อกสำเร็จ",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
