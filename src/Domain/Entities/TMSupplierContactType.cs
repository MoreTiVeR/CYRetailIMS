using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMSupplierContactType")]
public partial class TMSupplierContactType : BaseAuditableEntity
{
    [Key]
    public int SupplierContactTypeID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string SupplierContactTypeName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }

    [InverseProperty("SupplierContactType")]
    public virtual ICollection<TMSupplierContact> TMSupplierContacts { get; set; } = new List<TMSupplierContact>();
}
