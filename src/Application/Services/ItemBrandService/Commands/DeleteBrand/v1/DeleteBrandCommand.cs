using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemBrandService.Commands.DeleteBrand.v1;
public record DeleteBrandCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int brandid { get; init; }
    public string updatedby { get; set; }
}
