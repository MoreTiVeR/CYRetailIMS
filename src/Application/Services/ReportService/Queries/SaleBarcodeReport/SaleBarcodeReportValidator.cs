using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleBarcodeReport.v1;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleBarcodeReport;
public class SaleBarcodeReportValidator : AbstractValidator<SaleBarcodeReportQuery>
{
    public SaleBarcodeReportValidator()
    {
        RuleFor(x => x.transaction_startdate)
            .NotEmpty().WithMessage("กรุณาระบุวันที่เริ่มต้นรายการที่ต้องการค้นหา.")
            .LessThanOrEqualTo(x => x.transaction_enddate).WithMessage("วันที่เริ่มต้น ไม่น้อยกว่าวันที่สิ้นสุดการต้นหารายการ.");
        RuleFor(x => x.transaction_enddate)
            .NotEmpty().WithMessage("กรุณาระบุวันที่สิ้นสุดรายการที่ต้องการค้นหา.")
            .GreaterThanOrEqualTo(x => x.transaction_startdate).WithMessage("วันที่สินสุด ควรมากกว่าวันที่เริมต้นค้นหารายการ.");
    }   
}
