using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemBrandService.Commands.UpdateBrand.v1;
public record UpdateBrandCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int brandid { get; init; }
    public string brandname { get; init; }
    public string brandshortname { get; init; }
    public string desription { get; init; }
    public string updatedby { get; init; }
    public bool isactive { get; init; }
}
