using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByTransactionID.v1;
public class GetTransactionByTransactionIDValidator : AbstractValidator<GetTransactionByTransactionIDQuery>
{
    public GetTransactionByTransactionIDValidator()
    {
        RuleFor(w => w.transactionid).NotNull().Must(w => w > 0).WithMessage("ข้อมูลลงขายที่ต้องการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
    }
}
