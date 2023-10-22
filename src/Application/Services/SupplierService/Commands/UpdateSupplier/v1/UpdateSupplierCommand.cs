using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.SupplierService.Commands.CreateSupplier.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.SupplierService.Commands.UpdateSupplier.v1;

[Serializable]
public record UpdateSupplierCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int supplierid { get; set; }
    public string suppliernameth { get; init; }
    public string suppliernameen { get; init; }
    public int suppliertypeid { get; init; }
    public string description { get; init; }
    public List<UpdateSupplierContact> contact { get; init; }
    public string updatedby { get; init; }
    public DateTime updateddate { get; init; }
}
