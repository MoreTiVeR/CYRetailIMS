using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

public partial class TTAdjustItemTransaction : BaseAuditableEntity
{
    [Key]
    public int AdjustID { get; set; }

    public int AdjustTypeID { get; set; }

    public int ItemID { get; set; }

    public int Qty { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Remark { get; set; }

}
