using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.SubItemTypeService.Commands.CreateSubItemType.v1;
public record CreateSubItemTypeCommand : IRequest<BaseResponse<CommandResponse>>
{
    public List<CreateSubItemTypeDetail> subitemtypelist { get; set; }

}
