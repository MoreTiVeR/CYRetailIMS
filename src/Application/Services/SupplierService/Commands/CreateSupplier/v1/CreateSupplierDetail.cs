using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.SupplierService.Commands.CreateSupplier.v1;
public record CreateSupplierDetail
{
    public string address { get; init; }
    public string city { get; init; }
    public int zipcode { get; init; }
    public string phone { get; init; }
    public string description { get; init; }

    public int MyProperty { get; set; }
}
