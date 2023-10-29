using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMTransportPrefixDetail")]
public partial class TMTransportPrefixDetail : BaseAuditableEntity
{
    [Key]
    public int TransportPrefixID { get; set; }

    public int TransportID { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string PrefixCode { get; set; } = null!;

    [ForeignKey("TransportID")]
    [InverseProperty("TMTransportPrefixDetails")]
    public virtual TMTransportCompany Transport { get; set; } = null!;
}
