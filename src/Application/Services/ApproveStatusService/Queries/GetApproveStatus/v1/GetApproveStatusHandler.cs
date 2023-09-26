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

namespace CYRetailIMS.Application.Services.ApproveStatusService.Queries.GetApproveStatus.v1;
public class GetApproveStatusHandler : BaseService, IRequestHandler<GetApproveStatusQuery, BaseResponse<List<GetApproveStatusResponseDTO>>>
{
	public GetApproveStatusHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
	{
	}

	public async Task<BaseResponse<List<GetApproveStatusResponseDTO>>> Handle(GetApproveStatusQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<TMApproveStatus> resStatus = await _unitOfWork.Repository<TMApproveStatus>().FindListAsync(w => w.IsActive);
		if (!resStatus.Any() || resStatus.Count() == 0)
		{
			throw new Exception("ไม่พบข้อมูลสถานะอนุมัติ");
		}

		return new BaseResponse<List<GetApproveStatusResponseDTO>>
		{
			result = true,
			data = _mapper.Map<List<GetApproveStatusResponseDTO>>(resStatus),
			message = "Success",
			soruce = "db",
			status = StatusCodes.Status200OK.ToString()
		};
	}
}
