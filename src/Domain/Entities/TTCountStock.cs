using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

public partial class TTCountStock : BaseAuditableEntity
{
    [Key]
    public int CountStockID { get; set; }

    public int BranchID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CountDate { get; set; }

    public int TotalCount { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Remark { get; set; }


    [InverseProperty("CountStock")]
    public virtual ICollection<TTCountStockDetail> TTCountStockDetails { get; set; } = new List<TTCountStockDetail>();
}
