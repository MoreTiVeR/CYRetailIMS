using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.BranchService.Queries.GetBranchList.v1;

[Serializable]
public record GetBranchListQuery : IRequest<BaseResponse<List<GetBranchListResponseDTO>>> { }