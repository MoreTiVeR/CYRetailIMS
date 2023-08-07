using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMCurrency")]
public partial class TMCurrency
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

    [InverseProperty("Currency")]
    public virtual ICollection<TTPurchaseOrder> TTPurchaseOrders { get; set; } = new List<TTPurchaseOrder>();
}
