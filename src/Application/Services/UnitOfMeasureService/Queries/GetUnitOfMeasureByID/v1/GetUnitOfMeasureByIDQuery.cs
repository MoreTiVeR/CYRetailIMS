using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.UnitOfMeasureService.Queries.GetUnitOfMeasureByID.v1;

public record class GetUnitOfMeasureByIDQuery : IRequest<BaseResponse<GetUnitOfMeasureByIDResponseDTO>>
{
    [Required(ErrorMessage = "unit of measure must more than or equal 1")]
    public int unitofmeasureid { get; init; }
}
