using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.EmployeeService.Queries.GetEmployee.v1;

[Serializable]
public class GetEmployeeResponseDTO
{
    public int empid { get; set; }

    public string empcode { get; set; }

    public string username { get; set; }

    public int departmentid { get; set; }

    public string departmentname { get; set; }

    public string firstname { get; set; }

    public string lastname { get; set; }

    public string nickname { get; set; }

    public string email { get; set; }

    public string mobileno { get; set; }

    public decimal? salary { get; set; }

    public DateTime? startworkingdate { get; set; }

    public string createdby { get; set; }

    public DateTime createddate { get; set; }

    public bool isactive { get; set; }

    public bool isregister { get; set; }
}
