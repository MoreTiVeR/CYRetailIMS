using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.SupplierService.Commands.CreateSupplier.v1;

namespace CYRetailIMS.Application.Services.SupplierService.Commands.UpdateSupplier.v1;
public record UpdateSupplierContact
{
    
    public int suppliercontacttypeid { get; init; }
    public string contactaccountname { get; init; }
    public string contactperson { get; init; }
    public string mobileno { get; init; }
    public string desctiption { get; init; }
}
