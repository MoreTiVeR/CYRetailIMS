using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferList.v1;

[Serializable]
public record GetItemTransferListQuery : IRequest<BaseResponse<GetItemTransferListResponseDTO>> 
{
    public DateTime? transferstartdate { get; init; }
    public DateTime? transferenddate { get; init; }
    public int? transferstatusid { get; init; }
    public int? branchid { get; init; }

    /// <summary>
    /// Start index 1
    /// </summary>
    public int startrow { get; set; }

    /// <summary>
    /// Total row take
    /// </summary>
    public int pagesize { get; set; }

    public string? searchvalue { get; set; }

    public bool isexportalldata { get; set; }
}
