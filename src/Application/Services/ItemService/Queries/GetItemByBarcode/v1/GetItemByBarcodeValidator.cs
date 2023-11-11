using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ItemService.Queries.GetItemByBarcode.v1;
public class GetItemByBarcodeValidator : AbstractValidator<GetItemByBarcodeQuery>
{
    public GetItemByBarcodeValidator()
    {
        RuleFor(w => w.itembarcode).NotEmpty().WithMessage("กรุณาระบุบาร์โค้ดสินค้า");
    }
}
