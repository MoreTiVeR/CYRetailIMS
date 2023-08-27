using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.BranchService.Queries.GetBranchList.v1;
public class GetBranchListHandler : BaseService, IRequestHandler<GetBranchListQuery, BaseResponse<List<GetBranchListResponseDTO>>>
{
    public GetBranchListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetBranchListResponseDTO>>> Handle(GetBranchListQuery request, CancellationToken cancellationToken)
    {
		IEnumerable<TMBranch> resBrach = await _unitOfWork.Repository<TMBranch>().FindWithInclude(w => w.IsActive == true, i => i.Include(ii => ii.TMBranchDetail));
		if (!resBrach.Any())
		{
			throw new Exception("Data not found");
		}
		return new BaseResponse<List<GetBranchListResponseDTO>>
		{
			result = true,
			data = _mapper.Map<List<GetBranchListResponseDTO>>(resBrach),
			message = "Success",
			soruce = "db",
			status = StatusCodes.Status200OK.ToString()
		};
	}
}
