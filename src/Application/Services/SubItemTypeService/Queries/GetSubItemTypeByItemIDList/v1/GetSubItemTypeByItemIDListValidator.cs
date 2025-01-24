
using FluentValidation;


namespace CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeByItemIDList.v1;
public class GetSubItemTypeByItemIDListValidator : AbstractValidator<GetSubItemTypeByItemIDListQuery>
{
    public GetSubItemTypeByItemIDListValidator()
    {
        RuleForEach(s => s.itemids).NotEmpty().NotNull().Must(s => s >= 1).WithMessage("ข้อมูลสินค้าไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
    }
}
