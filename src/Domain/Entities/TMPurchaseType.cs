using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMPurchaseType")]
public partial class TMPurchaseType
{
    [Key]
    public int PurchaseTypeID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PurchaseTypeName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Description { get; set; }

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

    [InverseProperty("PurchaseType")]
    public virtual ICollection<TTPurchaseOrder> TTPurchaseOrders { get; set; } = new List<TTPurchaseOrder>();
}
