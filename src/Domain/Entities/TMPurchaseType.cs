using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMPurchaseType")]
public partial class TMPurchaseType : BaseAuditableEntity
{
    [Key]
    public int PurchaseTypeID { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string PurchaseTypeCode { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string PurchaseTypeName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Description { get; set; }

    [InverseProperty("PurchaseType")]
    public virtual ICollection<TTPurchaseOrder> TTPurchaseOrders { get; set; } = new List<TTPurchaseOrder>();
}
