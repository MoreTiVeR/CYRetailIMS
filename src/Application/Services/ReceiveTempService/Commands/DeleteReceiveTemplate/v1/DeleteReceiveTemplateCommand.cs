using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ReceiveTempService.Commands.DeleteReceiveTemplate.v1;
public record DeleteReceiveTemplateCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int receivetemplateid { get; set; }
    public string updatedby { get; set; }
}
