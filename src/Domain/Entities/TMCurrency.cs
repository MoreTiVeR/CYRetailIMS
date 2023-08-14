using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMCurrency")]
public partial class TMCurrency : BaseAuditableEntity
{
    [Key]
    public int CurrencyID { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string CurrencyCode { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string CurrencyName { get; set; } = null!;

    [StringLength(50)]
    public string? CurrencySymbol { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CountryName { get; set; }

    [InverseProperty("Currency")]
    public virtual ICollection<TTPurchaseOrder> TTPurchaseOrders { get; set; } = new List<TTPurchaseOrder>();
}
