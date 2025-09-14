using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Extensions;
public static class VatCalculatorExtension
{
    public static decimal ToExVat(this decimal totalPrice)
    {
        try
        {
            var vat = Math.Round((totalPrice * 7) / 100, MidpointRounding.AwayFromZero);
            return vat;
        }
        catch
        {
            return 0;
        }
    }
}
