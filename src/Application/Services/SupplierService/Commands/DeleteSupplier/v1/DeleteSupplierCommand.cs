using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.SupplierService.Commands.DeleteSupplier.v1;

[Serializable]
public record DeleteSupplierCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int supplierid { get; init; }
    public string deleteddby { get; init; }
    public DateTime deleteddate { get; init; }
}
