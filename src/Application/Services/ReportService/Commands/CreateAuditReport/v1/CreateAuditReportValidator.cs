using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ReportService.Commands.CreateAuditReport.v1;
public class CreateAuditReportValidator : AbstractValidator<CreateAuditReportCommand>
{
    public CreateAuditReportValidator()
    {
        RuleFor(w => w.transactionid).NotNull().Must(w => w > 0).WithMessage("ข้อมูลประวัติที่ทำการตรวจสอบไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        RuleFor(w => w.totalamountaudit).NotNull().Must(w => w > 0).WithMessage("ยอดเงินบัญชีไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        //RuleFor(r => r.totalamountaudit).Must((r, totalAuditAmount) => IsValidTotalAmountAudit(r, totalAuditAmount)).WithMessage("ไม่สามารถทำรายการได้, เนื่องจากยอดเงินรวมทั้งหมดไม่ตรงกับยอดขาย (หากมีเงินฝากกรุณาระบุค่าธรรมเนียม)");
        RuleFor(r => r.createdby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
        //RuleFor(w => w.transactionid).NotNull().Must(w => w > 0).WithMessage("ข้อมูลประวัติที่ทำการตรวจสอบไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
    }

    //private bool IsValidTotalAmountAudit(CreateAuditReportCommand command, decimal totalAmount)
    //{
    //    //If have amountdeposit (Back Deposit) must have fee
    //    if (command. > 0)
    //    {
    //        if (command.fee <= 0)
    //        {
    //            return false;
    //        }
    //    }

    //    // Calculate the sum of Amount Cash and Amount Deposit
    //    decimal calculatedTotal = command.amounttransfer + command.amountdeposit + command.amountcash + command.fee;

    //    // Compare the calculated total with the provided Total Amount
    //    return totalAmount == calculatedTotal;
    //}
}
