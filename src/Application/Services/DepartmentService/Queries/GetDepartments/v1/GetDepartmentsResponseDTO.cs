using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartments.v1;

[Serializable]
public class GetDepartmentsResponseDTO
{
    public int departmentid { get; set; }

    public string departmentname { get; set; }

    public string description { get; set; }

    public string createdby { get; set; }

    public DateTime creadeddate { get; set; }

    public bool isactive { get; set; }
}
