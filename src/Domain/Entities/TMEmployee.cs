using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMEmployee")]
public partial class TMEmployee
{
    [Key]
    public int EmpID { get; set; }

    public int DepartmentID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? Salary { get; set; }

    [Column(TypeName = "date")]
    public DateTime? StartWorkingDate { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string CreatedBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreadedDate { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? UpdatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    [Required]
    public bool? Status { get; set; }

    [ForeignKey("DepartmentID")]
    [InverseProperty("TMEmployees")]
    public virtual TMDepartment Department { get; set; } = null!;

    [InverseProperty("Emp")]
    public virtual ICollection<TMUser> TMUsers { get; set; } = new List<TMUser>();
}
