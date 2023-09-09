using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;
using Newtonsoft.Json;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchList.v1;

[Serializable]
public record GetItemInBranchByBranchListQuery : IRequest<BaseResponse<List<GetItemInBranchByBranchListResponseDTO>>>
{
    //[Required(ErrorMessage = "รหัสสาขาไม่ถูกต้อง")]
    //[JsonProperty(Required = Required.Always)]
    public List<int> branchid_list { get; init; }
}
