using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMPaymentType")]
public partial class TMPaymentType : BaseAuditableEntity
{
    [Key]
    public int PaymenTypeID { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string PaymenTypeCode { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string PaymenTypeName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }

    [InverseProperty("PaymenType")]
    public virtual ICollection<TTPurchaseOrder> TTPurchaseOrders { get; set; } = new List<TTPurchaseOrder>();
}
