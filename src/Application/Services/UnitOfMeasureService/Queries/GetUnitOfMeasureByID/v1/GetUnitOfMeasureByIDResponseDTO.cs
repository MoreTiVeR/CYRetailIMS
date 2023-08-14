using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.UnitOfMeasureService.Queries.GetUnitOfMeasureByID.v1;

[Serializable]
public class GetUnitOfMeasureByIDResponseDTO
{
    public int unitofmeasureid { get; set; }

    public string unitofmeasurename { get; set; }

    public string description { get; set; }
}
