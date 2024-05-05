using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByCriteria.v1;
public class GetTransactionByCriteriaValidator : AbstractValidator<GetTransactionByCriteriaQuery>
{
    public GetTransactionByCriteriaValidator()
    {
        RuleFor(r => r.branchid).NotNull().NotEmpty().Must(w => w > 0).WithMessage("กรุณาระบุรหัสสาขาให้ถูกต้อง");
        RuleFor(r => r.transactionid).NotNull().NotEmpty().Must(w => w > 0).WithMessage("กรุณาระบุข้อมูลรายการ");
    }
}
