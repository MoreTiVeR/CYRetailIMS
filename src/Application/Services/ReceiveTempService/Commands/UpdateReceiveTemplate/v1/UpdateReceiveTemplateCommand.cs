using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ReceiveTempService.Commands.UpdateReceiveTemplate.v1;
public record UpdateReceiveTemplateCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int receivetemplateid { get; set; }
    public int branchid { get; init; }
    public string shopheadernametext { get; init; }
    public string shopheaderaddresstext { get; init; }
    public string? additionalheadertext { get; init; }
    public string? shopfootertext { get; init; }
    public string? additionalfootertext { get; init; }
    public string telephoneno { get; init; }
    public string printername { get; init; }
    public string updatedby { get; init; }
    public bool isactive { get; set; }
}
