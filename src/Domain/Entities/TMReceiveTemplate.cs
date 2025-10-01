using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMReceiveTemplate")]
public partial class TMReceiveTemplate : BaseAuditableEntity
{
    [Key]
    public int ReceiveTempID { get; set; }
    public int BranchID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ShopHeaderNameText { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string ShopHeaderAddressText { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AdditionalHeaderText { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ShopFooterText { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AdditionalFooterText { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string TelephoneNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PrinterName { get; set; }
}
