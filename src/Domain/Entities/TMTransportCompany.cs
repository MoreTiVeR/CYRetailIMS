using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMTransportCompany")]
public partial class TMTransportCompany : BaseAuditableEntity
{
    [Key]
    public int TransportID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string TransportNameTH { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string TransportNameEN { get; set; } = null!;

    [StringLength(1000)]
    [Unicode(false)]
    public string? TrackingUrl { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }


    [InverseProperty("Transport")]
    public virtual ICollection<TMTransportPrefixDetail> TMTransportPrefixDetails { get; set; } = new List<TMTransportPrefixDetail>();
}
