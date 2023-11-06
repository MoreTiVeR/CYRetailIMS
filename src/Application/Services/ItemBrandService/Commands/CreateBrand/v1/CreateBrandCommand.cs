using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemBrandService.Commands.CreateBrand.v1;
public record CreateBrandCommand : IRequest<BaseResponse<CommandResponse>>
{

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    public string brandname { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    public string brandshortname { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    public string description { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    public string createdby { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    public DateTime createddate { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    public bool isactive { get; init; }
}
