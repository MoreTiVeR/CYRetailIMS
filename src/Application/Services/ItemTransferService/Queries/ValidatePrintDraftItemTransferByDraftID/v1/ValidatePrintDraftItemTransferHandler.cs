using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.ValidatePrintDraftItemTransferByDraftID.v1;
public class ValidatePrintDraftItemTransferHandler : BaseService, IRequestHandler<ValidatePrintDraftItemTransferQuery, BaseResponse<ValidatePrintDraftItemTransferResponseDTO>>
{
    public ValidatePrintDraftItemTransferHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<ValidatePrintDraftItemTransferResponseDTO>> Handle(ValidatePrintDraftItemTransferQuery request, CancellationToken cancellationToken)
    {
        TTDraftItemTransfer res = await _unitOfWork.Repository<TTDraftItemTransfer>().FirstOrDefaultAsync(w => w.TransferHeaderID == request.draftid);
        if(res == null)
        {
            throw new Exception("Data not found");
        }

        if(!res.IsActive || res.TransferStatus != (int)EnumModel.TransferStatus.Received)
        {
            return new BaseResponse<ValidatePrintDraftItemTransferResponseDTO>
            {
                result = false,
                data = new ValidatePrintDraftItemTransferResponseDTO { ispass = false, remark = "ไม่สามารถทำรายการได้ เนื่องจากข้อมูลยังไม่บันทึกโอนหรือถูกยกเลิก" },
                message = "Validation failed",
                soruce = "db",
                status = StatusCodes.Status200OK.ToString()
            };
        }
        return new BaseResponse<ValidatePrintDraftItemTransferResponseDTO>
        {
            result = true,
            data = new ValidatePrintDraftItemTransferResponseDTO { ispass = true, remark = "Success" },
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };

    }
}
