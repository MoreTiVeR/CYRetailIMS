using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMPaymentType")]
public partial class TMPaymentType
{
    [Key]
    public int PaymenTypeID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PaymenTypeName { get; set; }

    [StringLength(50)]
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

    [InverseProperty("PaymenType")]
    public virtual ICollection<TTPurchaseOrder> TTPurchaseOrders { get; set; } = new List<TTPurchaseOrder>();
}
