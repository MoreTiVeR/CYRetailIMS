using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee;
public record CreateEmployeeCommand : IRequest<BaseResponse<CommandResponse>>
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    public int DepartmentID { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(50, ErrorMessage ="Maximum length 50")]
    public string FirstName { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(50, ErrorMessage = "Maximum length 50")]
    public string LastName { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(50, ErrorMessage = "Maximum length 50")]
    public string Email { get; set; }

    public decimal Salary { get; set; }
    public DateTime StartWorkingDate { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(20, ErrorMessage = "Maximum length 20")]
    public string UserName { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Required field")]
    [MaxLength(20, ErrorMessage = "Maximum length 50")]
    public string Password { get; set; }
}
