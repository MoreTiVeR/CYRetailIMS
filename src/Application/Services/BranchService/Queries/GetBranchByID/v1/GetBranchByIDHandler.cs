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
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
public class GetBranchByIDHandler : BaseService, IRequestHandler<GetBranchByIDQuery, BaseResponse<GetBranchByIDResponseDTO>>
{
	public GetBranchByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
	{
	}

	public async Task<BaseResponse<GetBranchByIDResponseDTO>> Handle(GetBranchByIDQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<TMBranch> resBrach = await _unitOfWork.Repository<TMBranch>().FindWithInclude(w => w.BranchID == request.branchid && w.IsActive == true, i => i.Include(ii => ii.TMBranchDetail));
		if (!resBrach.Any())
		{
			throw new Exception("Data not found");
		}
		return new BaseResponse<GetBranchByIDResponseDTO>
		{
			result = true,
			data = _mapper.Map<GetBranchByIDResponseDTO>(resBrach.FirstOrDefault()),
			message = "Success",
			soruce = "db",
			status = StatusCodes.Status200OK.ToString()
		};
	}
}
