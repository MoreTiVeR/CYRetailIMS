using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ApproveStatusService.Queries.GetApproveStatus.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemTransferStatusService.Queries.GetItemTransferStatus.v1;
public class GetItemTransferStatusHandler : BaseService, IRequestHandler<GetItemTransferStatusQuery, BaseResponse<List<GetItemTransferStatusResponseDTO>>>
{
	public GetItemTransferStatusHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
	{
	}

	public async Task<BaseResponse<List<GetItemTransferStatusResponseDTO>>> Handle(GetItemTransferStatusQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<TMItemTransferStatus> resTransferStatus = await _unitOfWork.Repository<TMItemTransferStatus>().FindListAsync(w => w.IsActive);
		if (!resTransferStatus.Any() || resTransferStatus.Count() == 0)
		{
			throw new Exception("ไม่พบข้อมูลสถานะรับสินค้า");
		}
		return new BaseResponse<List<GetItemTransferStatusResponseDTO>>
		{
			result = true,
			data = _mapper.Map<List<GetItemTransferStatusResponseDTO>>(resTransferStatus),
			message = "Success",
			soruce = "db",
			status = StatusCodes.Status200OK.ToString()
		};
	}
}
