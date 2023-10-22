using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.SupplierService.Commands.CreateSupplier.v1;

[Serializable]
public record CreateSupplierCommand : IRequest<BaseResponse<CommandResponse>>
{
    public string suppliernameth { get; init; }
    public string suppliernameen { get; init; }
    public int suppliertypeid { get; init; }
    public string description { get; init; }

    //public List<CreateSupplierDetail> detail { get; set; }
    public List<CreateSupplierContact> contact { get; init; }

    public string createdby { get; init; }
    public DateTime createddate { get; init; }

}
