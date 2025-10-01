using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ReceiveTempService.Commands.GenerateReceiptNo.v1;
public record GenerateReceiptNoCommand : IRequest<BaseResponse<GenerateReceiptNoResponseDTO>>
{
    [Required]
    public string branchcode { get; init; }
}
