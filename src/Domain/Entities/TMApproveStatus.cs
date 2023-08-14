using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMApproveStatus")]
public partial class TMApproveStatus : BaseAuditableEntity
{
    [Key]
    public int ApproveStatusID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ApproveStatusName_TH { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string ApproveStatusName_EN { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Description { get; set; }

}
