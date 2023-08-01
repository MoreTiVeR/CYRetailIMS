using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMEmployee")]
public partial class TMEmployee : BaseAuditableEntity
{
    [Key]
    public int EmpID { get; set; }

    public int UserID { get; set; }

    public int DepartmentID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? NickName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? Salary { get; set; }

    [Column(TypeName = "date")]
    public DateTime? StartWorkingDate { get; set; }

    [ForeignKey("DepartmentID")]
    [InverseProperty("TMEmployees")]
    public virtual TMDepartment Department { get; set; } = null!;

    [ForeignKey("UserID")]
    [InverseProperty("TMEmployees")]
    public virtual TMUser User { get; set; } = null!;
}
