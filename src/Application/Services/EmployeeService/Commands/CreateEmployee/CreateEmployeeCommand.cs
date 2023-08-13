using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee;
public record CreateEmployeeCommand : IRequest<BaseResponse<CommandResponse>>
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [JsonPropertyName("departmentid")]
    public int departmentid { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(50, ErrorMessage ="Maximum length 50")]
    [JsonPropertyName("firstname")]
    public string firstname { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(50, ErrorMessage = "Maximum length 50")]
    [JsonPropertyName("lastname")]
    public string lastname { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(50, ErrorMessage = "Maximum length 50")]
    [JsonPropertyName("email")]
    public string email { get; set; }

    [JsonPropertyName("salary")]
    public decimal salary { get; set; }

    [JsonPropertyName("startworkingdate")]
    public DateTime startworkingdate { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(20, ErrorMessage = "Maximum length 20")]
    [JsonPropertyName("username")]
    public string username { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(20, ErrorMessage = "Maximum length 50")]
    [JsonPropertyName("password")]
    public string password { get; set; }

    [Required(ErrorMessage = "Required field")]
    [JsonPropertyName("roleid")]
    public int roleid { get; init; }
}
