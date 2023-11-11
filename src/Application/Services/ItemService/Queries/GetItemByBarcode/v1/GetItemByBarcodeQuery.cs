using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemByID.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemService.Queries.GetItemByBarcode.v1;

[Serializable]
public record GetItemByBarcodeQuery : IRequest<BaseResponse<GetItemByIDResponseDTO>>
{
    public string itembarcode { get; init; }
}
