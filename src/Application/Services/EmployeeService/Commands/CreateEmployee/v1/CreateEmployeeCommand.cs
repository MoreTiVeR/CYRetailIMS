using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee.v1;

[Serializable]
public record CreateEmployeeCommand : IRequest<BaseResponse<CommandResponse>>
{
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

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(50, ErrorMessage = "Maximum length 50")]
    [JsonPropertyName("email")]
    public string email { get; init; }

    [MaxLength(10, ErrorMessage = "Maximum length 50")]
    [JsonPropertyName("mobileno")]
    public string mobileno { get; init; }

    [JsonPropertyName("salary")]
    public decimal salary { get; init; }

    [JsonPropertyName("startworkingdate")]
    public DateTime startworkingdate { get; init; }

    public string nickname { get; set; }

    [Required(ErrorMessage = "Required field")]
    public bool IsActive { get; set; }

    //[Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    //[MaxLength(20, ErrorMessage = "Maximum length 20")]
    //[JsonPropertyName("username")]
    //public string username { get; init; }

    //[Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    //[MaxLength(20, ErrorMessage = "Maximum length 50")]
    //[JsonPropertyName("password")]
    //public string password { get; init; }

    //[Required(ErrorMessage = "Required field")]
    //[JsonPropertyName("roleid")]
    //public int roleid { get; init; }

    [Required(ErrorMessage = "Required field")]
    public string createdby { get; init; }

    [Required(ErrorMessage = "Required field")]
    public DateTime createddate { get; init; }

    //[Required(ErrorMessage = "Required field")]
    //public int userinbranchid { get; set; }
}
