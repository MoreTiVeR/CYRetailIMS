using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.CancelCountStock.v1;

/// <summary>
/// Handler: ยกเลิกรายการนับสต๊อกที่ส่งแล้ว โดยปรับสถานะกลับเป็น Draft(0)
/// </summary>
public class CancelCountStockHandler : BaseService, IRequestHandler<CancelCountStockCommand, BaseResponse<CommandResponse>>
{
    public CancelCountStockHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CancelCountStockCommand request, CancellationToken cancellationToken)
    {
        TTCountStock countStock = await _unitOfWork.Repository<TTCountStock>()
            .FirstOrDefaultAsync(w => w.CountStockID == request.countstockid && w.IsActive);

        if (countStock is null)
        {
            throw new Exception("ไม่พบข้อมูลนับสต๊อก กรุณาลองใหม่อีกครั้ง");
        }

        if (countStock.CountStockStatusID != 1)
        {
            throw new Exception("ยกเลิกได้เฉพาะรายการที่อยู่ในสถานะรออนุมัติ");
        }

        countStock.CountStockStatusID = 0; // Back to draft
        countStock.ApprovedBy = null;
        countStock.ApprovedDate = null;
        countStock.SetUpdatedBy(request.canceledby);
        countStock.SetUpdatedDate();

        _unitOfWork.Repository<TTCountStock>().Update(countStock);
        await _unitOfWork.SaveChangesAsync();

        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse { result = true },
            message = "ยกเลิกรายการนับสต๊อกสำเร็จ",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
