using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.PurchaseTypeService.Queries.GetPurchaseTypeList.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.PurchaseTypeService.Queries.PurchaseTypeByID.v1;
public record PurchaseTypeByIDCommand : IRequest<BaseResponse<GetPurchaseTypeResponseDTO>>
{
    public int purchasetypeid { get; init; }
}
