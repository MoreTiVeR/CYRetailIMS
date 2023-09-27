using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.EmployeeService.Commands.UpdateEmployee.v1;

[Serializable]
public record UpdateEmployeeCommand : IRequest<BaseResponse<CommandResponse>>
{
    [Required(ErrorMessage = "Required field")]
    [JsonPropertyName("empid")]
    public int empid { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [JsonPropertyName("departmentid")]
    public int departmentid { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(50, ErrorMessage = "Maximum length 50")]
    [JsonPropertyName("firstname")]
    public string firstname { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(50, ErrorMessage = "Maximum length 50")]
    [JsonPropertyName("lastname")]
    public string lastname { get; init; }

    [JsonPropertyName("nickname")]
    public string nickname { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(50, ErrorMessage = "Maximum length 50")]
    [JsonPropertyName("email")]
    public string email { get; init; }

    [MaxLength(10, ErrorMessage = "Maximum length 50")]
    [JsonPropertyName("mobileno")]
    public string mobileno { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(10, ErrorMessage = "Maximum length 10")]
    public string updatedby { get; init; }

    [Required(ErrorMessage = "Required field")]
    public DateTime updateddate { get; init; }

    public bool isactive { get; set; }
}
