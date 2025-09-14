using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ReceiveTempService.Commands.CreateReceiveTemplate.v1;
public record CreateReceiveTemplateCommand: IRequest<BaseResponse<CommandResponse>>
{
    public int branchid { get; init; }
    public string shopheadernametext { get; init; }
    public string shopheaderaddresstext { get; init; }
    public string? additionalheadertext { get; init; }
    public string? shopfootertext { get; init; }
    public string? additionalfootertext { get; init; }
    public string telephoneno { get; init; }
    public string createdby { get; init; }
}
