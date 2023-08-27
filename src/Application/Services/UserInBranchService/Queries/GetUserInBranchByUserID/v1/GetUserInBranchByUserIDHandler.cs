using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.AccountService.Queries.Login.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.UserInBranchService.Queries.GetUserInBranchByUserID.v1;
public class GetUserInBranchByUserIDHandler : BaseService, IRequestHandler<GetUserInBranchByUserIDQuery, BaseResponse<GetUserInBranchByUserIDResponseDTO>>
{
	public GetUserInBranchByUserIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
	{
	}

	public async Task<BaseResponse<GetUserInBranchByUserIDResponseDTO>> Handle(GetUserInBranchByUserIDQuery request, CancellationToken cancellationToken)
	{
		IQueryable<TMUserInBranch> resUserInBranch = await _unitOfWork.Repository<TMUserInBranch>().FindWithInclude(w => w.UserID == request.userid && w.IsActive, i => i.Include(x => x.Branch));
		if (!resUserInBranch.Any())
		{
			throw new Exception("Data not found");
		}
		GetUserInBranchByUserIDResponseDTO resData = resUserInBranch.GroupBy(g => g.UserID).Select(s => new GetUserInBranchByUserIDResponseDTO
		{
			userid = s.Key,
			branchs = (from a in s
					   select new GetUserInBranchByUserIDBrancResponseDTO
					   {
						   branchid = a.BranchID,
						   branchcode = a.Branch.BranchCode,
						   branchname = a.Branch.BranchName
					   }).OrderBy(o => o.branchid).ToList()
		}).FirstOrDefault();

		return new BaseResponse<GetUserInBranchByUserIDResponseDTO>
		{
			result = true,
			data = resData,
			message = "Sucess",
			soruce = "db",
			status = StatusCodes.Status200OK.ToString()
		};
	}
}
