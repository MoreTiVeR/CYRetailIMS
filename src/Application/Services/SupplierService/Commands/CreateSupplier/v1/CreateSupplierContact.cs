using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.SupplierService.Commands.CreateSupplier.v1;
public record CreateSupplierContact
{
    public int suppliercontacttypeid { get; init; }
    public string contactaccountname { get; init; }
    public string contactperson { get; init; }
    public string mobileno { get; init; }
    public string desctiption { get; init; }

}
