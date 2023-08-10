using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

public partial class TMDepartment : BaseAuditableEntity
{
    [Key]
    public int DepartmentID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string DepartmentName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Description { get; set; }

    [InverseProperty("Department")]
    public virtual ICollection<TMEmployee> TMEmployees { get; set; } = new List<TMEmployee>();
}
