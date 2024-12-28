using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMSubItemTypes;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.SubItemTypeService.Commands.CreateSubItemType.v1;
public class CreateSubItemTypeHandler : BaseService, IRequestHandler<CreateSubItemTypeCommand, BaseResponse<CommandResponse>>
{
    public CreateSubItemTypeHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreateSubItemTypeCommand request, CancellationToken cancellationToken)
    {
        #region Check dupplicate SubItemCode
        var subitemcodes = request.subitemtypelist.Select(s => s.subitemcode).ToList();
        var resSupplicateData = await _unitOfWork.Repository<TMSubItemType>().FindListAsync(w => subitemcodes.Contains(w.SubItemCode));
        if (resSupplicateData.Any())
        {
            var dupCodes = resSupplicateData.Select(s => s.SubItemCode).Aggregate((s, t) => s + " " + t);
            throw new Exception($"ไม่สามารถทำรายการได้ เนื่องจากมีประเภทย่อยในระบบแล้ว {dupCodes}");
        }
        #endregion

        DateTime createDate = DateTime.Now;
        List<TMSubItemType> subItemtype = _mapper.Map<List<TMSubItemType>>(request.subitemtypelist);
        subItemtype.ForEach(e =>
        {
            e.SetCreatedDate(createDate);
            e.AddDomainEvent(new TMSubItemTypeCreateEvent(e));
        });
        await _unitOfWork.Repository<TMSubItemType>().AddRangeAsync(subItemtype);
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse { result = true },
            message = "Success",
            status = StatusCodes.Status200OK.ToString(),
            soruce = "db"
        };
    }
}
