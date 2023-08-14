using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using CYRetailIMS.Domain.Common;

namespace CYRetailIMS.Domain.Entities;

[Table("TMItemBrand")]
public partial class TMItemBrand : BaseAuditableEntity
{
    [Key]
    public int BrandID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string BrandName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string BrandShortName { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string? Description { get; set; }

    [InverseProperty("Brand")]
    public virtual ICollection<TMItem> TMItems { get; set; } = new List<TMItem>();
}