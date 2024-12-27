using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.SubItemTypeService.Commands.CreateSubItemType.v1;
public record CreateSubItemTypeDetail
{
    public string subitemcode { get; init; }
    public string subtypename_th { get; init; }
    public string subTypename_en { get; init; }
    public string description { get; init; }
    public string createdby { get; init; }
    public DateTime createddate { get; init; }
}
