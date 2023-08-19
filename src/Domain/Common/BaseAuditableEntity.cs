using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CYRetailIMS.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    //public DateTime CreatedDate { get; set; }

    //public string CreatedBy { get; set; }

    //public DateTime? UpdatedDate { get; set; }

    //public string? UpdatedBy { get; set; }

    //public bool Status { get; set; }
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
    public bool IsActive { get; set; }

    public void ActiveStatus() => IsActive = true;

    public void DeActiveStatus() => IsActive = false;

    public void SetCreatedDate() => CreadedDate = DateTime.Now;

    public void SetCreatedBy(string userName = "") => CreatedBy = !string.IsNullOrEmpty(userName) ? CreatedBy = userName : CreatedBy = "SYSTEM";

    public void SetUpdatedDate() => UpdatedDate = DateTime.Now;

    public void SetUpdatedBy(string userName = "") => UpdatedBy = !string.IsNullOrEmpty(userName) ? UpdatedBy = userName : UpdatedBy = "SYSTEM";
}
